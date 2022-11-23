using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MigraDoc;
using MigraDocCore.DocumentObjectModel;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using MigraDocCore.DocumentObjectModel.Shapes;
using MigraDocCore.DocumentObjectModel.Tables;
using MigraDocCore.Rendering;
using PdfSharpCore.Drawing;
using PdfSharpCore.Utils;
//using MigraDoc.DocumentObjectModel;
//using MigraDoc.DocumentObjectModel.Shapes;
//using MigraDoc.DocumentObjectModel.Tables;
//using MigraDoc.Rendering;
using SharedLibrary.Models;
using SixLabors.ImageSharp.PixelFormats;
using SQLitePCL;
using System;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing.Text;
using System.Globalization;
using System.Net;
using System.Text;
using Column = MigraDocCore.DocumentObjectModel.Tables.Column;
using Image = System.Drawing.Image;
using Row = MigraDocCore.DocumentObjectModel.Tables.Row;
using Table = MigraDocCore.DocumentObjectModel.Tables.Table;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore.Scaffolding;
using System.Drawing;
using Color = MigraDocCore.DocumentObjectModel.Color;

namespace ReportesInmobiliaria.Utilities
{
    /// <summary>
    /// Creates the invoice form.
    /// </summary>
    public class ReportesFactory
    {
        /// <summary>
        /// The MigraDoc document that represents the invoice.
        /// </summary>
        Document document;

        readonly static Color TableColor = new Color(255, 175, 175);
        readonly static Color TableBorder = new Color(0, 0, 0);
        readonly static Color TableNoBorder = new Color(255, 255, 255);

        /// <summary>
        /// The text frame of the MigraDoc document that contains the address.
        /// </summary>
        TextFrame dataParametersFrameLeft;
        TextFrame dataParametersFrameRight;
        TextFrame headerFrame;
        TextFrame dataValuesFrame;
        TextFrame dataValuesFrameRight;
        TextFrame dataValueTable;
        TextFrame dataValuesFrameFecha;

        //PARA GUARDAR LOS URIS DE BLOBS
        List<string> blobUris = new List<string>();

        /// <summary>
        /// The table of the MigraDoc document that contains the invoice items.
        /// </summary>
        Table tableAreas;
        Table tableEntregables;
        Table tablaFirmas;
        Section section;

        PageSetup pageSetup;

        /// <summary>
        /// Initializes a new instance of the class BillFrom and opens the specified XML document.
        /// </summary>
        public ReportesFactory()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //invoice = new XmlDocument();
            //invoice.Load(filename);
            //navigator = invoice.CreateNavigator();
        }

        /// <summary>
        /// Creates the invoice document.
        /// </summary>
        public byte[] CrearPdf<T>(T reporte)
        {
            // Create a new MigraDoc document
            document = new Document();
            pageSetup = document.DefaultPageSetup;
            //document.Info.Title = "";
            //document.Info.Subject = "";
            //document.Info.Author = "";
            section = document.AddSection();

            DefineStyles();
            CreateLayout(reporte);

            //CrearReporteArrendadores(reporte as ReporteArrendadores);
            CrearReporte(reporte as ReporteActaEntrega);

            PdfDocumentRenderer pdfRenderer = new(true)
            {
                Document = document
            };

            pdfRenderer.RenderDocument();

            using MemoryStream ms = new();
            pdfRenderer.Save(ms, false);

            return ms.ToArray();
        }

        /// <summary>
        /// Defines the styles used to format the MigraDoc document.
        /// </summary>
        void DefineStyles()
        {
            // Get the predefined style Normal.
            Style style = document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Calibri";

            style = document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Calibri";
            style.Font.Size = 8;

            // Create a new style called Reference based on style Normal
            style = document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "3mm";
            style.ParagraphFormat.SpaceAfter = "3mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }

        //void CrearReporteArrendadores(ReporteArrendadores? reporteArrendadores)
        void CrearReporte(ReporteActaEntrega? reporteActaEntrega)
        {
            section.PageSetup.Orientation = Orientation.Portrait;

            headerFrame = section.AddTextFrame();
            headerFrame.Width = "20.0cm";
            headerFrame.Left = ShapePosition.Center;
            headerFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            headerFrame.Top = "2.70cm";
            headerFrame.RelativeVertical = RelativeVertical.Page;

            // Create the text frame for the data parameters
            dataParametersFrameLeft = section.AddTextFrame();
            dataParametersFrameLeft.Height = "2.0cm";
            dataParametersFrameLeft.Width = "7.0cm";
            dataParametersFrameLeft.Left = ShapePosition.Left;
            dataParametersFrameLeft.RelativeHorizontal = RelativeHorizontal.Margin;
            dataParametersFrameLeft.Top = "4.0cm";
            dataParametersFrameLeft.RelativeVertical = RelativeVertical.Page;

            dataParametersFrameRight = section.AddTextFrame();
            dataParametersFrameRight.Height = "2.0cm";
            dataParametersFrameRight.Width = "6.5cm";
            dataParametersFrameRight.Left = ShapePosition.Right;
            dataParametersFrameRight.RelativeHorizontal = RelativeHorizontal.Margin;
            dataParametersFrameRight.Top = "4.0cm";
            dataParametersFrameRight.RelativeVertical = RelativeVertical.Page;

            // Create the text frame for the data values
            dataValuesFrame = section.AddTextFrame();
            dataValuesFrame.Width = "7.0cm";
            dataValuesFrame.Left = "2.2cm";//"3.5cm"
            dataValuesFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            dataValuesFrame.Top = "4.0cm";
            dataValuesFrame.RelativeVertical = RelativeVertical.Page;

            dataValuesFrameRight = section.AddTextFrame();
            dataValuesFrameRight.Width = "7.0cm";
            dataValuesFrameRight.Left = "12.5cm";//"3.5cm"
            dataValuesFrameRight.RelativeHorizontal = RelativeHorizontal.Margin;
            dataValuesFrameRight.Top = "4.0cm";
            dataValuesFrameRight.RelativeVertical = RelativeVertical.Page;

            dataValueTable = section.AddTextFrame();
            dataValueTable.Width = "5.0cm";
            dataValueTable.Left = ShapePosition.Left;
            dataValueTable.RelativeHorizontal = RelativeHorizontal.Margin;
            dataValueTable.Top = "6.6cm";
            dataValueTable.RelativeVertical = RelativeVertical.Page;

            // Put header in header frame
            Paragraph paragraph = headerFrame.AddParagraph("Acta Entrega Recepción de Inmueble");//Titulo
            paragraph.Format.Font.Name = "Calibri";
            paragraph.Format.Font.Size = 20;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Put parameters in data Frame
            paragraph = dataParametersFrameLeft.AddParagraph();
            paragraph.Format.Font.Bold = true;
            paragraph.AddText("No. Contrato: ");
            paragraph.AddLineBreak();
            paragraph.AddText("Dirección: ");
            paragraph.AddLineBreak();
            paragraph.AddText("Arrendador: ");
            paragraph.AddLineBreak();
            paragraph.AddText("Arrendatario: ");
            paragraph.AddLineBreak();
            paragraph.AddText("Agente: ");

            paragraph = dataParametersFrameRight.AddParagraph();
            paragraph.Format.Font.Bold = true;
            paragraph.AddText("Fecha y Hora: ");
            paragraph.AddLineBreak();
            paragraph.AddText("Tipo de Inmueble: ");
            paragraph.AddLineBreak();
            paragraph.AddText("Habitaciones: ");

            // Put values in data Frame
            paragraph = dataValuesFrame.AddParagraph();
            //paragraph.AddText(reporteArrendadores.FechaGeneracion.ToString("dd/MM/yyyy hh:mm tt") ?? "");
            paragraph.AddText(reporteActaEntrega.header.ElementAt(0).NoContrato);
            paragraph.AddLineBreak();
            paragraph.AddText(reporteActaEntrega.header.ElementAt(0).Direccion);
            paragraph.AddLineBreak();
            paragraph.AddText(reporteActaEntrega.header.ElementAt(0).Arrendador);
            paragraph.AddLineBreak();
            paragraph.AddText(reporteActaEntrega.header.ElementAt(0).Arrendatario);
            paragraph.AddLineBreak();
            paragraph.AddText(reporteActaEntrega.header.ElementAt(0).Agente);

            paragraph = dataValuesFrameRight.AddParagraph();
            paragraph.AddText(reporteActaEntrega.header.ElementAt(0).FechaHora.ToString());
            paragraph.AddLineBreak();
            paragraph.AddText(reporteActaEntrega.header.ElementAt(0).TipoInmueble);
            paragraph.AddLineBreak();
            paragraph.AddText(reporteActaEntrega.header.ElementAt(0).Habitaciones.ToString());
            //paragraph.AddText(reporteArrendadores.Arrendadores.Count().ToString() ?? "");
            //paragraph = dataValueTable.AddParagraph();
            //paragraph.Format.Font.Bold = true;
            //paragraph.AddText(reporteActaEntrega.areas.ElementAt(0).Area);
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "3.0cm";//"2.0cm"
            paragraph.Format.Font.Size = 10;
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("", TextFormat.Bold);

            //Control de NullReferenceException al llamar a las imágenes
            if (MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes.ImageSource.ImageSourceImpl == null)
            {
                MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes.ImageSource.ImageSourceImpl = new ImageSharpImageSource<Rgba32>();
            }
            
            for (int i = 0; i < reporteActaEntrega.areas.Count; i++)
            {
                string tableTitle = reporteActaEntrega.areas.ElementAt(i).Area;
                // Add the data separation field
                paragraph = section.AddParagraph();
                paragraph.Format.SpaceBefore = "1.0cm";
                paragraph.Format.Font.Size = 14;
                paragraph.Style = "Reference";
                paragraph.AddFormattedText(tableTitle, TextFormat.Bold);

                // Create the item table
                tableAreas = section.AddTable();
                tableAreas.Style = "Table";
                tableAreas.Borders.Color = TableBorder;
                tableAreas.Borders.Width = 0.5;
                tableAreas.Rows.LeftIndent = 0;
                tableAreas.Rows.Alignment = RowAlignment.Center;
                // Before you can add a row, you must define the columns
                Column column = tableAreas.AddColumn("3cm");
                column.Format.Alignment = ParagraphAlignment.Center;
                column = tableAreas.AddColumn("4cm");
                column.Format.Alignment = ParagraphAlignment.Center;
                column = tableAreas.AddColumn("4cm");
                column.Format.Alignment = ParagraphAlignment.Center;
                // Create the header of the table
                Row row = tableAreas.AddRow();
                row.HeadingFormat = true;
                row.Format.Alignment = ParagraphAlignment.Center;
                row.Format.Font.Bold = true;
                row.Format.Font.Size = 10;
                row.Shading.Color = TableColor;
                row.Cells[0].AddParagraph("Elemento");
                row.Cells[1].AddParagraph("Cantidad/Descripción");                
                row.Cells[2].AddParagraph("Observaciones");
                

                i = FillGenericContent(reporteActaEntrega.areas, tableAreas, i, tableTitle) - 1;
                //A partir de la primera fila de elementos combina las celdas de la tercer columna
                Row elementsRow = tableAreas.Rows[1];
                elementsRow.Cells[2].MergeDown = tableAreas.Rows.Count - 2;

                //Agrega un espacio y una imagen de la ruta especificada
                paragraph = section.AddParagraph();
                paragraph.Format.SpaceBefore = "0.6cm";
                paragraph.Format.Font.Size = 11;
                paragraph.Style = "Reference";
                paragraph.AddFormattedText("Fotografías", TextFormat.Bold);

                Table tableImages = section.AddTable();
                tableImages.Style = "Table";
                tableImages.Rows.LeftIndent = 0;
                tableImages.Rows.Alignment = RowAlignment.Center;
                Column columnI = tableImages.AddColumn("5cm");
                columnI.Format.Alignment = ParagraphAlignment.Center;
                columnI = tableImages.AddColumn("5cm");
                columnI.Format.Alignment = ParagraphAlignment.Center;
                columnI = tableImages.AddColumn("5cm");
                columnI.Format.Alignment = ParagraphAlignment.Center;
                Row rowI = tableImages.AddRow();
                for (int j = 0; j < blobUris.Count; j++) {
                    if (j == 3)
                        break;
                    string currentUri = blobUris.ElementAt(j);
                    if (currentUri.Contains(".webp"))
                        continue;
                    var currentImage = new BlobClient(new Uri(currentUri)).DownloadContent();
                    rowI.Cells[j].Format.Alignment = ParagraphAlignment.Center;
                    rowI.Cells[j].VerticalAlignment = VerticalAlignment.Center;
                    rowI.Cells[j].AddParagraph().AddImage(ImageSource.FromStream("imagen" + i + j, currentImage.Value.Content.ToStream)).Width = "4.8cm";
                }
                blobUris.Clear();
            }


            int contadorTabla = 0;
            for (int i = 0; i < reporteActaEntrega.deliverables.Count; i++)
            {
                string tableTitle = reporteActaEntrega.deliverables.ElementAt(i).Entregable;
                //if (tableTitle.Contains("Llaves"))
                //    tableTitle = "Llaves de habitación";
                //else if (tableTitle.Contains("Medidores"))
                //    tableTitle = "Medidores";
                paragraph = section.AddParagraph();
                if (i != 0)
                    paragraph.Format.SpaceBefore = "1.0cm";
                paragraph.Format.Font.Size = 14;
                paragraph.Style = "Reference";
                paragraph.AddFormattedText(tableTitle, TextFormat.Bold);
                //SECOND TABLE
                tableEntregables = section.AddTable();
                tableEntregables.Style = "Table";
                tableEntregables.Borders.Color = TableBorder;
                tableEntregables.Borders.Width = 0.5;
                tableEntregables.Rows.LeftIndent = 0;
                tableEntregables.Rows.Alignment = RowAlignment.Center;
                Column column2 = tableEntregables.AddColumn("3cm");
                column2.Format.Alignment = ParagraphAlignment.Center;
                column2 = tableEntregables.AddColumn("4cm");
                column2.Format.Alignment = ParagraphAlignment.Center;
                column2 = tableEntregables.AddColumn("4cm");
                column2.Format.Alignment = ParagraphAlignment.Center;
                Row row2 = tableEntregables.AddRow();
                row2.HeadingFormat = true;
                row2.Format.Alignment = ParagraphAlignment.Center;
                row2.Format.Font.Bold = true;
                row2.Format.Font.Size = 10;
                row2.Shading.Color = TableColor;
                if (!tableTitle.Contains("Medidores"))
                {
                    row2.Cells[0].AddParagraph("Area");
                    row2.Cells[1].AddParagraph("Cantidad");
                    row2.Cells[2].AddParagraph("Observaciones");
                    i = FillGenericContent(reporteActaEntrega.deliverables, tableEntregables, i, tableTitle) - 1;
                }
                else
                {
                    row2.Cells[0].AddParagraph("Servicio");
                    row2.Cells[1].AddParagraph("No. Serie");
                    row2.Cells[2].AddParagraph("Cantidad");
                    i = FillGenericContentMedidores(reporteActaEntrega.deliverables, tableEntregables, i, tableTitle) - 1;
                }
                contadorTabla++;

                //A partir de la primera fila de elementos combina las celdas de la tercer columna
                if (!tableTitle.Contains("Medidores"))
                {
                    Row elementsRow = tableEntregables.Rows[1];
                    elementsRow.Cells[2].MergeDown = tableEntregables.Rows.Count - 2;
                }

                paragraph = section.AddParagraph();
                paragraph.Format.SpaceBefore = "0.6cm";
                paragraph.Format.Font.Size = 11;
                paragraph.Style = "Reference";
                paragraph.AddFormattedText("Fotografías", TextFormat.Bold);

                Table tableImages = section.AddTable();
                tableImages.Style = "Table";
                tableImages.Rows.LeftIndent = 0;
                tableImages.Rows.Alignment = RowAlignment.Center;
                Column columnI = tableImages.AddColumn("5cm");
                columnI.Format.Alignment = ParagraphAlignment.Center;
                columnI = tableImages.AddColumn("5cm");
                columnI.Format.Alignment = ParagraphAlignment.Center;
                Row rowI = tableImages.AddRow();

                for (int j = 0; j < blobUris.Count; j++)
                {
                    if (j == 3)
                        break;
                    string currentUri = blobUris.ElementAt(j);
                    if (currentUri.Contains(".webp"))
                        continue;
                    var currentImage = new BlobClient(new Uri(currentUri)).DownloadContent();                    
                    rowI.Cells[j].Format.Alignment = ParagraphAlignment.Center;
                    rowI.Cells[j].VerticalAlignment = VerticalAlignment.Center;
                    rowI.Cells[j].AddParagraph().AddImage(ImageSource.FromStream("imagenD" + i + j, currentImage.Value.Content.ToStream)).Width = "4.8cm";
                }
                blobUris.Clear();

                if (contadorTabla == 1)
                    document.LastSection.AddPageBreak();

                //rowI.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                //rowI.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                //rowI.Cells[0].AddParagraph().AddImage(ImageSource.FromFile(Environment.CurrentDirectory + @"\Imagenes\key.jpg")).Width = "4.2cm";
                //rowI.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                //rowI.Cells[1].VerticalAlignment = VerticalAlignment.Center;
                //rowI.Cells[1].AddParagraph().AddImage(ImageSource.FromFile(Environment.CurrentDirectory + @"\Imagenes\medidor.jpg")).Width = "4.2cm";
            }            

            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "1.0cm";
            paragraph.Format.Font.Size = 16;
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("Firmas", TextFormat.Bold);

            tablaFirmas = section.AddTable();
            tablaFirmas.Style = "Table";
            tablaFirmas.Borders.Color = TableBorder;
            tablaFirmas.Borders.Width = 0.5;
            tablaFirmas.Rows.LeftIndent = 0;
            tablaFirmas.Rows.Alignment = RowAlignment.Center;
            Column columnF = tablaFirmas.AddColumn("7cm");
            columnF.Format.Alignment = ParagraphAlignment.Center;
            columnF = tablaFirmas.AddColumn("7cm");
            columnF.Format.Alignment = ParagraphAlignment.Center;
            Row rowF = tablaFirmas.AddRow();
            rowF.Format.Font.Size = 10;
            rowF.VerticalAlignment = VerticalAlignment.Center;
            rowF.Cells[0].AddParagraph(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
            rowF.Cells[1].AddParagraph(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
            Row rowF1 = tablaFirmas.AddRow();
            rowF1.Format.Font.Size = 68;
            rowF1.VerticalAlignment = VerticalAlignment.Center;
            //rowF1.Cells[1].AddParagraph().AddImage(ImageSource.FromFile(Environment.CurrentDirectory + @"\Images\pngegg.png")).Width = "4.2cm";

            string base64Arrendador;
            string base64Arrendatario;

            if (!string.IsNullOrWhiteSpace(reporteActaEntrega.header.ElementAt(0).FirmaArrendatario))
            {
                base64Arrendador = reporteActaEntrega.header.ElementAt(0).FirmaArrendatario.Split(',')[1];
                Stream? streamArrendador = new MemoryStream(Convert.FromBase64String(base64Arrendador));
                rowF1.Cells[0].AddParagraph().AddImage(ImageSource.FromStream("Firma Arrendatario", () => streamArrendador)).Width = "8cm";
            }
            if (!string.IsNullOrWhiteSpace(reporteActaEntrega.header.ElementAt(0).FirmaArrendador))
            {
                base64Arrendatario = reporteActaEntrega.header.ElementAt(0).FirmaArrendador.Split(',')[1];
                Stream? streamArrendatario = new MemoryStream(Convert.FromBase64String(base64Arrendatario));
                Image imageBackground = Image.FromFile(Environment.CurrentDirectory + @"\Images\pngegg.png");
                Image arrentatarioImg = Image.FromStream(streamArrendatario);
                Image img = new Bitmap(arrentatarioImg.Width, 400);
                Rectangle limit = new Rectangle((arrentatarioImg.Width - imageBackground.Width) / 2, 0, 400, 400);
                Rectangle limit2 = new Rectangle(0, (img.Height - arrentatarioImg.Height)/2, arrentatarioImg.Width, arrentatarioImg.Height);
                using (Graphics gr = Graphics.FromImage(img))
                {
                    gr.DrawImage(imageBackground,limit);
                    gr.DrawImage(arrentatarioImg,limit2);
                }
                img.Save(Environment.CurrentDirectory + "\\Images\\FirmaSello.png");
                //rowF1.Cells[1].AddParagraph().AddImage(ImageSource.FromStream("Firma Arrendador", () => streamArrendatario)).Width = "10cm";
                rowF1.Cells[1].AddParagraph().AddImage(ImageSource.FromFile(Environment.CurrentDirectory + @"\Images\FirmaSello.png")).Width = "8cm";
            }

            Row rowF2 = tablaFirmas.AddRow();
            rowF2.Format.Font.Size = 12;
            rowF2.VerticalAlignment = VerticalAlignment.Center;
            rowF2.Cells[0].AddParagraph(reporteActaEntrega.header.ElementAt(0).Arrendatario + " (Arrendatario)");
            rowF2.Cells[1].AddParagraph(reporteActaEntrega.header.ElementAt(0).Arrendador + " (Arrendador)");
        }

        void CreateLayout<T>(T reporte)
        {
            // Each MigraDoc document needs at least one section.
            //section = document.AddSection();

            // Create footer
            Paragraph footer = section.Footers.Primary.AddParagraph();
            footer.AddLineBreak();
            footer.AddText("SOF2245");
            footer.AddLineBreak();
            footer.AddText("Pagina ");
            footer.AddPageField();
            footer.AddText(" de ");
            footer.AddNumPagesField();
            footer.Format.Font.Size = 8;
            footer.Format.Alignment = ParagraphAlignment.Center;

            // Put a logo in the header
            //Image image = section.Headers.Primary.AddImage(Path.Combine(Environment.CurrentDirectory, @"Imagenes\", "ferromex.png"));
            //image.Height = "0.75cm"; image.Width = "5.25cm";
            //image.LockAspectRatio = true;
            //image.RelativeVertical = RelativeVertical.Margin;
            //image.RelativeHorizontal = RelativeHorizontal.Margin;
            //image.Top = ShapePosition.Top;
            //image.Left = ShapePosition.Right;
            //image.WrapFormat.Style = WrapStyle.Through;
        }

        /// <summary>
        /// Creates the static parts of the invoice.
        /// </summary>


        int FillGenericContent<T>(List<T> value, Table table, int tableIndex, string title, int fontSize = 8)
        {
            Table _table = table;
            //foreach (var item in value)
            for(int i = tableIndex; i < value.Count; i++)
            {
                var item = value.ElementAt(i);
                Row row = _table.AddRow();
                row.Format.Font.Size = (Unit)fontSize;
                row.VerticalAlignment = VerticalAlignment.Center;
                if (item != null)
                    foreach (var (prop, index) in item.GetType().GetProperties().Select((v, i) => (v, i)))
                    {
                        var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                        if (index == 2)
                        {
                            string currentTitle = prop.GetValue(item, null)?.ToString();
                            if (currentTitle != title) {                                
                                row.Format.Font.Size = (Unit)0;
                                return i;
                            }                                
                        }
                        if (index >= 3 && index <= 5)
                        {                            
                            if (type == typeof(DateTime))
                            {
                                row.Cells[index - 3].AddParagraph(((DateTime?)prop.GetValue(item, null))?.ToString("dd/MM/yyyy hh:mm:ss tt") ?? "");
                            }
                            if (type == typeof(string))
                            {
                                row.Cells[index - 3].AddParagraph(prop.GetValue(item, null)?.ToString());
                            }
                            if (type == typeof(bool))
                            {
                                row.Cells[index - 3].AddParagraph((bool?)prop.GetValue(item, null) ?? false ? "SI" : "NO");
                            }
                            if (type == typeof(int))
                            {
                                row.Cells[index - 3].AddParagraph(prop.GetValue(item, null)?.ToString());
                            }
                            if (type == typeof(long))
                            {
                                row.Cells[index - 3].AddParagraph(prop.GetValue(item, null)?.ToString());
                            }
                        }
                        if (index == 6)
                        {
                            string currentUri = prop.GetValue(item, null)?.ToString();
                            if(currentUri != null)
                                if (currentUri.Contains("http"))
                                {
                                    blobUris.Add(currentUri);
                                }
                        }
                    }
            }
            return value.Count;
        }

        int FillGenericContentMedidores<T>(List<T> value, Table table, int tableIndex, string title, int fontSize = 8)
        {
            Table _table = table;
            //foreach (var item in value)
            for (int i = tableIndex; i < value.Count; i++)
            {
                var item = value.ElementAt(i);
                Row row = _table.AddRow();
                row.Format.Font.Size = (Unit)fontSize;
                row.VerticalAlignment = VerticalAlignment.Center;
                if (item != null)
                    foreach (var (prop, index) in item.GetType().GetProperties().Select((v, i) => (v, i)))
                    {
                        var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                        if (index == 2)
                        {
                            string currentTitle = prop.GetValue(item, null)?.ToString();
                            if (currentTitle != title)
                            {
                                row.Format.Font.Size = (Unit)0;
                                return i;
                            }
                        }
                        if (index >= 3 && index <= 5)
                        {
                            int indexChanged = 3;
                            if (index == 4)
                                indexChanged = 5;
                            else if (index == 5)
                                indexChanged = 4;
                            if (type == typeof(DateTime))
                            {
                                row.Cells[indexChanged - 3].AddParagraph(((DateTime?)prop.GetValue(item, null))?.ToString("dd/MM/yyyy hh:mm:ss tt") ?? "");
                            }
                            if (type == typeof(string))
                            {
                                row.Cells[indexChanged - 3].AddParagraph(prop.GetValue(item, null)?.ToString());
                            }
                            if (type == typeof(bool))
                            {
                                row.Cells[indexChanged - 3].AddParagraph((bool?)prop.GetValue(item, null) ?? false ? "SI" : "NO");
                            }
                            if (type == typeof(int))
                            {
                                row.Cells[indexChanged - 3].AddParagraph(prop.GetValue(item, null)?.ToString());
                            }
                            if (type == typeof(long))
                            {
                                row.Cells[indexChanged - 3].AddParagraph(prop.GetValue(item, null)?.ToString());
                            }
                        }
                        if (index == 6)
                        {
                            string currentUri = prop.GetValue(item, null)?.ToString();
                            if (currentUri != null)
                                if (currentUri.Contains("http"))
                                {
                                    blobUris.Add(currentUri);
                                }
                        }
                    }
            }
            return value.Count;
        }
    }    
}