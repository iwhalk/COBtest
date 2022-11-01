using MigraDoc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using Shared.Models;
using System.Globalization;
using System.Text;

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
        TextFrame dataParametersFrame;
        TextFrame headerFrame;
        TextFrame dataValuesFrame;
        TextFrame dataParametersFrameFecha;
        TextFrame dataValuesFrameFecha;

        /// <summary>
        /// The table of the MigraDoc document that contains the invoice items.
        /// </summary>
        Table table;
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

            CrearReporteArrendadores(reporte as ReporteArrendadores);

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

        void CrearReporteArrendadores(ReporteArrendadores? reporteArrendadores)
        {
            section.PageSetup.Orientation = Orientation.Landscape;

            headerFrame = section.AddTextFrame();
            headerFrame.Width = "10.0cm";
            headerFrame.Left = ShapePosition.Center;
            headerFrame.RelativeHorizontal = RelativeHorizontal.Page;
            headerFrame.Top = "2.70cm";
            headerFrame.RelativeVertical = RelativeVertical.Page;

            // Create the text frame for the data parameters
            dataParametersFrame = section.AddTextFrame();
            dataParametersFrame.Height = "2.0cm";
            dataParametersFrame.Width = "7.0cm";
            dataParametersFrame.Left = ShapePosition.Left;
            dataParametersFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            dataParametersFrame.Top = "4.0cm";
            dataParametersFrame.RelativeVertical = RelativeVertical.Page;

            // Create the text frame for the data values
            dataValuesFrame = section.AddTextFrame();
            dataValuesFrame.Width = "7.0cm";
            dataValuesFrame.Left = "6.5cm";
            dataValuesFrame.RelativeHorizontal = RelativeHorizontal.Page;
            dataValuesFrame.Top = "4.0cm";
            dataValuesFrame.RelativeVertical = RelativeVertical.Page;

            // Put header in header frame
            Paragraph paragraph = headerFrame.AddParagraph("REPORTE ARRENDADORES");//Titulo
            paragraph.Format.Font.Name = "Calibri";
            paragraph.Format.Font.Size = 13;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Put parameters in data Frame
            paragraph = dataParametersFrame.AddParagraph();
            paragraph.Format.Font.Bold = true;
            paragraph.AddText("Fecha de generaciòn: ");
            paragraph.AddLineBreak();
            paragraph.AddText("Total de Arrendatarios: ");

            // Put values in data Frame
            paragraph = dataValuesFrame.AddParagraph();
            paragraph.AddText(reporteArrendadores.FechaGeneracion.ToString("dd/MM/yyyy hh:mm tt") ?? "");
            paragraph.AddLineBreak();
            paragraph.AddText(reporteArrendadores.Arrendadores.Count().ToString() ?? "");

            // Add the data separation field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "2.0cm";
            paragraph.Format.Font.Size = 10;
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("", TextFormat.Bold);

            // Create the item table
            table = section.AddTable();
            table.Style = "Table";
            table.Borders.Color = TableBorder;
            table.Borders.Width = 0.5;
            table.Rows.LeftIndent = 0;
            table.Rows.Alignment = RowAlignment.Center;

            // Before you can add a row, you must define the columns
            Column column = table.AddColumn("5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("7cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            // Create the header of the table
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Format.Font.Size = 10;
            row.Shading.Color = TableColor;
            row.Cells[0].AddParagraph("Nombre");
            row.Cells[1].AddParagraph("RFC");
            row.Cells[2].AddParagraph("Direccìón");
            row.Cells[3].AddParagraph("Telefono");

            FillGenericContent(reporteArrendadores.Arrendadores);
        }

        void CreateLayout<T>(T reporte)
        {
            // Each MigraDoc document needs at least one section.
            //section = document.AddSection();

            // Create footer
            Paragraph footer = section.Footers.Primary.AddParagraph();
            footer.AddLineBreak();
            footer.AddText("El footer");
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


        void FillGenericContent<T>(List<T> value, int fontSize = 8)
        {
            foreach (var item in value)
            {
                Row row = table.AddRow();
                row.Format.Font.Size = (Unit)fontSize;
                row.VerticalAlignment = VerticalAlignment.Center;
                if (item != null)
                    foreach (var (prop, index) in item.GetType().GetProperties().Select((v, i) => (v, i)))
                    {
                        var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                        if (type == typeof(DateTime))
                        {
                            row.Cells[index].AddParagraph(((DateTime?)prop.GetValue(item, null))?.ToString("dd/MM/yyyy hh:mm:ss tt") ?? "");
                        }
                        if (type == typeof(string))
                        {
                            row.Cells[index].AddParagraph(prop.GetValue(item, null)?.ToString());
                        }
                        if (type == typeof(bool))
                        {
                            row.Cells[index].AddParagraph((bool?)prop.GetValue(item, null) ?? false ? "SI" : "NO");
                        }
                    }
            }
        }
    }
}