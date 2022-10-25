using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PdfSharp.Pdf;
using PdfSharp.Pdf.AcroForms;
using PdfSharp.Pdf.IO;
using Shared.Models;

namespace ReportesInmobiliaria.Utilities
{
    public class PdfFactory
    {
        public Stream GenerarReporteCajeroReceptor(PreliquidacionCajeroReceptor preliquidacionCajeroReceptor, ComparativoCajeroReceptor comparativoCajeroReceptor)
        {
            MemoryStream stream = new();

            GenerarComparativoCajeroReceptor(comparativoCajeroReceptor).Save(stream);
            using (PdfDocument? reportePdf = GenerarPreliquidacionCajeroReceptor(preliquidacionCajeroReceptor))
            {
                reportePdf.AddPage((PdfPage?)PdfReader.Open(stream, PdfDocumentOpenMode.Import).Pages[0]);
                reportePdf.Save(stream);
                reportePdf.Close();
            }
            return stream;
        }

        public Stream GenerarReporteTurnoCarriles(PreliquidacionTurnoCarriles preliquidacionTurnoCarriles, ComparativoTurnoCarriles comparativoTurnoCarriles)
        {
            MemoryStream stream = new();

            GenerarComparativoTurnoCarriles(comparativoTurnoCarriles).Save(stream);
            using (PdfDocument? reportePdf = GenerarPreliquidacionTurnoCarriles(preliquidacionTurnoCarriles))
            {
                reportePdf.AddPage((PdfPage?)PdfReader.Open(stream, PdfDocumentOpenMode.Import).Pages[0]);
                reportePdf.Save(stream);
                reportePdf.Close();
            }

            return stream;
        }

        public Stream GenerarReporteDiaCaseta(PreliquidacionDiaCaseta preliquidacionDiaCaseta, ComparativoDiaCaseta comparativoDiaCaseta)
        {
            MemoryStream stream = new();

            GenerarComparativoDiaCaseta(comparativoDiaCaseta).Save(stream);
            using (PdfDocument? reportePdf = GenerarPreliquidacionDiaCaseta(preliquidacionDiaCaseta))
            {
                reportePdf.AddPage((PdfPage?)PdfReader.Open(stream, PdfDocumentOpenMode.Import).Pages[0]);
                reportePdf.Save(stream);
                reportePdf.Close();
            }

            return stream;
        }

        public PdfDocument GenerarPreliquidacionCajeroReceptor(PreliquidacionCajeroReceptor preliquidacionCajeroReceptor)
        {
            PdfDocument doc = PdfReader.Open(Path.Combine(Environment.CurrentDirectory, @"PdfTemplates\", "PreliquidacionCajeroReceptor.pdf"), PdfDocumentOpenMode.Modify);
            PdfAcroForm form = doc.AcroForm;

            if (form.Elements.ContainsKey("/NeedAppearances"))
            {
                form.Elements["/NeedAppearances"] = new PdfBoolean(true);
            }
            else
            {
                form.Elements.Add("/NeedAppearances", new PdfBoolean(true));
            }

            if (form.Fields["par3"] as PdfTextField != null) (form.Fields["par3"] as PdfTextField).Value = new PdfString(preliquidacionCajeroReceptor.Par3.FirstOrDefault()?.NomDelegacion ?? "");
            if (form.Fields["par64"] as PdfTextField != null) (form.Fields["par64"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["par66"] as PdfTextField != null) (form.Fields["par66"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["par68"] as PdfTextField != null) (form.Fields["par68"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["par77"] as PdfTextField != null) (form.Fields["par77"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["par82"] as PdfTextField != null) (form.Fields["par82"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["par92"] as PdfTextField != null) (form.Fields["par92"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["par95"] as PdfTextField != null) (form.Fields["par95"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["date"] as PdfTextField != null) (form.Fields["date"] as PdfTextField).Value = new PdfString(DateTime.Now.ToString("MM/dd/yyyy"));
            if (form.Fields["time"] as PdfTextField != null) (form.Fields["time"] as PdfTextField).Value = new PdfString(DateTime.Now.ToString("hh:mm:ss tt"));
            if (form.Fields["parelu1"] as PdfTextField != null) (form.Fields["parelu1"] as PdfTextField).Value = new PdfString(preliquidacionCajeroReceptor.Parelu1 ?? "");
            if (form.Fields["parelu2"] as PdfTextField != null) (form.Fields["parelu2"] as PdfTextField).Value = new PdfString(preliquidacionCajeroReceptor.Parelu2 ?? "");
            if (form.Fields["parelu3"] as PdfTextField != null) (form.Fields["parelu3"] as PdfTextField).Value = new PdfString(preliquidacionCajeroReceptor.Parelu3 ?? "");
            if (form.Fields["parelu4"] as PdfTextField != null) (form.Fields["parelu4"] as PdfTextField).Value = new PdfString(preliquidacionCajeroReceptor.Parelu4 ?? "");
            
            string[] stringArray = { "par3", "par64", "par66", "par68", "par77", "par82", "par92", "par95", "date", "time", "parelu1", "parelu2", "parelu3", "parelu4" };

            foreach (var fieldName in form.Fields.Names)
            {
                if (!stringArray.Any(s => s.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    var value = (string)(preliquidacionCajeroReceptor?.GetType().GetProperty(char.ToUpper(fieldName[0]) + fieldName[1..])?.GetValue(preliquidacionCajeroReceptor, null) ?? "");
                    if (form.Fields[fieldName] as PdfTextField != null) (form.Fields[fieldName] as PdfTextField).Value = new PdfString(value);
                }
                form.Fields[fieldName].Elements.SetName($"/T", $"{Guid.NewGuid()}");
            }

            doc.Flatten();
            //doc.Save(Path.Combine("C:/Users/martin/Desktop/PCR.pdf"));
            return doc;

        }
        public PdfDocument GenerarComparativoCajeroReceptor(ComparativoCajeroReceptor comparativoCajeroReceptor)
        {
            PdfDocument doc = PdfReader.Open(Path.Combine(Environment.CurrentDirectory, @"PdfTemplates\", "ComparativoCajeroReceptor.pdf"), PdfDocumentOpenMode.Modify);
            PdfAcroForm form = doc.AcroForm;

            if (form.Elements.ContainsKey("/NeedAppearances"))
            {
                form.Elements["/NeedAppearances"] = new PdfBoolean(true);
            }
            else
            {
                form.Elements.Add("/NeedAppearances", new PdfBoolean(true));
            }

            if(form.Fields["par1"] as PdfTextField != null) (form.Fields["par1"] as PdfTextField).Value = new PdfString(comparativoCajeroReceptor.Par1?.FirstOrDefault()?.NomDelegacion ?? "");
            if (form.Fields["par426"] as PdfTextField != null) (form.Fields["par426"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["date"] as PdfTextField != null) (form.Fields["date"] as PdfTextField).Value = new PdfString(DateTime.Now.ToString("MM/dd/yyyy"));

            string[] stringArray = { "par1", "par426", "date" };

            foreach (var fieldName in form.Fields.Names)
            {
                if (!stringArray.Any(s => s.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    var value = (string)(comparativoCajeroReceptor?.GetType().GetProperty(char.ToUpper(fieldName[0]) + fieldName[1..])?.GetValue(comparativoCajeroReceptor, null) ?? "");               
                    if (form.Fields[fieldName] as PdfTextField != null) (form.Fields[fieldName] as PdfTextField).Value = new PdfString(value);
                }
                form.Fields[fieldName].Elements.SetName($"/T", $"{Guid.NewGuid()}");
            }

            doc.Flatten();
            //doc.Save(Path.Combine("C:/Users/martin/Desktop/CCR.pdf"));
            return doc;
        }

        public PdfDocument GenerarPreliquidacionTurnoCarriles(PreliquidacionTurnoCarriles preliquidacionTurnoCarriles)
        {
            PdfDocument doc = PdfReader.Open(Path.Combine(Environment.CurrentDirectory, @"PdfTemplates\", "PreliquidacionTurnoCarriles.pdf"), PdfDocumentOpenMode.Modify);
            PdfAcroForm form = doc.AcroForm;

            if (form.Elements.ContainsKey("/NeedAppearances"))
            {
                form.Elements["/NeedAppearances"] = new PdfBoolean(true);
            }
            else
            {
                form.Elements.Add("/NeedAppearances", new PdfBoolean(true));
            }

            if (form.Fields["par64"] as PdfTextField != null) (form.Fields["par64"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["par66"] as PdfTextField != null) (form.Fields["par66"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["par68"] as PdfTextField != null) (form.Fields["par68"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["par95"] as PdfTextField != null) (form.Fields["par95"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["date"] as PdfTextField != null) (form.Fields["date"] as PdfTextField).Value = new PdfString(DateTime.Now.ToString("MM/dd/yyyy"));
            if (form.Fields["time"] as PdfTextField != null) (form.Fields["time"] as PdfTextField).Value = new PdfString(DateTime.Now.ToString("hh:mm:ss tt"));
            if (form.Fields["parelu1"] as PdfTextField != null) (form.Fields["parelu1"] as PdfTextField).Value = new PdfString(preliquidacionTurnoCarriles.Parelu1 ?? "");
            if (form.Fields["parelu2"] as PdfTextField != null) (form.Fields["parelu2"] as PdfTextField).Value = new PdfString(preliquidacionTurnoCarriles.Parelu2 ?? "");
            if (form.Fields["parelu3"] as PdfTextField != null) (form.Fields["parelu3"] as PdfTextField).Value = new PdfString(preliquidacionTurnoCarriles.Parelu3 ?? "");
            if (form.Fields["parelu4"] as PdfTextField != null) (form.Fields["parelu4"] as PdfTextField).Value = new PdfString(preliquidacionTurnoCarriles.Parelu4 ?? "");

            string[] stringArray = { "par64", "par66", "par68", "par95", "date", "time", "parelu1", "parelu2", "parelu3", "parelu4" };

            foreach (var fieldName in form.Fields.Names)
            {
                if (!stringArray.Any(s => s.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    var value = (string)(preliquidacionTurnoCarriles?.GetType().GetProperty(char.ToUpper(fieldName[0]) + fieldName[1..])?.GetValue(preliquidacionTurnoCarriles, null) ?? "");
                    if (form.Fields[fieldName] as PdfTextField != null) (form.Fields[fieldName] as PdfTextField).Value = new PdfString(value);
                }
                form.Fields[fieldName].Elements.SetName($"/T", $"{Guid.NewGuid()}");
            }

            doc.Flatten();
            //doc.Save(Path.Combine("C:/Users/martin/Desktop/PTC.pdf"));
            return doc;

        }
        public PdfDocument GenerarComparativoTurnoCarriles(ComparativoTurnoCarriles comparativoTurnoCarriles)
        {
            PdfDocument doc = PdfReader.Open(Path.Combine(Environment.CurrentDirectory, @"PdfTemplates\", "ComparativoTurnoCarriles.pdf"), PdfDocumentOpenMode.Modify);
            PdfAcroForm form = doc.AcroForm;

            if (form.Elements.ContainsKey("/NeedAppearances"))
            {
                form.Elements["/NeedAppearances"] = new PdfBoolean(true);
            }
            else
            {
                form.Elements.Add("/NeedAppearances", new PdfBoolean(true));
            }

            if (form.Fields["par426"] as PdfTextField != null) (form.Fields["par426"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["date"] as PdfTextField != null) (form.Fields["date"] as PdfTextField).Value = new PdfString(DateTime.Now.ToString("MM/dd/yyyy"));

            string[] stringArray = { "par426", "date" };

            foreach (var fieldName in form.Fields.Names)
            {
                if (!stringArray.Any(s => s.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    var value = (string)(comparativoTurnoCarriles?.GetType().GetProperty(char.ToUpper(fieldName[0]) + fieldName[1..])?.GetValue(comparativoTurnoCarriles, null) ?? "");
                    if (form.Fields[fieldName] as PdfTextField != null) (form.Fields[fieldName] as PdfTextField).Value = new PdfString(value);
                }
                form.Fields[fieldName].Elements.SetName($"/T", $"{Guid.NewGuid()}");
            }

            doc.Flatten();
            //doc.Save(Path.Combine("C:/Users/martin/Desktop/CTC.pdf"));
            return doc;
        }

        public PdfDocument GenerarPreliquidacionDiaCaseta(PreliquidacionDiaCaseta preliquidacionDiaCaseta)
        {
            PdfDocument doc = PdfReader.Open(Path.Combine(Environment.CurrentDirectory, @"PdfTemplates\", "PreliquidacionDiaCaseta.pdf"), PdfDocumentOpenMode.Modify);
            PdfAcroForm form = doc.AcroForm;

            if (form.Elements.ContainsKey("/NeedAppearances"))
            {
                form.Elements["/NeedAppearances"] = new PdfBoolean(true);
            }
            else
            {
                form.Elements.Add("/NeedAppearances", new PdfBoolean(true));
            }

            if (form.Fields["par64"] as PdfTextField != null) (form.Fields["par64"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["par66"] as PdfTextField != null) (form.Fields["par66"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["par68"] as PdfTextField != null) (form.Fields["par68"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["par95"] as PdfTextField != null) (form.Fields["par95"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["date"] as PdfTextField != null) (form.Fields["date"] as PdfTextField).Value = new PdfString(DateTime.Now.ToString("MM/dd/yyyy"));
            if (form.Fields["time"] as PdfTextField != null) (form.Fields["time"] as PdfTextField).Value = new PdfString(DateTime.Now.ToString("hh:mm:ss tt"));
            if (form.Fields["parelu1"] as PdfTextField != null) (form.Fields["parelu1"] as PdfTextField).Value = new PdfString(preliquidacionDiaCaseta.Parelu1 ?? "");
            if (form.Fields["parelu2"] as PdfTextField != null) (form.Fields["parelu2"] as PdfTextField).Value = new PdfString(preliquidacionDiaCaseta.Parelu2 ?? "");
            if (form.Fields["parelu3"] as PdfTextField != null) (form.Fields["parelu3"] as PdfTextField).Value = new PdfString(preliquidacionDiaCaseta.Parelu3 ?? "");
            if (form.Fields["parelu4"] as PdfTextField != null) (form.Fields["parelu4"] as PdfTextField).Value = new PdfString(preliquidacionDiaCaseta.Parelu4 ?? "");

            string[] stringArray = { "par64", "par66", "par68", "par95", "date", "time", "parelu1", "parelu2", "parelu3", "parelu4" };

            foreach (var fieldName in form.Fields.Names)
            {
                if (!stringArray.Any(s => s.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    var value = (string)(preliquidacionDiaCaseta?.GetType().GetProperty(char.ToUpper(fieldName[0]) + fieldName[1..])?.GetValue(preliquidacionDiaCaseta, null) ?? "");
                    if (form.Fields[fieldName] as PdfTextField != null) (form.Fields[fieldName] as PdfTextField).Value = new PdfString(value);
                }
                form.Fields[fieldName].Elements.SetName($"/T", $"{Guid.NewGuid()}");
            }

            doc.Flatten();
            //doc.Save(Path.Combine("C:/Users/martin/Desktop/PDC.pdf"));
            return doc;

        }
        public PdfDocument GenerarComparativoDiaCaseta(ComparativoDiaCaseta comparativoDiaCaseta)
        {
            PdfDocument doc = PdfReader.Open(Path.Combine(Environment.CurrentDirectory, @"PdfTemplates\", "ComparativoDiaCaseta.pdf"), PdfDocumentOpenMode.Modify);
            PdfAcroForm form = doc.AcroForm;

            if (form.Elements.ContainsKey("/NeedAppearances"))
            {
                form.Elements["/NeedAppearances"] = new PdfBoolean(true);
            }
            else
            {
                form.Elements.Add("/NeedAppearances", new PdfBoolean(true));
            }

            if (form.Fields["par426"] as PdfTextField != null) (form.Fields["par426"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["date"] as PdfTextField != null) (form.Fields["date"] as PdfTextField).Value = new PdfString(DateTime.Now.ToString("MM/dd/yyyy"));

            string[] stringArray = { "par426", "date" };

            foreach (var fieldName in form.Fields.Names)
            {
                if (!stringArray.Any(s => s.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    var value = (string)(comparativoDiaCaseta?.GetType().GetProperty(char.ToUpper(fieldName[0]) + fieldName[1..])?.GetValue(comparativoDiaCaseta, null) ?? "");
                    if (form.Fields[fieldName] as PdfTextField != null) (form.Fields[fieldName] as PdfTextField).Value = new PdfString(value);
                }
                form.Fields[fieldName].Elements.SetName($"/T", $"{Guid.NewGuid()}");
            }

            doc.Flatten();
            //doc.Save(Path.Combine("C:/Users/martin/Desktop/CDC.pdf"));
            return doc;
        }
        public PdfDocument GenerarPrueba(ComparativoDiaCaseta comparativoDiaCaseta)
        {
            Color TableBlue = new Color(235, 240, 249);
            Color TableBorder = new Color(81, 125, 192);
            PdfDocument doc = PdfReader.Open(Path.Combine(Environment.CurrentDirectory, @"PdfTemplates\", "DescuentosDetalle.pdf"), PdfDocumentOpenMode.Modify);
            PdfAcroForm form = doc.AcroForm;

            if (form.Elements.ContainsKey("/NeedAppearances"))
            {
                form.Elements["/NeedAppearances"] = new PdfBoolean(true);
            }
            else
            {
                form.Elements.Add("/NeedAppearances", new PdfBoolean(true));
            }

            //if (form.Fields["par426"] as PdfTextField != null) (form.Fields["par426"] as PdfTextField).Value = new PdfString("0");
            if (form.Fields["fechafin"] as PdfTextField != null) (form.Fields["fechafin"] as PdfTextField).Value = new PdfString(DateTime.Now.ToString("MM/dd/yyyy"));

            //string[] stringArray = { "par426", "date" };

            //foreach (var fieldName in form.Fields.Names)
            //{
            //    if (!stringArray.Any(s => s.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase)))
            //    {
            //        var value = (string)(comparativoDiaCaseta?.GetType().GetProperty(char.ToUpper(fieldName[0]) + fieldName[1..])?.GetValue(comparativoDiaCaseta, null) ?? "");
            //        if (form.Fields[fieldName] as PdfTextField != null) (form.Fields[fieldName] as PdfTextField).Value = new PdfString(value);
            //    }
            //    form.Fields[fieldName].Elements.SetName($"/T", $"{Guid.NewGuid()}");
            //}

            Document document = new();
            Section section = document.AddSection();
            var table = section.AddTable();
            table.Style = "Table";
            table.Borders.Color = TableBorder;
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 0;

            Column column = table.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;
            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Right;
            column = table.AddColumn("3.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;
            column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = TableBlue;
            row.Cells[0].AddParagraph("Item");
            row.Cells[0].Format.Font.Bold = false;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
            row.Cells[0].MergeDown = 1;
            row.Cells[1].AddParagraph("Title and Author");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[1].MergeRight = 3;
            row.Cells[5].AddParagraph("Extended Price");
            row.Cells[5].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[5].VerticalAlignment = VerticalAlignment.Bottom;
            row.Cells[5].MergeDown = 1;

            row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = TableBlue;
            row.Cells[1].AddParagraph("Quantity");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[2].AddParagraph("Unit Price");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[3].AddParagraph("Discount (%)");
            row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[4].AddParagraph("Taxable");
            row.Cells[4].Format.Alignment = ParagraphAlignment.Left;

            table.SetEdge(0, 0, 6, 2, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);

            doc.Flatten();
            //doc.Save(Path.Combine("C:/Users/martin/Desktop/CDC.pdf"));
            return doc;
        }
    }
}
