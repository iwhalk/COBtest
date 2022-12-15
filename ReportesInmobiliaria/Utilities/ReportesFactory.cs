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
using MimeKit.IO.Filters;

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
        private string _apartmentNumber;
        private string _title = "";
        private bool _firstActivity = true;
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

        public byte[] CrearPdf<T>(T reporte, string title)
        {
            // Create a new MigraDoc document
            document = new Document();
            pageSetup = document.DefaultPageSetup;
            //document.Info.Title = "";
            //document.Info.Subject = "";
            //document.Info.Author = "";
            section = document.AddSection();
            _title = title;

            DefineStyles();
            CreateLayout(reporte);
            CrearReporteDetalle(reporte as ReporteDetalles);
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
            style.Font.Size = 12;

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

            //Control de NullReferenceException al llamar a las imágenes
            if (ImageSource.ImageSourceImpl == null)
            {
                ImageSource.ImageSourceImpl = new ImageSharpImageSource<Rgba32>();
            }

            WebClient client = new WebClient();
            MemoryStream stream = new MemoryStream(client.DownloadData("https://aeriblobs.blob.core.windows.net/inventoryblobs/282039e4-b91f-4c4a-8356-eb359c9c4ece.jpeg"));
            stream.Position = 0;
            var logoSOF2245 = section.AddImage(ImageSource.FromStream("logoSOF", () => stream));
            logoSOF2245.Width = "4.5cm";
            logoSOF2245.LockAspectRatio = true;
            logoSOF2245.RelativeHorizontal = RelativeHorizontal.Margin;
            logoSOF2245.RelativeVertical = RelativeVertical.Page;
            logoSOF2245.Top = "1.7cm";
            logoSOF2245.Left = "-1.3cm";
            logoSOF2245.WrapFormat.Style = WrapStyle.Through;

            MemoryStream stream1 = new MemoryStream(client.DownloadData("https://aeriblobs.blob.core.windows.net/inventoryblobs/63450fd9-9af2-4c14-88ec-0752b1b6f1ae.jpeg"));
            stream1.Position = 0;
            var logoGeneric = section.AddImage(ImageSource.FromStream("logoGEN", () => stream1));
            logoGeneric.Width = "3.5cm";
            logoGeneric.LockAspectRatio = true;
            logoGeneric.RelativeHorizontal = RelativeHorizontal.Margin;
            logoGeneric.RelativeVertical = RelativeVertical.Page;
            logoGeneric.Top = "1.7cm";
            logoGeneric.Left = "14.0cm";
            logoGeneric.WrapFormat.Style = WrapStyle.Through;
            Paragraph paragraph;
            if (_title == "")
            {
                // Put header in header frame
                paragraph = headerFrame.AddParagraph("Reporte Detallado");//Titulo
                paragraph.AddLineBreak();
                paragraph.Format.Font.Name = "Times New Roman";
                paragraph.Format.Font.Size = 20;
                paragraph.Format.Font.Bold = true;
                paragraph.Format.Alignment = ParagraphAlignment.Center;
            }
            else
            {
                paragraph = headerFrame.AddParagraph("Reporte Detallado Por Actividad");//Titulo
                paragraph.AddLineBreak();
                paragraph.AddText(_title);
                paragraph.Format.Font.Name = "Times New Roman";
                paragraph.Format.Font.Size = 18;
                paragraph.Format.Font.Bold = true;
                paragraph.Format.Alignment = ParagraphAlignment.Center;

                paragraph = section.AddParagraph();
                paragraph.AddLineBreak();
                paragraph.Format.SpaceBefore = "0.6cm";
            }

            // Put parameters in data Frame
            paragraph = dataParametersFrameRight.AddParagraph();
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Font.Size = 10;
            paragraph.AddText("Fecha de creación: ");

            // Put values in data Frame
            paragraph = dataValuesFrameRight.AddParagraph();
            paragraph.AddText(DateTime.Now.ToString("dd/MM/yyyy"));
            reporteDetalles.detalladoActividades = reporteDetalles.detalladoActividades.OrderBy(x => x.numeroApartamento).ToList();

            for (int i = 0; i < reporteDetalles.detalladoActividades.Count; i++)
            {
                string apartmentTitle = reporteDetalles.detalladoActividades.ElementAt(i).numeroApartamento;
                // Create the item table
                paragraph = section.AddParagraph();
                paragraph.AddLineBreak();
                paragraph.Format.SpaceBefore = "0.8cm";
                paragraph.AddText("Departamento " + apartmentTitle);
                paragraph.Format.Font.Name = "Times New Roman";
                paragraph.Format.Font.Size = 16;
                if (_title == "")
                {
                    paragraph.Format.Font.Bold = true;
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                }
                else
                    paragraph.Format.Alignment = ParagraphAlignment.Left;

                paragraph = section.AddParagraph();
                paragraph.AddLineBreak();
                paragraph.Format.SpaceBefore = "0.8cm";

                tableAreas = section.AddTable();
                tableAreas.Style = "Table";
                tableAreas.Borders.Color = Colors.Gray;
                tableAreas.Borders.Width = 0.3;
                tableAreas.Rows.LeftIndent = 0;
                tableAreas.Rows.Alignment = RowAlignment.Center;
                // Before you can add a row, you must define the columns
                Column column = tableAreas.AddColumn("4.0cm");
                column.Format.Alignment = ParagraphAlignment.Center;
                column = tableAreas.AddColumn("4.6cm");
                column.Format.Alignment = ParagraphAlignment.Center;
                column = tableAreas.AddColumn("3.8cm");
                column.Format.Alignment = ParagraphAlignment.Center;
                column = tableAreas.AddColumn("2.0cm");
                column.Format.Alignment = ParagraphAlignment.Center;
                column = tableAreas.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;
                column = tableAreas.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;
                // Create the header of the table
                Row row = tableAreas.AddRow();
                row.HeadingFormat = true;
                row.Format.Alignment = ParagraphAlignment.Center;
                //row.Format.Font.Bold = true;
                row.Format.Font.Size = 14;
                row.Borders.Visible = false;
                //row.Shading.Color = TableColor;
                row.Cells[0].AddParagraph("Actividad");
                row.Cells[1].AddParagraph("Elemento");
                row.Cells[2].AddParagraph("Sub-Elemento");
                row.Cells[3].AddParagraph("Estatus");
                row.Cells[4].AddParagraph("Total");
                row.Cells[5].AddParagraph("Avance");

                if(_title == "")
                    i = FillGenericContent(reporteDetalles.detalladoActividades, tableAreas, i, apartmentTitle) - 1;
                else
                    i = FillGenericContentCombination(reporteDetalles.detalladoActividades, tableAreas, i, apartmentTitle) - 1;
            }
        }

        void CrearReporteAvance(ReporteAvance? reporteAvance)
        {
            section.PageSetup.Orientation = Orientation.Portrait;

            headerFrame = section.AddTextFrame();
            headerFrame.Width = "22.5cm";
            headerFrame.Left = "15cm";
            headerFrame.RelativeHorizontal = RelativeHorizontal.Page;
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

            //Control de NullReferenceException al llamar a las imágenes
            if (ImageSource.ImageSourceImpl == null)
            {
                ImageSource.ImageSourceImpl = new ImageSharpImageSource<Rgba32>();
            }

            WebClient client = new WebClient();
            MemoryStream stream = new MemoryStream(client.DownloadData("https://aeriblobs.blob.core.windows.net/inventoryblobs/282039e4-b91f-4c4a-8356-eb359c9c4ece.jpeg"));
            stream.Position = 0;
            var logoSOF2245 = section.AddImage(ImageSource.FromStream("logoSOF", () => stream));
            logoSOF2245.Width = "4.5cm";
            logoSOF2245.LockAspectRatio = true;
            logoSOF2245.RelativeHorizontal = RelativeHorizontal.Margin;
            logoSOF2245.RelativeVertical = RelativeVertical.Page;
            logoSOF2245.Top = "1.7cm";
            logoSOF2245.Left = "-1.3cm";
            logoSOF2245.WrapFormat.Style = WrapStyle.Through;

            MemoryStream stream1 = new MemoryStream(client.DownloadData("https://aeriblobs.blob.core.windows.net/inventoryblobs/63450fd9-9af2-4c14-88ec-0752b1b6f1ae.jpeg"));
            stream1.Position = 0;
            var logoGeneric = section.AddImage(ImageSource.FromStream("logoGEN", () => stream1));
            logoGeneric.Width = "3.5cm";
            logoGeneric.LockAspectRatio = true;
            logoGeneric.RelativeHorizontal = RelativeHorizontal.Margin;
            logoGeneric.RelativeVertical = RelativeVertical.Page;
            logoGeneric.Top = "1.7cm";
            logoGeneric.Left = "14.0cm";
            logoGeneric.WrapFormat.Style = WrapStyle.Through;


            // Put header in header frame
            Paragraph paragraph = headerFrame.AddParagraph("Resumen de Avance General Por Departamento");//Titulo
            paragraph.AddLineBreak();
            paragraph.Format.Font.Name = "Times New Roman";
            paragraph.Format.Font.Size = 14;
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

            Table table = section.AddTable();
            table.Rows.Alignment = RowAlignment.Center;

            Column columna = table.AddColumn("3cm");
            columna.Format.Alignment = ParagraphAlignment.Center;
            columna = table.AddColumn("14cm");
            columna.Format.Alignment = ParagraphAlignment.Center;

            Row rowo = table.AddRow();
            rowo.HeadingFormat = true;
            rowo.Format.Alignment = ParagraphAlignment.Center;
            rowo.Format.Font.Bold = true;
            rowo.Format.Font.Size = 14;
            rowo.Borders.Visible = false;
            rowo.BottomPadding = "1cm";
            rowo.Cells[0].AddParagraph("Departamento");
            rowo.Cells[1].AddParagraph("Avance General");
          
            FillChartContent(reporteAvance.Apartments, table);
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
            
        }

        void FillChartContent<T>(List<T> value, Table table, int fontSize = 12)
        {
            var chart = new MigraDocCore.DocumentObjectModel.Shapes.Charts.Chart(MigraDocCore.DocumentObjectModel.Shapes.Charts.ChartType.Bar2D);
            chart.Width = "15cm";
            chart.Height = "1cm";


            chart.XAxis.MajorTickMark = MigraDocCore.DocumentObjectModel.Shapes.Charts.TickMarkType.Outside;
            chart.XAxis.Title.Caption = "";
            chart.XAxis.HasMajorGridlines = true;

            chart.YAxis.TickLabels.Format = " ";
            chart.YAxis.MajorTickMark = MigraDocCore.DocumentObjectModel.Shapes.Charts.TickMarkType.Cross;
            chart.YAxis.MinimumScale = 0;
            chart.YAxis.MaximumScale = 1;

            chart.PlotArea.LineFormat.Color = Colors.OrangeRed;
            chart.PlotArea.LineFormat.Width = 2;
            chart.PlotArea.LineFormat.Visible = false;
            chart.PlotArea.FillFormat.Color = Colors.OrangeRed;            

            foreach (var item in value)
            {
                Row row = table.AddRow();
                row.Format.Font.Size = (Unit)fontSize;
                row.VerticalAlignment = VerticalAlignment.Center;

                if (item != null)
                    foreach (var (prop, index) in item.GetType().GetProperties().Select((v, i) => (v, i)))
                    {
                        var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                        if (type == typeof(Double))
                        {
                            var clone_chart = chart.Clone();
                            var series = clone_chart.SeriesCollection.AddSeries();
                            series.Add((Double)prop.GetValue(item, null) / 100.0000 );
                            series.DataLabel.Format = "#0.00%";
                            var asdasda = (Double)prop.GetValue(item, null);
                            if ((Double)prop.GetValue(item, null) < 90 && (Double)prop.GetValue(item, null) > 0)
                                series.DataLabel.Position = MigraDocCore.DocumentObjectModel.Shapes.Charts.DataLabelPosition.OutsideEnd;
                            else
                                series.DataLabel.Position = MigraDocCore.DocumentObjectModel.Shapes.Charts.DataLabelPosition.InsideEnd;
                            series.DataLabel.Font.Color = Colors.White;
                            var elements = series.Elements.Cast<MigraDocCore.DocumentObjectModel.Shapes.Charts.Point>().ToArray();
                          
                            elements[0].FillFormat.Color = Colors.MediumSeaGreen;
                            elements[0].LineFormat.Color = Colors.MediumSeaGreen;
                            elements[0].LineFormat.Width= 3;
                            var xseries = clone_chart.XValues.AddXSeries();
                            xseries.Add("");
                            row.Cells[index].Add(clone_chart);
                            row.Cells[index].Row.TopPadding = "0.5cm";
                        }
                        if (type == typeof(string))
                        {
                            row.Cells[index].AddParagraph(prop.GetValue(item, null)?.ToString());
                            row.Cells[index].Row.VerticalAlignment = VerticalAlignment.Top;
                        }
                        if (type == typeof(bool))
                        {
                            row.Cells[index].AddParagraph((bool?)prop.GetValue(item, null) ?? false ? "SI" : "NO");
                        }
                    }
            }
        }

        int FillGenericContent<T>(List<T> value, Table table, int tableIndex, string title, int fontSize = 10)
        {
            Table _table = table;
            //foreach (var item in value)
            string currentName = "";
            string beforeName = "";
            Row lastRow = null;
            int countCombination = 0;
            int indexCombination = 0;
            var newColorGray = MigraDocCore.DocumentObjectModel.Color.Parse("0xffE5E8E8");
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
                        if (index == 0)
                        { 
                            string currentApartmentName = prop.GetValue(item, null)?.ToString();
                            //Si emepiezan los datos de otro departamento, elimina la fila extra, agrega el borde inferior a la última celda y regresa
                            if (currentApartmentName != title)
                            {
                                table.Rows.RemoveObjectAt(table.Rows.Count - 1);
                                if(lastRow != null)
                                    lastRow.Cells[0].Borders.Bottom.Width = 1.5;
                                return i;
                            }
                        }
                        else
                        {
                            if (index == 1)
                            {
                                currentName = prop.GetValue(item, null)?.ToString();
                                //countCombination++;
                            }
                            //else
                            //{
                            if (type == typeof(DateTime))
                                {
                                    row.Cells[index - 1].AddParagraph(((DateTime?)prop.GetValue(item, null))?.ToString("dd/MM/yyyy hh:mm:ss tt") ?? "");
                                }
                                if (type == typeof(string))
                                {
                                    row.Cells[index - 1].AddParagraph(prop.GetValue(item, null)?.ToString());
                                }
                                if (type == typeof(bool))
                                {
                                    row.Cells[index - 1].AddParagraph((bool?)prop.GetValue(item, null) ?? false ? "SI" : "NO");
                                }
                                if (type == typeof(int))
                                {
                                    row.Cells[index - 1].AddParagraph(prop.GetValue(item, null)?.ToString());
                                }
                                if (type == typeof(long))
                                {
                                    row.Cells[index - 1].AddParagraph(prop.GetValue(item, null)?.ToString());
                                }
                            //}

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
                                {
                                    row.Cells[0].Borders.Top.Width = 1.5;
                                    //Row rowData = _table.Rows[indexCombination + 1];
                                    //rowData.Cells[0].AddParagraph(beforeName);
                                    //rowData.Cells[0].MergeDown = countCombination - 2;
                                    //rowData.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                                    //indexCombination += countCombination;
                                    countCombination = 0;
                                }
                            }

                            beforeName = currentName;
                        }
                    }
                lastRow = row;
                if (i == value.Count - 1)
                {
                    //Row rowData;
                    row.Cells[0].Borders.Bottom.Width = 1.5;
                    //if(indexCombination == 0)
                    //    rowData = _table.Rows[indexCombination + 1];
                    //else
                    //    rowData = _table.Rows[indexCombination];
                    //rowData.Cells[0].AddParagraph(beforeName);
                    //rowData.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                }
                    
                if (i % 2 == 0)
                {                    
                    //row.Shading.Color= Colors.LightGray;
                    row.Cells[1].Shading.Color = newColorGray;
                    row.Cells[2].Shading.Color = newColorGray;
                    row.Cells[3].Shading.Color = newColorGray;
                    row.Cells[4].Shading.Color = newColorGray;
                    row.Cells[5].Shading.Color = newColorGray;
                }
            }
            return value.Count;
        }

        int FillGenericContentCombination<T>(List<T> value, Table table, int tableIndex, string title, int fontSize = 10)
        {
            Table _table = table;
            //foreach (var item in value)
            string currentName = "";
            string beforeName = "";
            Row lastRow = null;
            int countCombination = 0;
            int indexCombination = 0;
            var newColorGray = MigraDocCore.DocumentObjectModel.Color.Parse("0xffE5E8E8");
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
                        if (index == 0)
                        {
                            string currentApartmentName = prop.GetValue(item, null)?.ToString();
                            //Si empiezan los datos de otro departamento, elimina la fila extra, agrega el borde inferior a la última celda y regresa
                            if (currentApartmentName != title)
                            {
                                table.Rows.RemoveObjectAt(table.Rows.Count - 1);
                                if (lastRow != null)
                                    lastRow.Cells[0].Borders.Bottom.Width = 1.5;
                                //Agrega el último nombre de actividad a la tabla, combina las celdas restantes
                                Row rowData;
                                if (indexCombination == 0)
                                    rowData = _table.Rows[indexCombination + 1];
                                else
                                    rowData = _table.Rows[indexCombination];
                                rowData.Cells[0].AddParagraph(beforeName);
                                if (countCombination < 45)
                                    rowData.Cells[0].MergeDown = countCombination - 1;
                                rowData.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                                return i;
                            }
                        }
                        else
                        {
                            //No coloca el nombre de la actividad hasta que haya cambiado
                            if (index == 1)
                            {
                                currentName = prop.GetValue(item, null)?.ToString();
                                countCombination++;
                            }
                            else
                            {
                                if (type == typeof(DateTime))
                                {
                                    row.Cells[index - 1].AddParagraph(((DateTime?)prop.GetValue(item, null))?.ToString("dd/MM/yyyy hh:mm:ss tt") ?? "");
                                }
                                if (type == typeof(string))
                                {
                                    row.Cells[index - 1].AddParagraph(prop.GetValue(item, null)?.ToString());
                                }
                                if (type == typeof(bool))
                                {
                                    row.Cells[index - 1].AddParagraph((bool?)prop.GetValue(item, null) ?? false ? "SI" : "NO");
                                }
                                if (type == typeof(int))
                                {
                                    row.Cells[index - 1].AddParagraph(prop.GetValue(item, null)?.ToString());
                                }
                                if (type == typeof(long))
                                {
                                    row.Cells[index - 1].AddParagraph(prop.GetValue(item, null)?.ToString());
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
                                {
                                    //Coloca el nombre de la actividad y combina las celdas de su respectivo conjunto
                                    Row rowData;
                                    row.Cells[0].Borders.Top.Width = 1.5;
                                    if (!_firstActivity)
                                    {
                                        rowData = _table.Rows[indexCombination];
                                        rowData.Cells[0].AddParagraph(beforeName);
                                        if (countCombination < 45)
                                            rowData.Cells[0].MergeDown = countCombination - 1;
                                    }

                                    else
                                    {
                                        rowData = _table.Rows[indexCombination + 1];
                                        _firstActivity = false;
                                        rowData.Cells[0].AddParagraph(beforeName);
                                        if (countCombination < 45)
                                            rowData.Cells[0].MergeDown = countCombination - 2;
                                    }
                                    rowData.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                                    indexCombination += countCombination;
                                    countCombination = 0;
                                }
                            }

                            beforeName = currentName;
                        }
                    }
                lastRow = row;
                if (i == value.Count - 1)
                {
                    Row rowData;
                    row.Cells[0].Borders.Bottom.Width = 1.5;
                    //Agrega el último nombre de actividad a la tabla, combina las celdas restantes
                    if (indexCombination == 0)
                        rowData = _table.Rows[indexCombination + 1];
                    else
                        rowData = _table.Rows[indexCombination];
                    rowData.Cells[0].AddParagraph(beforeName);
                    if(countCombination < 45)
                        rowData.Cells[0].MergeDown = countCombination - 1;
                    rowData.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                }
                //Cambia el color de relleno de las celdas cuya fila sea par
                if (i % 2 == 0)
                {
                    row.Cells[1].Shading.Color = newColorGray;
                    row.Cells[2].Shading.Color = newColorGray;
                    row.Cells[3].Shading.Color = newColorGray;
                    row.Cells[4].Shading.Color = newColorGray;
                    row.Cells[5].Shading.Color = newColorGray;
                }
            }
            return value.Count;
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