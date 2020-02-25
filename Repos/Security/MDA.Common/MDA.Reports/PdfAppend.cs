namespace MDA.Reports
{
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using System;
    using System.Data;
    using System.IO;

    /// <summary>
    /// Generates Report in PDF Format
    /// </summary>
    /// <typeparam name="T">Report Type</typeparam>
    public class PdfAppend<T> : PdfReport<T>
    {
        /// <summary>
        /// Generate Report
        /// </summary>
        /// <returns>True on Success, False on Failure</returns>
        public override bool GenerateReport()
        {
            ReportDataTable = ToDataTable(ReportData, false);
            if (ReportDataTable == null || ReportDataTable.Rows.Count <= 0 || !File.Exists(SourceFile))
            {
                return false;
            }

            ReportFileName = string.Format("{0}_{1}.pdf", ReportDataTable.TableName, GetTimeStamp(DateTime.Now));
            ReportFilePath = string.Format("{0}{1}", ReportFilePath, ReportFileName);
            ReportPageSize = (ReportOrientation == PageOrientation.PORTRAIT) ? ReportPageSize : ReportPageSize.Rotate();

            using (var document = new Document(ReportPageSize, MarginLeft, MarginRight, MarginTop, MarginBottom))
            {
                // Create Page Events Objects
                var documentWriter = PdfWriter.GetInstance(document, new FileStream(ReportFilePath, FileMode.Create));

                // Open the Document
                document.Open();

                // Add the Source Data
                var pdfReader = new PdfReader(SourceFile);
                for (var i = 1; i <= pdfReader.NumberOfPages; i++)
                {
                    var page = documentWriter.GetImportedPage(pdfReader, i);
                    documentWriter.DirectContent.AddTemplate(page, 0f, 0f);
                    document.NewPage();
                }

                // Generate Report Data
                GenerateReport(document);
            }

            return true;
        }

        /// <summary>
        /// Generate Report
        /// </summary>
        /// <param name="document">Document Object</param>
        protected void GenerateReport(Document document)
        {
            // Header Table
            var pdfHeaderTable = new PdfPTable(3) { WidthPercentage = 100 };
            pdfHeaderTable.SetWidths(new float[] { 55, 30, 15 });

            pdfHeaderTable.DefaultCell.BorderWidth = 0;
            pdfHeaderTable.DefaultCell.Padding = 0;
            pdfHeaderTable.DefaultCell.PaddingBottom = 10;

            var applicationLogo = Image.GetInstance(CompanyLogo);

            pdfHeaderTable.AddCell(new Phrase(ReportTitle, DocumentTitleFont));
            pdfHeaderTable.AddCell(string.Empty);
            pdfHeaderTable.AddCell(applicationLogo);

            document.Add(pdfHeaderTable);

            // Data Table
            var pdfDataTable = new PdfPTable(ReportDataTable.Columns.Count - 1) { WidthPercentage = 100 };
            pdfDataTable.DefaultCell.Padding = 3;

            pdfDataTable.DefaultCell.Border = Rectangle.NO_BORDER;
            pdfDataTable.SetWidths(new float[] { 30, 30, 20, 20 });

            pdfDataTable.AddCell(FormatHeaderRowPhrase(""));
            pdfDataTable.AddCell(FormatHeaderRowPhrase("Name"));
            pdfDataTable.AddCell(FormatHeaderRowPhrase("Action"));
            pdfDataTable.AddCell(FormatHeaderRowPhrase("Date"));

            var section = string.Empty;
            foreach (DataRow row in ReportDataTable.Rows)
            {
                for (var i = 0; i < ColumnHeaders.Length; i++)
                {
                    if (i == 0)
                    {
                        if (section != row[ColumnHeaders[0]].ToString())
                        {
                            section = row[ColumnHeaders[0]].ToString();
                            pdfDataTable.AddCell(new PdfPCell(FormatHeaderRowPhrase(section)) { Colspan = ColumnHeaders.Length - 1, Border = Rectangle.NO_BORDER });
                        }
                    }
                    else
                    {
                        pdfDataTable.AddCell(FormatDataRowPhrase(row[ColumnHeaders[i]].ToString()));
                    }
                }
            }

            document.Add(pdfDataTable);
        }
    }
}