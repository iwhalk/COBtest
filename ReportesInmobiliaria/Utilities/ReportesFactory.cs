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

            switch (typeof(T).Name)
            {
                case nameof(ReporteOperacionesDetalle):
                    CrearReporteOperacionDetalle(reporte as ReporteOperacionesDetalle);
                    break;
                case nameof(ReporteDescuentosResumen):
                    CrearReporteDescuentosResumen(reporte as ReporteDescuentosResumen);
                    break;
                case nameof(ReporteTransacciones):
                    CrearReporteTransacciones(reporte as ReporteTransacciones);
                    break;
                case nameof(ReporteIngresosResumen):
                    CrearReporteIngresosResumen(reporte as ReporteIngresosResumen);
                    break;
                case nameof(ReporteDescuentosDetalle):
                    CrearReporteDescuentosDetalle(reporte as ReporteDescuentosDetalle);
                    break;
                case nameof(ReporteConcentradoTags):
                    CrearReporteConcentradoTags(reporte as ReporteConcentradoTags);
                    break;
                case nameof(ReporteTransaccionesOperativo):
                    CrearReporteTransaccionesOperativo(reporte as ReporteTransaccionesOperativo);
                    break;
                case nameof(ReporteActividadUsuarios):
                    CrearReporteActividadUsuarios(reporte as ReporteActividadUsuarios);
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

        void CreateLayout<T>(T reporte)
        {
            // Each MigraDoc document needs at least one section.
            //section = document.AddSection();

            // Create footer
            Paragraph footer = section.Footers.Primary.AddParagraph();
            footer.AddLineBreak();
            footer.AddText("D.R.© Ferrocarril Mexicano S.A. de C.V., Bosque de Ciruelos no. 99, Col. Bosques de las Lomas, México D.F. C.P. 11700");
            footer.AddLineBreak();
            footer.AddText("Pagina ");
            footer.AddPageField();
            footer.AddText(" de ");
            footer.AddNumPagesField();
            footer.Format.Font.Size = 8;
            footer.Format.Alignment = ParagraphAlignment.Center;

            // Put a logo in the header
            if (typeof(T) != typeof(ReporteTransaccionesOperativo) && typeof(T) != typeof(ReporteOperacionesDetalle) && typeof(T) != typeof(ReporteActividadUsuarios))
            {
                Image image = section.Headers.Primary.AddImage(Path.Combine(Environment.CurrentDirectory, @"Imagenes\", "ferromex.png"));
                image.Height = "0.75cm"; image.Width = "5.25cm";
                image.LockAspectRatio = true;
                image.RelativeVertical = RelativeVertical.Margin;
                image.RelativeHorizontal = RelativeHorizontal.Margin;
                image.Top = ShapePosition.Top;
                image.Left = ShapePosition.Right;
                image.WrapFormat.Style = WrapStyle.Through;
            }
        }

        /// <summary>
        /// Creates the static parts of the invoice.
        /// </summary>
        void CrearReporteDescuentosDetalle(ReporteDescuentosDetalle? reporteDescuentoDetalle)
        {
            section.PageSetup.Orientation = Orientation.Landscape;
            section.PageSetup.BottomMargin = 20;
            section.PageSetup.TopMargin = 10;
            section.PageSetup.FooterDistance = 1;

            DataParameters(4);
            FechaGeneracion();

            Paragraph paragraph = headerFrame.AddParagraph("");//DESCUENTOS DETALLE AMARRE
            paragraph.Format.Font.Name = "Calibri";
            paragraph.Format.Font.Size = 13;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Put parameters in data Frame
            paragraph = dataParametersFrame.AddParagraph();
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Font.Size = 9;
            paragraph.AddText("Fecha Inicio:");
            paragraph.AddLineBreak();
            paragraph.AddText("Fecha Fin:");
            paragraph.AddLineBreak();
            paragraph.AddText("Total de Registros:");

            // Put values in data Frame
            paragraph = dataValuesFrame.AddParagraph();
            paragraph.Format.Font.Size = 9;
            paragraph.AddText(reporteDescuentoDetalle.FechaInicio.ToString("dd/MM/yyyy hh:mm tt"));
            paragraph.AddLineBreak();
            paragraph.AddText(reporteDescuentoDetalle.FechaFin.ToString("dd/MM/yyyy hh:mm tt"));
            paragraph.AddLineBreak();
            paragraph.AddText(reporteDescuentoDetalle.DescuentosDetalle?.Count.ToString());

            //// Add the data separation field
            //paragraph = section.AddParagraph();
            //paragraph.Format.SpaceBefore = "1cm";
            //paragraph.Style = "Reference";
            //paragraph.AddFormattedText("", TextFormat.Bold);
            //paragraph.AddTab();
            //paragraph.AddText("Cologne, ");
            //paragraph.AddDateField("dd.MM.yyyy");

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

            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            // Create the header of the table
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Format.Font.Size = 8;
            row.Shading.Color = TableColor;
            row.Cells[0].AddParagraph("Tag");
            row.Cells[1].AddParagraph("Placa");
            row.Cells[2].AddParagraph("No. Económico");
            row.Cells[3].AddParagraph("Cuerpo");
            row.Cells[4].AddParagraph("Fecha");
            row.Cells[5].AddParagraph("Carril");
            row.Cells[6].AddParagraph("Clase");
            row.Cells[7].AddParagraph("Tarifa");
            row.Cells[8].AddParagraph("Tarifa Desc.");
            row.Cells[9].AddParagraph("Descuento");

            //this.table.SetEdge(0, 0, 6, 2, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
            FillGenericContent(reporteDescuentoDetalle.DescuentosDetalle);
        }
        void CrearReporteIngresosResumen(ReporteIngresosResumen? reporteIngresoResumen)
        {
            section.PageSetup.Orientation = Orientation.Portrait;

            DataParameters(1);
            FechaGeneracion();

            // Put header in header frame
            Paragraph paragraph = headerFrame.AddParagraph("");//RESUMEN INGRESOS FERROMEX
            paragraph.Format.Font.Name = "Calibri";
            paragraph.Format.Font.Size = 13;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Put parameters in data Frame
            paragraph = dataParametersFrame.AddParagraph();
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Font.Size = 9;
            paragraph.AddText("Fecha Inicio:");
            paragraph.AddLineBreak();
            paragraph.AddText("Fecha Fin:");
            paragraph.AddLineBreak();
            paragraph.AddText("Total de Registros:");

            // Put values in data Frame
            paragraph = dataValuesFrame.AddParagraph();
            paragraph.Format.Font.Size = 9;
            paragraph.AddText(reporteIngresoResumen.FechaInicio.ToString("dd/MM/yyyy hh:mm tt"));
            paragraph.AddLineBreak();
            paragraph.AddText(reporteIngresoResumen.FechaFin.ToString("dd/MM/yyyy hh:mm tt"));
            paragraph.AddLineBreak();
            paragraph.AddText(reporteIngresoResumen.IngresosResumen.Count.ToString());

            // Add the data separation field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "1.5cm";
            paragraph.Format.Font.Size = 2;
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("", TextFormat.Bold);

            // Create the item table
            table = section.AddTable();
            table.Style = "Table";
            table.Borders.Color = TableBorder;
            table.Borders.Width = 0.5;
            table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            Column column = table.AddColumn("8cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("4cm");
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
            row.Cells[0].AddParagraph("Medios de pago");
            row.Cells[1].AddParagraph("Cantidad");
            row.Cells[2].AddParagraph("Egresos");

            //this.table.SetEdge(0, 0, 6, 2, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
            FillGenericContent(reporteIngresoResumen.IngresosResumen, 6);

        }

        void CrearReporteConcentradoTags(ReporteConcentradoTags? reporteConcentradoTags)
        {
            section.PageSetup.Orientation = Orientation.Portrait;

            DataParameters(1);
            FechaGeneracion();

            // Put header in header frame
            Paragraph paragraph = headerFrame.AddParagraph("");//MANTENIMIENTO TAGS
            paragraph.Format.Font.Name = "Calibri";
            paragraph.Format.Font.Size = 13;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Put parameters in data Frame
            paragraph = dataParametersFrame.AddParagraph();
            paragraph.Format.Font.Bold = true;
            paragraph.AddText("Generó reporte:");

            // Put values in data Frame
            paragraph = dataValuesFrame.AddParagraph();
            paragraph.AddText(reporteConcentradoTags.Usuario ?? "");

            // Add the data separation field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "1.0cm";
            paragraph.Format.Font.Size = 2;
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("", TextFormat.Bold);

            // Create the item table
            table = section.AddTable();
            table.Style = "Table";
            table.Borders.Color = TableBorder;
            table.Borders.Width = 0.5;
            table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            Column column = table.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("4cm");
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
            row.Cells[0].AddParagraph("Tag");
            row.Cells[1].AddParagraph("Placa");
            row.Cells[2].AddParagraph("No. Económico");
            row.Cells[3].AddParagraph("Estatus");
            row.Cells[4].AddParagraph("Fecha de Registro");

            FillGenericContent(reporteConcentradoTags.ConcentradoTags);

        }

        void CreateTransaccionesDetalle(ReporteTransaccionesDetalle reporteTransaccionesDetalle)
        {
            section.PageSetup.Orientation = Orientation.Portrait;

            // Create the text frame for the header
            dataValuesFrame = section.AddTextFrame();
            dataValuesFrame.Width = "7.0cm";
            dataValuesFrame.Left = "8.0cm";
            dataValuesFrame.RelativeHorizontal = RelativeHorizontal.Page;
            dataValuesFrame.Top = "3.70cm";
            dataValuesFrame.RelativeVertical = RelativeVertical.Page;

            // Create the text frame for the data parameters
            dataParametersFrame = section.AddTextFrame();
            dataParametersFrame.Height = "3.0cm";
            dataParametersFrame.Width = "7.0cm";
            dataParametersFrame.Left = ShapePosition.Left;
            dataParametersFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            dataParametersFrame.Top = "3.70cm";
            dataParametersFrame.RelativeVertical = RelativeVertical.Page;

            // Create the text frame for the data values
            headerFrame = section.AddTextFrame();
            headerFrame.Width = "7.0cm";
            headerFrame.Left = ShapePosition.Center;
            headerFrame.RelativeHorizontal = RelativeHorizontal.Page;
            headerFrame.Top = "2.70cm";
            headerFrame.RelativeVertical = RelativeVertical.Page;

            // Put header in header frame
            Paragraph paragraph = headerFrame.AddParagraph("TRANSACCIONES DETALLE");
            paragraph.Format.Font.Name = "Calibri";
            paragraph.Format.Font.Size = 13;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Put parameters in data Frame
            paragraph = dataParametersFrame.AddParagraph();
            paragraph.Format.Font.Bold = true;
            paragraph.AddText("Fecha Inicio");
            paragraph.AddLineBreak();
            paragraph.AddText("Fecha Fin");
            paragraph.AddLineBreak();
            paragraph.AddText("Fecha de Generacion Reporte");
            paragraph.AddLineBreak();
            paragraph.AddText("Cantidad de transacciones");

            // Put values in data Frame
            paragraph = dataValuesFrame.AddParagraph();
            paragraph.AddText(reporteTransaccionesDetalle.FechaInicio.ToString("dd/MM/yyyy hh:mm tt"));
            paragraph.AddLineBreak();
            paragraph.AddText(reporteTransaccionesDetalle.FechaFin.ToString("dd/MM/yyyy hh:mm tt"));
            paragraph.AddLineBreak();
            paragraph.AddText(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
            paragraph.AddLineBreak();
            paragraph.AddText(reporteTransaccionesDetalle.TransaccionesDetalle.Count.ToString());

            // Add the data separation field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "2.5cm";
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("", TextFormat.Bold);

            // Create the item table
            table = section.AddTable();
            table.Style = "Table";
            table.Borders.Color = TableBorder;
            table.Borders.Width = 0.5;
            table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            Column column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("2.0cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            //Fecha, Carril, Clase Pre, Clase Cajero, Clase POST, Medio de Pago, Tarifa, Tag
            // Create the header of the table
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Format.Font.Size = 8;
            row.Shading.Color = TableColor;
            row.Cells[0].AddParagraph("Fecha");
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[1].AddParagraph("Carril");
            row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[2].AddParagraph("Clase Pre");
            row.Cells[2].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[3].AddParagraph("Clase Cajero");
            row.Cells[4].AddParagraph("Clase POST");
            row.Cells[5].AddParagraph("Medio de Pago");
            row.Cells[6].AddParagraph("Tarifa");
            row.Cells[6].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[7].AddParagraph("Tag");
            row.Cells[7].VerticalAlignment = VerticalAlignment.Center;

            //this.table.SetEdge(0, 0, 6, 2, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
            FillGenericContent(reporteTransaccionesDetalle.TransaccionesDetalle, 5);

        }

        void CrearReporteDescuentosResumen(ReporteDescuentosResumen? reporteDescuentoResumen)
        {
            // set orientation
            section.PageSetup.Orientation = Orientation.Landscape;

            DataParameters(3);
            FechaGeneracion();

            // Put header in header frame
            Paragraph paragraph = headerFrame.AddParagraph("");//DESCUENTOS AMARRE RESUMEN
            paragraph.Format.Font.Name = "Calibri";
            paragraph.Format.Font.Size = 13;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Put parameters in data Frame
            paragraph = dataParametersFrame.AddParagraph();
            paragraph.Format.Font.Bold = true;
            paragraph.AddText("Fecha de Inicio");
            paragraph.AddLineBreak();
            paragraph.AddText("Fecha de Fin");
            paragraph.AddLineBreak();
            paragraph.AddText("Cantidad de transacciones");
            paragraph.AddLineBreak();

            // Put values in data Frame
            paragraph = dataValuesFrame.AddParagraph();
            paragraph.AddText(reporteDescuentoResumen.FechaInicio.ToString("dd/MM/yyyy hh:mm tt"));
            paragraph.AddLineBreak();
            paragraph.AddText(reporteDescuentoResumen.FechaFin.ToString("dd/MM/yyyy hh:mm tt"));
            paragraph.AddLineBreak();
            paragraph.AddText(reporteDescuentoResumen.DescuentosResumen?.Count.ToString());
            paragraph.AddLineBreak();

            // Add the data separation field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "1.5cm";
            paragraph.Format.Font.Size = 2;
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("", TextFormat.Bold);

            // Create the item table
            table = section.AddTable();
            table.Style = "Table";
            table.Borders.Color = TableBorder;
            table.Borders.Width = 0.5;
            table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            Column column = table.AddColumn("5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("2.25cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("2.25cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("2.75cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.25cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.25cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.25cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("2.75cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.25cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.25cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.25cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.25cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            // Create the header of the table
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Format.Font.Size = 8;
            row.Format.Borders.Visible = false;
            row.Cells[0].AddParagraph("");
            row.Cells[0].MergeRight = 2;
            row.Cells[3].AddParagraph(reporteDescuentoResumen?.DireccionEntrada ?? "");
            row.Cells[3].MergeRight = 3;
            row.Cells[7].AddParagraph(reporteDescuentoResumen?.DireccionSalida ?? "");
            row.Cells[7].MergeRight = 4;
            row.Cells[12].AddParagraph("");
            row.Cells[12].Borders.Visible = false;

            // Create the header of the table
            row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Format.Font.Size = 8;
            row.Shading.Color = TableColor;
            row.Cells[0].AddParagraph("Tag");
            row.Cells[1].AddParagraph("Placa");
            row.Cells[2].AddParagraph("No. Ecónomico");
            row.Cells[3].AddParagraph("Fecha");
            row.Cells[4].AddParagraph("Carril");
            row.Cells[5].AddParagraph("Clase");
            row.Cells[6].AddParagraph("Tarifa");
            row.Cells[7].AddParagraph("Fecha");
            row.Cells[8].AddParagraph("Carril");
            row.Cells[9].AddParagraph("Clase");
            row.Cells[10].AddParagraph("Tarifa");
            row.Cells[11].AddParagraph("Tarifa Desc.");
            row.Cells[12].AddParagraph("Descuento");

            table.SetEdge(0, 0, 1, 1, Edge.Top, BorderStyle.Single, 0.5, Colors.White);
            table.SetEdge(0, 0, 1, 1, Edge.Left, BorderStyle.Single, 0.5, Colors.White);
            table.SetEdge(9, 0, 1, 1, Edge.Top, BorderStyle.Single, 0.5, Colors.White);
            table.SetEdge(9, 0, 1, 1, Edge.Right, BorderStyle.Single, 0.5, Colors.White);

            FillGenericContent(reporteDescuentoResumen.DescuentosResumen);
        }

        void CrearReporteTransaccionesOperativo(ReporteTransaccionesOperativo? reporteTransaccionesOperativo)
        {
            section.PageSetup.Orientation = Orientation.Portrait;
            Column column;
            Row row;

            DataParameters(1);
            FechaGeneracion();

            // Put header in header frame
            Paragraph paragraph = headerFrame
            .AddParagraph($"TRANSACCIONES DIA");
            paragraph.Format.Font.Name = "Calibri";
            paragraph.Format.Font.Size = 13;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Add the data separation field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "0.25cm";
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("", TextFormat.Bold);
            //paragraph.AddTab();
            //paragraph.AddText("Cologne, ");
            //paragraph.AddDateField("dd.MM.yyyy");

            // Put parameters in data Frame
            paragraph = dataParametersFrame.AddParagraph();
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Font.Size = 10;
            paragraph.AddText("Plaza:");
            paragraph.AddLineBreak();
            paragraph.AddText("Fecha Inicio:");
            paragraph.AddLineBreak();
            paragraph.AddText("Fecha Fin:");
            paragraph.AddLineBreak();
            paragraph.AddText("Total de Registros:");

            // Put values in data Frame
            paragraph = dataValuesFrame.AddParagraph();
            paragraph.Format.Font.Size = 10;
            paragraph.AddText(reporteTransaccionesOperativo?.Plaza ?? "");
            paragraph.AddLineBreak();
            paragraph.AddText(reporteTransaccionesOperativo == null ? DateTime.MinValue.ToString("dd/MM/yyyy hh:mm tt") : ((DateTime)reporteTransaccionesOperativo.FechaInicio).ToString("dd/MM/yyyy hh:mm tt"));
            paragraph.AddLineBreak();
            paragraph.AddText(reporteTransaccionesOperativo == null ? DateTime.MinValue.ToString("dd/MM/yyyy hh:mm tt") : ((DateTime)reporteTransaccionesOperativo.FechaFin).ToString("dd/MM/yyyy hh:mm tt"));
            paragraph.AddLineBreak();
            paragraph.AddText(reporteTransaccionesOperativo.TransaccionesOperativoDetalle.Count.ToString());

            // Add the data separation field
            paragraph = section.AddParagraph();
            paragraph.Format.Font.Size = 10;
            paragraph.Format.SpaceBefore = "1.25cm";
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
            column = table.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            // Create the header of the table
            row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Format.Font.Size = 8;
            row.Shading.Color = TableColor;
            row.Cells[0].AddParagraph("Tag");
            row.Cells[1].AddParagraph("Cuerpo");
            row.Cells[2].AddParagraph("Fecha");
            row.Cells[3].AddParagraph("Carril");
            row.Cells[4].AddParagraph("Clase Pre");
            row.Cells[5].AddParagraph("Clase Cajero");
            row.Cells[6].AddParagraph("Clase POST");
            row.Cells[7].AddParagraph("Medio de Pago");
            row.Cells[8].AddParagraph("Tarifa");

            FillGenericContent(reporteTransaccionesOperativo.TransaccionesOperativoDetalle);
        }

        void CrearReporteTransacciones(ReporteTransacciones? reporteTransacciones)
        {
            section.PageSetup.Orientation = Orientation.Landscape;
            section.PageSetup.BottomMargin = 20;
            section.PageSetup.TopMargin = 10;
            section.PageSetup.FooterDistance = 1;

            Column column;
            Row row;

            DataParameters(4);
            FechaGeneracion();

            // Put header in header frame
            Paragraph paragraph = headerFrame.AddParagraph("");//TRANSACCIONES FERROMEX DETALLE
            paragraph.Format.Font.Name = "Calibri";
            paragraph.Format.Font.Size = 13;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Put parameters in data Frame
            paragraph = dataParametersFrame.AddParagraph();
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Font.Size = 9;
            paragraph.AddText("Fecha Inicio:");
            paragraph.AddLineBreak();
            paragraph.AddText("Fecha Fin:");
            paragraph.AddLineBreak();
            paragraph.AddText("Total de Registros:");

            // Put values in data Frame
            paragraph = dataValuesFrame.AddParagraph();
            paragraph.Format.Font.Size = 9;
            paragraph.AddText(reporteTransacciones == null ? DateTime.MinValue.ToString("dd/MM/yyyy hh:mm tt") : ((DateTime)reporteTransacciones.FechaInicio).ToString("dd/MM/yyyy hh:mm tt"));
            paragraph.AddLineBreak();
            paragraph.AddText(reporteTransacciones == null ? DateTime.MinValue.ToString("dd/MM/yyyy hh:mm tt") : ((DateTime)reporteTransacciones.FechaFin).ToString("dd/MM/yyyy hh:mm tt"));
            paragraph.AddLineBreak();
            paragraph.AddText(reporteTransacciones.TransaccionesDetalle.Count.ToString());

            //// Add the data separation field
            //paragraph = section.AddParagraph();
            //paragraph.Format.SpaceBefore = "0.1cm";
            //paragraph.Format.Font.Size = 1;
            //paragraph.Style = "Reference";
            //paragraph.AddFormattedText("", TextFormat.Bold);


            // Create the item table
            table = section.AddTable();
            table.Style = "Table";
            table.Borders.Color = TableBorder;
            table.Borders.Width = 0.5;
            table.Rows.LeftIndent = 0;
            table.Rows.Alignment = RowAlignment.Center;

            // Before you can add a row, you must define the columns
            column = table.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            // Create the header of the table
            row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Format.Font.Size = 8;
            row.Shading.Color = TableColor;
            row.Cells[0].AddParagraph("Tag");
            row.Cells[1].AddParagraph("Placa");
            row.Cells[2].AddParagraph("NoEconomico");
            row.Cells[3].AddParagraph("Cuerpo");
            row.Cells[4].AddParagraph("Fecha");
            row.Cells[5].AddParagraph("Carril");
            row.Cells[6].AddParagraph("Clase Pre");
            row.Cells[7].AddParagraph("Clase Cajero");
            row.Cells[8].AddParagraph("Clase POST");
            row.Cells[9].AddParagraph("Medio de Pago");
            row.Cells[10].AddParagraph("Tarifa");
            row.Cells[11].AddParagraph("Tarifa Desc.");

            FillGenericContent(reporteTransacciones.TransaccionesDetalle);
        }

        void CrearReporteOperacionDetalle(ReporteOperacionesDetalle? reporteOperacionDetalle)
        {
            // definir orientacion
            section.PageSetup.Orientation = Orientation.Landscape;

            Column column;
            Row row;

            DataParameters(3);
            FechaGeneracion();

            // Put header in header frame
            Paragraph paragraph = headerFrame.AddParagraph("DIA CONCENTRADO");//TRANSACCIONES FERROMEX DETALLE
            paragraph.Format.Font.Name = "Calibri";
            paragraph.Format.Font.Size = 13;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Put parameters in data Frame
            paragraph = dataParametersFrame.AddParagraph();
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Font.Size = 9;
            paragraph.AddText("Fecha Inicio:");
            paragraph.AddLineBreak();
            paragraph.AddText("Fecha Fin:");
            paragraph.AddLineBreak();
            paragraph.AddText("Plaza:");

            // Put values in data Frame
            paragraph = dataValuesFrame.AddParagraph();
            paragraph.Format.Font.Size = 9;
            paragraph.AddText(reporteOperacionDetalle == null ? DateTime.MinValue.ToString("dd/MM/yyyy hh:mm tt") : ((DateTime)reporteOperacionDetalle.FechaInicio).ToString("dd/MM/yyyy hh:mm tt"));
            paragraph.AddLineBreak();
            paragraph.AddText(reporteOperacionDetalle == null ? DateTime.MinValue.ToString("dd/MM/yyyy hh:mm tt") : ((DateTime)reporteOperacionDetalle.FechaFin).ToString("dd/MM/yyyy hh:mm tt"));
            paragraph.AddLineBreak();
            paragraph.AddText(reporteOperacionDetalle?.Plaza ?? "");

            // Add the data separation field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "1.5cm";
            paragraph.Format.Font.Size = 2;
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("", TextFormat.Bold);

            // agregar un separador de campo
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "0.25cm";

            // Create the item table
            table = section.AddTable();
            table.Style = "Table";
            table.Borders.Color = TableBorder;
            table.Borders.Width = 0.5;
            table.Rows.LeftIndent = 0;
            table.Rows.Height = 10;

            // definir las columnas de la tabla
            column = table.AddColumn("1.75cm");
            column.Format.Alignment = ParagraphAlignment.Left;
            column = table.AddColumn("0.75cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("1.25cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("0.65cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("1.10cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("0.65cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("1.10cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("0.65cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("1.10cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("0.65cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("1.10cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("0.65cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("1.10cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("0.65cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("1.10cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("0.65cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("1.10cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("0.65cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("1.10cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("0.65cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("1.10cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("0.65cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("1.10cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("0.65cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("1.10cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("0.65cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("1.10cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            // agregar la fila de encabezados de la tabla
            row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Size = 7;
            row.Shading.Color = TableColor;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].AddParagraph("");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[0].Shading.Color = Colors.White;
            row.Cells[1].AddParagraph("Total");
            row.Cells[1].MergeRight = 1;
            row.Cells[3].AddParagraph("A Automóvil");
            row.Cells[3].MergeRight = 1;
            row.Cells[3].Format.Font.Size = 6.25;
            row.Cells[5].AddParagraph("M Moto");
            row.Cells[5].MergeRight = 1;
            row.Cells[7].AddParagraph("C2 Camion");
            row.Cells[7].MergeRight = 1;
            row.Cells[9].AddParagraph("C3 Camion");
            row.Cells[9].MergeRight = 1;
            row.Cells[11].AddParagraph("C4 Camion");
            row.Cells[11].MergeRight = 1;
            row.Cells[13].AddParagraph("C5 Camion");
            row.Cells[13].MergeRight = 1;
            row.Cells[15].AddParagraph("C6 Camion");
            row.Cells[15].MergeRight = 1;
            row.Cells[17].AddParagraph("C7 Camion");
            row.Cells[17].MergeRight = 1;
            row.Cells[19].AddParagraph("C8 Camion");
            row.Cells[19].MergeRight = 1;
            row.Cells[21].AddParagraph("C9 Camion");
            row.Cells[21].MergeRight = 1;
            row.Cells[23].AddParagraph("B2 Autobus");
            row.Cells[23].MergeRight = 1;
            row.Cells[25].AddParagraph("B3 Autobus");
            row.Cells[25].MergeRight = 1;

            // agregar la fila de encabezados secundarios de la tabla
            row = table.AddRow();
            row.Format.Font.Size = 7;
            row.Cells[0].AddParagraph("Medios de pago");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[0].Format.Font.Size = 6.5;
            row.VerticalAlignment = VerticalAlignment.Center;
            row.Cells[1].AddParagraph("Cant.");
            row.Cells[2].AddParagraph("Ingreso");
            row.Cells[3].AddParagraph("Cant.");
            row.Cells[4].AddParagraph("Ingreso");
            row.Cells[5].AddParagraph("Cant.");
            row.Cells[6].AddParagraph("Ingreso");
            row.Cells[7].AddParagraph("Cant.");
            row.Cells[8].AddParagraph("Ingreso");
            row.Cells[9].AddParagraph("Cant.");
            row.Cells[10].AddParagraph("Ingreso");
            row.Cells[11].AddParagraph("Cant.");
            row.Cells[12].AddParagraph("Ingreso");
            row.Cells[13].AddParagraph("Cant.");
            row.Cells[14].AddParagraph("Ingreso");
            row.Cells[15].AddParagraph("Cant.");
            row.Cells[16].AddParagraph("Ingreso");
            row.Cells[17].AddParagraph("Cant.");
            row.Cells[18].AddParagraph("Ingreso");
            row.Cells[19].AddParagraph("Cant.");
            row.Cells[20].AddParagraph("Ingreso");
            row.Cells[21].AddParagraph("Cant.");
            row.Cells[22].AddParagraph("Ingreso");
            row.Cells[23].AddParagraph("Cant.");
            row.Cells[24].AddParagraph("Ingreso");
            row.Cells[25].AddParagraph("Cant.");
            row.Cells[26].AddParagraph("Ingreso");

            table.SetEdge(0, 0, 1, 1, Edge.Top, BorderStyle.Single, 0.5, Colors.White);
            table.SetEdge(0, 0, 1, 1, Edge.Left, BorderStyle.Single, 0.5, Colors.White);

            FillGenericContent(reporteOperacionDetalle.OperacionesDetalle, 5);
        }

        void CrearReporteActividadUsuarios(ReporteActividadUsuarios? reporteActividadUsuarios)
        {
            section.PageSetup.Orientation = Orientation.Landscape;

            DataParameters(3);
            FechaGeneracion();

            // Put header in header frame
            Paragraph paragraph = headerFrame.AddParagraph("REPORTE DE ACTIVIDAD DE USUARIO");//MANTENIMIENTO TAGS
            paragraph.Format.Font.Name = "Calibri";
            paragraph.Format.Font.Size = 13;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Put parameters in data Frame
            paragraph = dataParametersFrame.AddParagraph();
            paragraph.Format.Font.Bold = true;
            paragraph.AddText("Generó reporte:");
            paragraph.AddLineBreak();
            if (reporteActividadUsuarios.FechaInicio != DateTime.MinValue)
            {
                paragraph.AddText("Fecha de Inicio:");
                paragraph.AddLineBreak();
            }
            if (reporteActividadUsuarios.FechaFin != DateTime.MaxValue)
            {
                paragraph.AddText("Fecha de Fin:");
                paragraph.AddLineBreak();
            }
            paragraph.AddText("Registros");

            // Put values in data Frame
            paragraph = dataValuesFrame.AddParagraph();
            paragraph.AddText(reporteActividadUsuarios?.Usuario ?? "");
            paragraph.AddLineBreak();
            if (reporteActividadUsuarios.FechaInicio != DateTime.MinValue)
            {
                paragraph.AddText(reporteActividadUsuarios == null ? DateTime.MinValue.ToString("dd/MM/yyyy hh:mm tt") : ((DateTime)reporteActividadUsuarios.FechaInicio).ToString("dd/MM/yyyy hh:mm tt"));
                paragraph.AddLineBreak();
            }
            if (reporteActividadUsuarios.FechaFin != DateTime.MaxValue)
            {
                paragraph.AddText(reporteActividadUsuarios == null ? DateTime.MinValue.ToString("dd/MM/yyyy hh:mm tt") : ((DateTime)reporteActividadUsuarios.FechaFin).ToString("dd/MM/yyyy hh:mm tt"));
                paragraph.AddLineBreak();
            }
            paragraph.AddText(reporteActividadUsuarios?.ActividadUsuario?.Count().ToString() ?? "");
            // Add the data separation field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "2.0cm";
            paragraph.Format.Font.Size = 8;
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

            column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("4cm");
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
            row.Cells[1].AddParagraph("Perfil");
            row.Cells[2].AddParagraph("Fecha");
            row.Cells[3].AddParagraph("Modulo");
            row.Cells[4].AddParagraph("Accíon");
            row.Cells[5].AddParagraph("Registro Original");
            row.Cells[6].AddParagraph("Registro Editado");


            FillGenericContent(reporteActividadUsuarios.ActividadUsuario);

        }

        void FechaGeneracion()
        {
            Paragraph paragraph = headerFrame.AddParagraph();
            paragraph.Format.Font.Name = "Calibri";
            paragraph.Format.Font.Size = 13;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Right;

            paragraph = dataParametersFrameFecha.AddParagraph();
            paragraph.Format.Font.Bold = true;
            paragraph.AddFormattedText();
            paragraph.Format.Font.Size = 10;
            paragraph.AddText("Fecha de Generación Reporte:");
            paragraph.AddLineBreak();

            paragraph = dataValuesFrameFecha.AddParagraph();
            paragraph.Format.Font.Size = 10;
            paragraph.AddText(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
            paragraph.AddLineBreak();

            paragraph = section.AddParagraph();
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("", TextFormat.Bold);
        }

        void DataParameters(int tipo)
        {
            switch (tipo)
            {
                case 1:
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
                    dataValuesFrame.Left = "5.5cm";
                    dataValuesFrame.RelativeHorizontal = RelativeHorizontal.Page;
                    dataValuesFrame.Top = "4.0cm";
                    dataValuesFrame.RelativeVertical = RelativeVertical.Page;

                    // Create the text frame for the data parameters
                    dataParametersFrameFecha = section.AddTextFrame();
                    dataParametersFrameFecha.Height = "0.5cm";
                    dataParametersFrameFecha.Width = "7.0cm";
                    dataParametersFrameFecha.Left = "7.90cm";
                    dataParametersFrameFecha.RelativeHorizontal = RelativeHorizontal.Margin;
                    dataParametersFrameFecha.Top = "4.0cm";
                    dataParametersFrameFecha.RelativeVertical = RelativeVertical.Page;

                    dataValuesFrameFecha = section.AddTextFrame();
                    dataValuesFrameFecha.Width = "7.0cm";
                    dataValuesFrameFecha.Left = "15.2cm";
                    dataValuesFrameFecha.RelativeHorizontal = RelativeHorizontal.Page;
                    dataValuesFrameFecha.Top = "4.0cm";
                    dataValuesFrameFecha.RelativeVertical = RelativeVertical.Page;
                    break;
                case 2:
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

                    dataParametersFrameFecha = section.AddTextFrame();
                    dataParametersFrameFecha.Height = "0.5cm";
                    dataParametersFrameFecha.Width = "7.0cm";
                    dataParametersFrameFecha.Left = "16.5cm";
                    dataParametersFrameFecha.RelativeHorizontal = RelativeHorizontal.Margin;
                    dataParametersFrameFecha.Top = "3.5cm";
                    dataParametersFrameFecha.RelativeVertical = RelativeVertical.Page;

                    dataValuesFrameFecha = section.AddTextFrame();
                    dataValuesFrameFecha.Width = "7.0cm";
                    dataValuesFrameFecha.Left = "23.90cm";
                    dataValuesFrameFecha.RelativeHorizontal = RelativeHorizontal.Page;
                    dataValuesFrameFecha.Top = "3.5cm";
                    dataValuesFrameFecha.RelativeVertical = RelativeVertical.Page;
                    break;
                case 3:
                    // Create the text frame for the header
                    headerFrame = section.AddTextFrame();
                    headerFrame.Width = "7.0cm";
                    headerFrame.Left = ShapePosition.Center;
                    headerFrame.RelativeHorizontal = RelativeHorizontal.Page;
                    headerFrame.Top = "2.30cm";
                    headerFrame.RelativeVertical = RelativeVertical.Page;

                    // Create the text frame for the data parameters
                    dataParametersFrame = section.AddTextFrame();
                    dataParametersFrame.Height = "3.0cm";
                    dataParametersFrame.Width = "7.0cm";
                    dataParametersFrame.Left = ShapePosition.Left;
                    dataParametersFrame.RelativeHorizontal = RelativeHorizontal.Margin;
                    dataParametersFrame.Top = "3.70cm";
                    dataParametersFrame.RelativeVertical = RelativeVertical.Page;

                    // Create the text frame for the data values
                    dataValuesFrame = section.AddTextFrame();
                    dataValuesFrame.Width = "7.0cm";
                    dataValuesFrame.Left = "8.0cm";
                    dataValuesFrame.RelativeHorizontal = RelativeHorizontal.Page;
                    dataValuesFrame.Top = "3.70cm";
                    dataValuesFrame.RelativeVertical = RelativeVertical.Page;

                    dataParametersFrameFecha = section.AddTextFrame();
                    dataParametersFrameFecha.Height = "3.0cm";
                    dataParametersFrameFecha.Width = "7.0cm";
                    dataParametersFrameFecha.Left = "16.60cm";
                    dataParametersFrameFecha.RelativeHorizontal = RelativeHorizontal.Margin;
                    dataParametersFrameFecha.Top = "3.70cm";
                    dataParametersFrameFecha.RelativeVertical = RelativeVertical.Page;

                    dataValuesFrameFecha = section.AddTextFrame();
                    dataValuesFrameFecha.Width = "7.0cm";
                    dataValuesFrameFecha.Left = "23.8cm";
                    dataValuesFrameFecha.RelativeHorizontal = RelativeHorizontal.Page;
                    dataValuesFrameFecha.Top = "3.70cm";
                    dataValuesFrameFecha.RelativeVertical = RelativeVertical.Page;
                    break;
                case 4:
                    // Create the text frame for the header
                    headerFrame = section.AddTextFrame();
                    headerFrame.Width = "7.0cm";
                    headerFrame.Left = ShapePosition.Center;
                    headerFrame.RelativeHorizontal = RelativeHorizontal.Page;
                    headerFrame.Top = "0.10cm";
                    headerFrame.RelativeVertical = RelativeVertical.Page;

                    // Create the text frame for the data parameters
                    dataParametersFrame = section.AddTextFrame();
                    dataParametersFrame.Height = "2.0cm";
                    dataParametersFrame.Width = "7.0cm";
                    dataParametersFrame.Left = ShapePosition.Left;
                    dataParametersFrame.RelativeHorizontal = RelativeHorizontal.Margin;
                    dataParametersFrame.Top = "0.10cm";
                    dataParametersFrame.RelativeVertical = RelativeVertical.Page;

                    // Create the text frame for the data values
                    dataValuesFrame = section.AddTextFrame();
                    dataValuesFrame.Width = "7.0cm";
                    dataValuesFrame.Left = "8.0cm";
                    dataValuesFrame.RelativeHorizontal = RelativeHorizontal.Page;
                    dataValuesFrame.Top = "0.10cm";
                    dataValuesFrame.RelativeVertical = RelativeVertical.Page;

                    dataParametersFrameFecha = section.AddTextFrame();
                    dataParametersFrameFecha.Height = "2.0cm";
                    dataParametersFrameFecha.Width = "7.0cm";
                    dataParametersFrameFecha.Left = "10.60cm";
                    dataParametersFrameFecha.RelativeHorizontal = RelativeHorizontal.Margin;
                    dataParametersFrameFecha.Top = "0.10cm";
                    dataParametersFrameFecha.RelativeVertical = RelativeVertical.Page;

                    dataValuesFrameFecha = section.AddTextFrame();
                    dataValuesFrameFecha.Width = "7.0cm";
                    dataValuesFrameFecha.Left = "17.8cm";
                    dataValuesFrameFecha.RelativeHorizontal = RelativeHorizontal.Page;
                    dataValuesFrameFecha.Top = "0.10cm";
                    dataValuesFrameFecha.RelativeVertical = RelativeVertical.Page;
                    break;
                default:
                    break;
            }

        }

        void FillGenericContent<T>(List<T> value, int fontSize = 6)
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