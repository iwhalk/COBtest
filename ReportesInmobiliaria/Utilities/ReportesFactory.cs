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
using System.Drawing.Imaging;

namespace ReportesObra.Utilities
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
        string obserbations = "";
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

            switch (typeof(T).Name)
            {
                case nameof(ReporteDetalles):
                    CrearReporteDetalle(reporte as ReporteDetalles);
                    break;
                case nameof(ReporteAvance):
                    CrearReporteAvance(reporte as ReporteAvance);
                    break;
                default:
                    break;
            }

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
            style.Font.Name = "Times New Roman";

            style = document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("10cm", TabAlignment.Right);

            style = document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Times New Roman";
            style.Font.Size = 9;

            // Create a new style called Reference based on style Normal
            style = document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "3mm";
            style.ParagraphFormat.SpaceAfter = "3mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }

        void CrearReporteDetalle(ReporteDetalles? reporteDetalles)
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
            //dataParametersFrameRight.Left = ShapePosition.Right;
            dataParametersFrameRight.Left = "13.0cm";
            dataParametersFrameRight.RelativeHorizontal = RelativeHorizontal.Margin;
            dataParametersFrameRight.Top = "4.0cm";
            dataParametersFrameRight.RelativeVertical = RelativeVertical.Page;

            // Create the text frame for the data values
            dataValuesFrame = section.AddTextFrame();
            dataValuesFrame.Width = "7.5cm";
            dataValuesFrame.Left = "2.3cm";//"3.5cm"
            dataValuesFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            dataValuesFrame.Top = "4.0cm";
            dataValuesFrame.RelativeVertical = RelativeVertical.Page;

            dataValuesFrameRight = section.AddTextFrame();
            dataValuesFrameRight.Width = "6.5cm";
            dataValuesFrameRight.Left = "16.0cm";//"3.5cm"
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
            Paragraph paragraph = headerFrame.AddParagraph("Reporte Detallado");//Titulo
            paragraph.AddLineBreak();
            paragraph.AddText("Departamento A-101");
            paragraph.Format.Font.Name = "Times New Roman";
            paragraph.Format.Font.Size = 16;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Put parameters in data Frame
            paragraph = dataParametersFrameRight.AddParagraph();
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Font.Size = 10;
            paragraph.AddText("Fecha de creación: ");

            // Put values in data Frame
            paragraph = dataValuesFrameRight.AddParagraph();
            paragraph.AddText(DateTime.Now.ToString("dd/MM/yyyy"));
            //paragraph.AddText(reporteActaEntrega.header.ElementAt(0).FechaHora.ToString("dd/MM/yyyy hh:mm tt"));
            // Add the data separation field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "2.0cm";//"2.0cm"
            paragraph.Format.Font.Size = 10;
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("", TextFormat.Bold);

            //Control de NullReferenceException al llamar a las imágenes
            if (ImageSource.ImageSourceImpl == null)
            {
                ImageSource.ImageSourceImpl = new ImageSharpImageSource<Rgba32>();
            }
            //---------------------------------------------------------------------------------------
            // Create the item table
            tableAreas = section.AddTable();
            tableAreas.Style = "Table";
            tableAreas.Borders.Color = Colors.Gray;
            tableAreas.Borders.Width = 0.3;
            tableAreas.Rows.LeftIndent = 0;
            tableAreas.Rows.Alignment = RowAlignment.Center;
            // Before you can add a row, you must define the columns
            Column column = tableAreas.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = tableAreas.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = tableAreas.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            // Create the header of the table
            Row row = tableAreas.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Format.Font.Size = 10;
            row.Borders.Visible = false;
            //row.Shading.Color = TableColor;
            row.Cells[0].AddParagraph("ID_Element");
            row.Cells[1].AddParagraph("SubElementName");
            row.Cells[2].AddParagraph("Type");

            FillGenericContent(reporteDetalles.SubElementos, tableAreas);         
        }

        void CrearReporteAvance(ReporteAvance? reporteAvance)
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
            //dataParametersFrameRight.Left = ShapePosition.Right;
            dataParametersFrameRight.Left = "13.0cm";
            dataParametersFrameRight.RelativeHorizontal = RelativeHorizontal.Margin;
            dataParametersFrameRight.Top = "4.0cm";
            dataParametersFrameRight.RelativeVertical = RelativeVertical.Page;

            // Create the text frame for the data values
            dataValuesFrame = section.AddTextFrame();
            dataValuesFrame.Width = "7.5cm";
            dataValuesFrame.Left = "2.3cm";//"3.5cm"
            dataValuesFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            dataValuesFrame.Top = "4.0cm";
            dataValuesFrame.RelativeVertical = RelativeVertical.Page;

            dataValuesFrameRight = section.AddTextFrame();
            dataValuesFrameRight.Width = "6.5cm";
            dataValuesFrameRight.Left = "16.0cm";//"3.5cm"
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
            Paragraph paragraph = headerFrame.AddParagraph("Reporte Detallado");//Titulo
            paragraph.AddLineBreak();
            paragraph.AddText("Departamento A-101");
            paragraph.Format.Font.Name = "Times New Roman";
            paragraph.Format.Font.Size = 16;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Put parameters in data Frame
            paragraph = dataParametersFrameRight.AddParagraph();
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Font.Size = 10;
            paragraph.AddText("Fecha de creación: ");

            // Put values in data Frame
            paragraph = dataValuesFrameRight.AddParagraph();
            paragraph.AddText(DateTime.Now.ToString("dd/MM/yyyy"));
            //paragraph.AddText(reporteActaEntrega.header.ElementAt(0).FechaHora.ToString("dd/MM/yyyy hh:mm tt"));
            // Add the data separation field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "2.0cm";//"2.0cm"
            paragraph.Format.Font.Size = 10;
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("", TextFormat.Bold);

            //Control de NullReferenceException al llamar a las imágenes
            if (ImageSource.ImageSourceImpl == null)
            {
                ImageSource.ImageSourceImpl = new ImageSharpImageSource<Rgba32>();
            }
            //---------------------------------------------------------------------------------------
            // Create the item table
            tableAreas = section.AddTable();
            tableAreas.Style = "Table";
            tableAreas.Borders.Color = Colors.Gray;
            tableAreas.Borders.Width = 0.3;
            tableAreas.Rows.LeftIndent = 0;
            tableAreas.Rows.Alignment = RowAlignment.Center;
            // Before you can add a row, you must define the columns
            Column column = tableAreas.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = tableAreas.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = tableAreas.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            // Create the header of the table
            Row row = tableAreas.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Format.Font.Size = 10;
            row.Borders.Visible = false;
            //row.Shading.Color = TableColor;
            row.Cells[0].AddParagraph("ID_Element");
            row.Cells[1].AddParagraph("SubElementName");
            row.Cells[2].AddParagraph("Type");

            //FillGenericContent(reporteDetalles.SubElementos, tableAreas);
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

        void FillGenericContent<T>(List<T> value, Table table, int fontSize = 8)
        {
            Table _table = table;
            //foreach (var item in value)
            string currentName = "";
            string beforeName = "";
            for (int i = 0; i < value.Count; i++)
            {
                var item = value.ElementAt(i);
                Row row = _table.AddRow();
                row.Format.Font.Size = (Unit)fontSize;
                row.VerticalAlignment = VerticalAlignment.Center;
                if (item != null)
                    foreach (var (prop, index) in item.GetType().GetProperties().Select((v, i) => (v, i)))
                    {
                        var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                        if (index >= 1 && index <= 3)
                        {
                            int changedIndex = 1;
                            switch (index)
                            {
                                case 2: changedIndex = 0; currentName = prop.GetValue(item, null)?.ToString(); break;
                                case 3: changedIndex = 2; break;
                            }
                            if (type == typeof(DateTime))
                            {
                                row.Cells[changedIndex].AddParagraph(((DateTime?)prop.GetValue(item, null))?.ToString("dd/MM/yyyy hh:mm:ss tt") ?? "");
                            }
                            if (type == typeof(string))
                            {
                                row.Cells[changedIndex].AddParagraph(prop.GetValue(item, null)?.ToString());
                            }
                            if (type == typeof(bool))
                            {
                                row.Cells[changedIndex].AddParagraph((bool?)prop.GetValue(item, null) ?? false ? "SI" : "NO");
                            }
                            if (type == typeof(int))
                            {
                                row.Cells[changedIndex].AddParagraph(prop.GetValue(item, null)?.ToString());
                            }
                            if (type == typeof(long))
                            {
                                row.Cells[changedIndex].AddParagraph(prop.GetValue(item, null)?.ToString());
                            }
                        }
                        row.Cells[0].Borders.Color = Colors.Black;
                        row.Cells[0].Borders.Visible = false;
                        row.Cells[0].Borders.Left.Width = 1.5;
                        if (!currentName.Equals(beforeName))
                        {
                            if (i == 0)
                            {
                                row.Cells[0].Borders.Color = Colors.Gray;
                                row.Cells[0].Borders.Top.Width = 0.3;
                            }
                            else
                                row.Cells[0].Borders.Top.Width = 1.5;
                        }
                            
                        beforeName = currentName;
                    }
                if(i == value.Count -1)
                    row.Cells[0].Borders.Bottom.Width = 1.5;
                if (i % 2 == 0)
                {
                    //row.Shading.Color= Colors.LightGray;
                    row.Cells[1].Shading.Color = Colors.LightGray;
                    row.Cells[2].Shading.Color = Colors.LightGray;
                }
            }
        }

        int FillGenericContentMedidores<T>(List<T> value, Table table, int tableIndex, string title, int fontSize = 8)
        {
            Table _table = table;
            string lastService = "";
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
                        //Quita las entradas repetidas del EP
                        if (index == 3)
                        {
                            string currentService = prop.GetValue(item, null)?.ToString();
                            if (currentService == lastService)
                            {
                                _table.Rows.RemoveObjectAt(_table.Rows.Count - 1);
                                continue;
                            }
                            else
                                lastService = currentService;
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