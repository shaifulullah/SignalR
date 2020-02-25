//-----------------------------------------------------------------------
// <copyright file="PdfReport.cs" company="MDA Corporation">
//     Copyright (c) MDA Corporation. All rights reserved.
// </copyright>
// <author>Lionel Daniel</author>
//-----------------------------------------------------------------------

using System.Globalization;

namespace MDA.Reports
{
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Generates Report in PDF Format
    /// </summary>
    /// <typeparam name="T">Report Type</typeparam>
    public class PdfReport<T> : Reports<T>
    {
        /// <summary>
        /// Margin Bottom
        /// </summary>
        protected const int MarginBottom = 20;

        /// <summary>
        /// Margin Left
        /// </summary>
        protected const int MarginLeft = 10;

        /// <summary>
        /// Margin Right
        /// </summary>
        protected const int MarginRight = 10;

        /// <summary>
        /// Margin Top
        /// </summary>
        protected const int MarginTop = 10;

        /// <summary>
        /// Initializes a new instance of the PdfReport class.
        /// </summary>
        public PdfReport()
        {
            DocumentTitleFont = FontFactory.GetFont(FontFactory.HELVETICA, 20, Font.NORMAL);
            HeaderRowFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8.0F, Font.NORMAL);
            DataRowFont = FontFactory.GetFont(BaseFont.HELVETICA, 8.0F, Font.NORMAL);
        }

        /// <summary>
        /// Gets or sets Data Row Font
        /// </summary>
        public Font DataRowFont { get; set; }

        /// <summary>
        /// Gets or sets Document Title Font
        /// </summary>
        public Font DocumentTitleFont { get; set; }

        /// <summary>
        /// Gets or sets Header Row Font
        /// </summary>
        public Font HeaderRowFont { get; set; }

        /// <summary>
        /// Gets PdfDataTable
        /// </summary>
        protected PdfPTable PdfDataTable
        {
            get
            {
                var pdfDataTable = new PdfPTable(Attributes.ReportColumns.Count()) { WidthPercentage = 100, HeaderRows = 1 };
                pdfDataTable.DefaultCell.Padding = 3;

                // Set the Column widths
                if (ColumnWidthInPercent != null)
                {
                    pdfDataTable.SetWidths(ColumnWidthInPercent);
                }

                // Column Headers
                foreach (var columnHeader in Attributes.ReportColumns)
                {
                    pdfDataTable.AddCell(FormatHeaderCell(columnHeader));
                }

                return pdfDataTable;
            }
        }

        /// <summary>
        /// Gets or sets Report Data Table
        /// </summary>
        protected DataTable ReportDataTable { get; set; }

        /// <summary>
        /// Format Cell
        /// </summary>
        /// <param name="value">Cell Value</param>
        /// <param name="type">Cell Value Type</param>
        /// <param name="prefix">Prefix Text</param>
        /// <returns>PDF Cell</returns>
        public PdfPCell FormatCell(string value, Type type, string prefix = "")
        {
            value = string.IsNullOrEmpty(value) ? " " : value.Trim();
            PdfPCell pdfPCell = new PdfPCell
            {
                Padding = 3,
                BackgroundColor = new BaseColor(255, 255, 255),
                HorizontalAlignment = type == typeof(String) || type == typeof(Boolean) ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT
            };

            if (ShowGridLines == ShowGrid.SHOW)
            {
                pdfPCell.BorderWidth = 0.01F;
                pdfPCell.BorderColor = new BaseColor(200, 200, 200);
            }

            pdfPCell.Phrase = FormatDataRowPhrase(string.Format("{0}{1}", prefix, FormatValue(value, type)));
            return pdfPCell;
        }

        /// <summary>
        /// Format Header Cell
        /// </summary>
        /// <param name="value">Cell Value</param>
        /// <returns>PDF Cell</returns>
        public PdfPCell FormatHeaderCell(string value)
        {
            PdfPCell pdfPCell = new PdfPCell { Padding = 3, Phrase = FormatHeaderRowPhrase(value), BackgroundColor = new BaseColor(224, 224, 224) };
            if (ShowGridLines == ShowGrid.SHOW)
            {
                pdfPCell.BorderWidth = 0.01F;
                pdfPCell.BorderColor = new BaseColor(200, 200, 200);
            }

            return pdfPCell;
        }

        /// <summary>
        /// Format Value
        /// </summary>
        /// <param name="value">Cell Value</param>
        /// <param name="type">Cell Value Type</param>
        /// <returns>Formatted Value</returns>
        public string FormatValue(string value, Type type)
        {
            //if (type == typeof(DateTime)) { value = string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()) ? value : DateTime.Parse(value).ToString(Attributes.DateFormat); }
            if (type == typeof(Decimal)) { value = string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()) ? value : decimal.Parse(value).ToString(Attributes.DecimalFormat); }

            return value;
        }

        /// <summary>
        /// Generate Report
        /// </summary>
        /// <returns>True on Success, False on Failure</returns>
        public override bool GenerateReport()
        {
            ReportDataTable = ToDataTable(ReportData, false);
            if (ReportDataTable == null || ReportDataTable.Rows.Count <= 0)
            {
                return false;
            }

            ReportFileName = string.Format("{0}_{1}.pdf", string.IsNullOrEmpty(ReportFileName) ? ReportDataTable.TableName : ReportFileName, GetTimeStamp(DateTime.Now));
            ReportFilePath = string.Format("{0}{1}", ReportFilePath, ReportFileName);
            ReportPageSize = (ReportOrientation == PageOrientation.PORTRAIT) ? ReportPageSize : ReportPageSize.Rotate();

            using (var document = new Document(ReportPageSize, MarginLeft, MarginRight, MarginTop, MarginBottom))
            {
                // Create Page Events Objects
                var pdfWriter = PdfWriter.GetInstance(document, new FileStream(ReportFilePath, FileMode.Create));
                var pageEvents = new PdfPageEvents<T> { PdfDocument = this };

                pdfWriter.PageEvent = pageEvents;

                // Open the Document
                document.Open();

                // Generate the Report Data
                GenerateReportData(document);
            }

            return true;
        }

        /// <summary>
        /// Generate Report Data
        /// </summary>
        /// <param name="document">Document Object</param>
        public virtual void GenerateReportData(Document document)
        {
            // Get Report Columns
            Attributes.ReportColumns = ReportDataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();

            Attributes.ReportColumns = Attributes.ControlBreakColumns == null ? Attributes.ReportColumns :
                Attributes.ReportColumns.Where(x => !(Attributes.ControlBreakColumns.Any(y => (y.Key == x || y.Value.Any(z => z == x))))).ToArray();

            var pdfDataTable = PdfDataTable;

            // Initialize Previous Control Break Values List
            var prevControlBreakValuesList = new Dictionary<string, string>();
            if (Attributes.ControlBreakColumns != null)
            {
                Attributes.ControlBreakColumns.Keys.ToList().ForEach(x => prevControlBreakValuesList[x] = null);
            }

            // Initialize SubTotals List
            var subTotalList = new Dictionary<string, decimal>();
            if (Attributes.SubTotalColumns != null)
            {
                Attributes.SubTotalColumns.ToList().ForEach(x => x.Value.ToList().ForEach(y => subTotalList.Add(string.Format("{0}:{1}", x.Key, y), 0)));
            }

            // Initialize GrandTotals List
            var grandTotalList = new Dictionary<string, decimal>();
            if (Attributes.GrandTotalColumns != null)
            {
                Attributes.GrandTotalColumns.ToList().ForEach(x => grandTotalList.Add(x, 0));
            }

            foreach (DataRow row in ReportDataTable.Rows)
            {
                // Check and Print Control Breaks
                if (Attributes.ControlBreakColumns != null)
                {
                    for (var i = 0; i < Attributes.ControlBreakColumns.Count; i++)
                    {
                        var controlBreak = Attributes.ControlBreakColumns.ElementAt(i).Key;
                        if (row[controlBreak].ToString() != prevControlBreakValuesList[controlBreak])
                        {
                            // Print SubTotals
                            if (pdfDataTable.Rows.Count > 1 && Attributes.SubTotalColumns != null)
                            {
                                foreach (var key in Attributes.SubTotalColumns.Keys)
                                {
                                    if (row[key].ToString() != prevControlBreakValuesList[key])
                                    {
                                        foreach (var columnHeader in Attributes.ReportColumns)
                                        {
                                            var index = string.Format("{0}:{1}", key, columnHeader);

                                            pdfDataTable.AddCell(Attributes.SubTotalColumns[key].Contains(columnHeader) ?
                                                FormatCell(subTotalList[index].ToString(CultureInfo.InvariantCulture), ReportDataTable.Columns[columnHeader].DataType, string.Format("Sub Total ({0}) : ", key)) :
                                                FormatCell(string.Empty, typeof(String)));

                                            subTotalList[index] = 0;
                                        }
                                    }
                                }
                            }

                            // Perform Control Break
                            document.Add(pdfDataTable);
                            if (i == 0 && prevControlBreakValuesList[controlBreak] != null)
                            {
                                document.NewPage();
                            }

                            // Clear Previous Control Break Values
                            prevControlBreakValuesList = prevControlBreakValuesList.Select((key, index) => new { key, index })
                                .Select(x => new { key = x.key.Key, value = (x.index > prevControlBreakValuesList.Keys.ToList().IndexOf(controlBreak)) ? null : x.key.Value })
                                .ToDictionary(x => x.key, x => x.value);

                            // Print Control Break Details
                            var controlBreakData = string.Format("{0}{1} : {2}", Environment.NewLine, controlBreak, FormatValue(row[controlBreak].ToString(), row[controlBreak].GetType()));

                            var controlBreakDetail = Attributes.ControlBreakColumns.ElementAt(i).Value;
                            if (controlBreakDetail.Any())
                            {
                                controlBreakDetail.ToList().ForEach(x =>
                                    controlBreakData += string.Format("{0}{1} - {2}", Attributes.ControlBreakDetailInsertNewLine ? Environment.NewLine : ", ", x, FormatValue(row[x].ToString(), row[x].GetType())));
                            }

                            document.Add(FormatHeaderRowPhrase(controlBreakData));

                            // Save New Control Break
                            prevControlBreakValuesList[controlBreak] = row[controlBreak].ToString();
                            pdfDataTable = PdfDataTable;
                        }
                    }
                }

                // Rows
                foreach (var columnHeader in Attributes.ReportColumns)
                {
                    // Accumulate SubTotals
                    if (Attributes.SubTotalColumns != null)
                    {
                        foreach (var key in Attributes.SubTotalColumns.Keys)
                        {
                            if (Attributes.SubTotalColumns[key].Contains(columnHeader))
                            {
                                subTotalList[string.Format("{0}:{1}", key, columnHeader)] += Convert.ToDecimal(row[columnHeader]);
                            }
                        }
                    }

                    // Accumulate GrandTotals
                    if (Attributes.GrandTotalColumns != null && Attributes.GrandTotalColumns.Contains(columnHeader))
                    {
                        grandTotalList[columnHeader] += Convert.ToDecimal(row[columnHeader]);
                    }

                    pdfDataTable.AddCell(FormatCell(row[columnHeader].ToString(), ReportDataTable.Columns[columnHeader].DataType));
                }
            }

            // Print SubTotals
            if (Attributes.SubTotalColumns != null)
            {
                foreach (var key in Attributes.SubTotalColumns.Keys)
                {
                    foreach (var columnHeader in Attributes.ReportColumns)
                    {
                        var index = string.Format("{0}:{1}", key, columnHeader);

                        pdfDataTable.AddCell(Attributes.SubTotalColumns[key].Contains(columnHeader) ?
                            FormatCell(subTotalList[index].ToString(CultureInfo.InvariantCulture), ReportDataTable.Columns[columnHeader].DataType, string.Format("Sub Total ({0}) : ", key)) :
                            FormatCell(string.Empty, typeof(String)));
                    }
                }
            }

            // Print Grand Total
            if (Attributes.GrandTotalColumns != null)
            {
                foreach (var columnHeader in Attributes.ReportColumns)
                {
                    pdfDataTable.AddCell(Attributes.GrandTotalColumns.Contains(columnHeader) ?
                        FormatCell(grandTotalList[columnHeader].ToString(CultureInfo.InvariantCulture), ReportDataTable.Columns[columnHeader].DataType, "Grand Total : ") :
                        FormatCell(string.Empty, typeof(String)));
                }
            }

            // Add the Table
            document.Add(pdfDataTable);

            // Add Footer
            document.Add(new Phrase(string.Format("{0}{1}{2}", Environment.NewLine, Environment.NewLine, ReportFooter), DataRowFont));
        }

        /// <summary>
        /// Format Data Row Phrase
        /// </summary>
        /// <param name="value">Phrase to be Formatted</param>
        /// <returns>Formatted Phrase</returns>
        protected Phrase FormatDataRowPhrase(string value)
        {
            return new Phrase(value, DataRowFont);
        }

        /// <summary>
        /// Format Header Row Phrase
        /// </summary>
        /// <param name="value">Phrase to be Formatted</param>
        /// <returns>Formatted Phrase</returns>
        protected Phrase FormatHeaderRowPhrase(string value)
        {
            return new Phrase(value, HeaderRowFont);
        }
    }
}