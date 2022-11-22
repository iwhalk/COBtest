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
using Microsoft.VisualBasic;

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
                    var currentImage = new BlobClient(new Uri(currentUri)).DownloadContent();
                    rowI.Cells[j].Format.Alignment = ParagraphAlignment.Center;
                    rowI.Cells[j].VerticalAlignment = VerticalAlignment.Center;
                    rowI.Cells[j].AddParagraph().AddImage(ImageSource.FromStream("imagen" + i + j, currentImage.Value.Content.ToStream)).Width = "4.8cm";
                }
                blobUris.Clear();
            }

            document.LastSection.AddPageBreak();

            for (int i = 0; i < reporteActaEntrega.deliverables.Count; i++)
            {
                string tableTitle = reporteActaEntrega.deliverables.ElementAt(i).Entregable;
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
                row2.Cells[0].AddParagraph("Area/Servicio");
                row2.Cells[1].AddParagraph("Cantidad");
                row2.Cells[2].AddParagraph("Observaciones");
                i = FillGenericContent(reporteActaEntrega.deliverables, tableEntregables, i, tableTitle) - 1;

                //A partir de la primera fila de elementos combina las celdas de la tercer columna
                Row elementsRow = tableEntregables.Rows[1];
                elementsRow.Cells[2].MergeDown = tableEntregables.Rows.Count - 2;

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
                    var currentImage = new BlobClient(new Uri(currentUri)).DownloadContent();                    
                    rowI.Cells[j].Format.Alignment = ParagraphAlignment.Center;
                    rowI.Cells[j].VerticalAlignment = VerticalAlignment.Center;
                    rowI.Cells[j].AddParagraph().AddImage(ImageSource.FromStream("imagenD" + i + j, currentImage.Value.Content.ToStream)).Width = "4.8cm";
                }
                blobUris.Clear();

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

            string base64Arrendador;
            Stream? streamArrendador = new MemoryStream();
            string base64Arrendatario;
            Stream? streamArrendatario = new MemoryStream();

            if (reporteActaEntrega.header.ElementAt(1).FirmaArrendatario != null)
            {
                base64Arrendador = reporteActaEntrega.header.ElementAt(1).FirmaArrendatario.Split(',')[1];
                streamArrendador.Write(Convert.FromBase64String(base64Arrendador));
                rowF1.Cells[0].AddParagraph().AddImage(ImageSource.FromStream("Firma Arrendatario", () => streamArrendador)).Width = "4.8cm";
            }
            if (reporteActaEntrega.header.ElementAt(1).FirmaArrendador != null)
            {
                base64Arrendatario = reporteActaEntrega.header.ElementAt(1).FirmaArrendador.Split(',')[1];
                streamArrendatario.Write(Convert.FromBase64String(base64Arrendatario));
                rowF1.Cells[0].AddParagraph().AddImage(ImageSource.FromStream("Firma Arrendador", () => streamArrendatario)).Width = "4.8cm";
            }
            //Stream? streamArrendador = new MemoryStream(Convert.FromBase64String("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"));
            //Stream? streamArrendatario = new MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAqAAAADACAYAAADbV9G2AAAAAXNSR0IArs4c6QAADHdJREFUeF7t2jGOnFUQhdFyACGwA9gBJBATsxWI2QQp7IecBFIiiEkgdYDQL02LYTTjbhtzqf/OsWTJstuu9049xCePX4xvBAgQIECAAAECBIICL4KzjCJAgAABAgQIECAwAtQjIECAAAECBAgQiAoI0Ci3YQQIECBAgAABAgLUGyBAgAABAgQIEIgKCNAot2EECBAgQIAAAQIC1BsgQIAAAQIECBCICgjQKLdhBAgQIECAAAECAtQbIECAAAECBAgQiAoI0Ci3YQQIECBAgAABAgLUGyBAgAABAgQIEIgKCNAot2EECBAgQIAAAQIC1BsgQIAAAQIECBCICgjQKLdhBAgQIECAAAECAtQbIECAAAECBAgQiAoI0Ci3YQQIECBAgAABAgLUGyBAgAABAgQIEIgKCNAot2EECBAgQIAAAQIC1BsgQIAAAQIECBCICgjQKLdhBAgQIECAAAECAtQbIECAAAECBAgQiAoI0Ci3YQQIECBAgAABAgLUGyBAgAABAgQIEIgKCNAot2EECBAgQIAAAQIC1BsgQIAAAQIECBCICgjQKLdhBAgQIECAAAECAtQbIECAAAECBAgQiAoI0Ci3YQQIECBAgAABAgLUGyBAgAABAgQIEIgKCNAot2EECBAgQIAAAQIC1BsgQIAAAQIECBCICgjQKLdhBAgQIECAAAECAtQbIECAAAECBAgQiAoI0Ci3YQQIECBAgAABAgLUGyBAgAABAgQIEIgKCNAot2EECBAgQIAAAQIC1BsgQIAAAQIECBCICgjQKLdhBAgQIECAAAECAtQbIECAAAECBAgQiAoI0Ci3YQQIECBAgAABAgLUGyBAgAABAgQIEIgKCNAot2EECBAgQIAAAQIC1BsgQIAAAQIECBCICgjQKLdhBAgQIECAAAECAtQbIECAAAECBAgQiAoI0Ci3YQQIECBAgAABAgLUGyBAgAABAgQIEIgKCNAot2EECBAgQIAAAQIC1BsgQIAAAQIECBCICgjQKLdhBAgQIECAAAECAtQbIECAAAECBAgQiAoI0Ci3YQQIECBAgAABAgLUGyBAgAABAgQIEIgKCNAot2EECBAgQIAAAQIC1BsgQIAAAQIECBCICgjQKLdhBAgQIECA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"));
            //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(reporteActaEntrega.header.ElementAt(1).FirmaArrendatario);
            //MemoryStream ms = new MemoryStream(plainTextBytes);
            //Image img = Image.FromStream(ms);
            //var asda = "base64:" + System.Convert.ToBase64String(plainTextBytes);

            rowF1.Cells[0].AddParagraph().AddImage(ImageSource.FromStream("Firma Arrendador", () => streamArrendador)).Width = "4.8cm";
            rowF1.Cells[0].AddParagraph().AddImage(ImageSource.FromStream("Firma Arrendatario", () => st)).Width = "4.8cm";
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
    }    
}