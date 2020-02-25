//-----------------------------------------------------------------------
// <copyright file="ExcelReport.cs" company="MDA Corporation">
//     Copyright (c) MDA Corporation. All rights reserved.
// </copyright>
// <author>Lionel Daniel</author>
//-----------------------------------------------------------------------

namespace MDA.Reports
{
    using ClosedXML.Excel;
    using System;
    using System.Data;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// Generates Report in XLS Format
    /// </summary>
    /// <typeparam name="T">Report Type</typeparam>
    public class ExcelReport<T> : Reports<T>
    {
        /// <summary>
        /// Gets or sets Report Data Table
        /// </summary>
        protected DataTable ReportDataTable { get; set; }

        /// <summary>
        /// Get Worksheet Name
        /// </summary>
        /// <param name="workbook">Excel Workbook Object</param>
        /// <param name="worksheetName">Worksheet Name</param>
        /// <returns>Worksheet Name</returns>
        public static string GetWorksheetName(XLWorkbook workbook, string worksheetName)
        {
            worksheetName = string.IsNullOrEmpty(worksheetName) ? Guid.NewGuid().ToString() : XmlConvert.DecodeName(worksheetName).RemoveSpecialCharacters();

            var sheetCount = workbook.Worksheets.Where(x => x.Name.Split(' ')[0].Trim() == worksheetName.Trim()).Count();
            var excessLength = (worksheetName.Length + sheetCount.ToString().Length + 3) - 32;

            worksheetName = worksheetName.Substring(0, worksheetName.Length - (excessLength > 0 ? excessLength : 0));
            worksheetName = sheetCount > 0 ? string.Format("{0} ({1})", worksheetName, sheetCount) : worksheetName;

            return worksheetName;
        }

        /// <summary>
        /// Generate Report
        /// </summary>
        /// <returns>True on success, False on failure</returns>
        public override bool GenerateReport()
        {
            ReportDataTable = ToDataTable(ReportData, true);
            if (ReportDataTable == null || ReportDataTable.Rows.Count <= 0)
            {
                return false;
            }

            ReportFileName = string.Format("{0}_{1}.xlsx", string.IsNullOrEmpty(ReportFileName) ? ReportDataTable.TableName : ReportFileName, GetTimeStamp(DateTime.Now));
            ReportFilePath = string.Format("{0}{1}", ReportFilePath, ReportFileName);

            // Initialise Excel Workbook
            using (var workbook = new XLWorkbook())
            {
                // Generate the Report Data
                GenerateReportData(workbook, ReportDataTable);

                // Save Workbook
                workbook.SaveAs(ReportFilePath);
            }

            return true;
        }

        /// <summary>
        /// Generate Report Data
        /// </summary>
        /// <param name="workbook">Excel Workbook Object</param>
        /// <param name="reportDataTable">Report Data Table</param>
        public virtual void GenerateReportData(XLWorkbook workbook, DataTable reportDataTable)
        {
            var tableName = reportDataTable.TableName;

            var worksheetName = GetWorksheetName(workbook, tableName);
            using (var worksheet = workbook.Worksheets.Add(worksheetName))
            {
                PrintWorksheetHeader(worksheet, reportDataTable.Columns.Count);
                PrintWorksheetData(worksheet, reportDataTable);
            }
        }

        /// <summary>
        /// Print Worksheet Data
        /// </summary>
        /// <param name="worksheet">Worksheet Object</param>
        /// <param name="reportDataTable">Report Data Table</param>
        public void PrintWorksheetData(IXLWorksheet worksheet, DataTable reportDataTable)
        {
            var row = worksheet.Rows().Count() + 2;
            var tableName = reportDataTable.TableName;

            // Insert Report Data
            worksheet.Cell(row, 1).InsertTable(reportDataTable, tableName).Theme = XLTableTheme.None;

            // Table Theme
            worksheet.Table(tableName).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            worksheet.Table(tableName).HeadersRow().Style.Fill.BackgroundColor = XLColor.Silver;
            worksheet.Table(tableName).HeadersRow().Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            // Format Columns
            foreach (DataColumn dataColumn in reportDataTable.Columns)
            {
                if (dataColumn.DataType == typeof(DateTime)) { worksheet.Column(dataColumn.Ordinal + 1).Style.DateFormat.Format = Attributes.DateFormat; }
                if (dataColumn.DataType == typeof(Int32) || dataColumn.DataType == typeof(Int64)) { worksheet.Column(dataColumn.Ordinal + 1).Style.NumberFormat.Format = Attributes.NumberFormat; }
            }
        }

        /// <summary>
        /// Print Worksheet Header
        /// </summary>
        /// <param name="worksheet">Worksheet Object</param>
        /// <param name="columnCount">Column Count</param>
        public void PrintWorksheetHeader(IXLWorksheet worksheet, int columnCount)
        {
            var row = 1;

            // Report Header
            worksheet.Range(row, 1, row, columnCount).Merge().Value = ReportTitle;
            worksheet.Cell(row, 1).Style.Font.FontSize = 20;

            // Additional Report Headers
            foreach (var reportHeader in HeaderList)
            {
                worksheet.Cell(++row, 1).SetValue(reportHeader.HeaderText);
                if (!string.IsNullOrEmpty(reportHeader.RightHeaderText))
                {
                    worksheet.Cell(row, columnCount).SetValue(reportHeader.RightHeaderText).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                }
            }
        }
    }
}