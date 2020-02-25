//-----------------------------------------------------------------------
// <copyright file="HtmlReport.cs" company="MDA Corporation">
//     Copyright (c) MDA Corporation. All rights reserved.
// </copyright>
// <author>Lionel Daniel</author>
//-----------------------------------------------------------------------

namespace MDA.Reports
{
    using System;
    using System.Data;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Generates Report in HTML Format
    /// </summary>
    /// <typeparam name="T">Report Type</typeparam>
    public class HtmlReport<T> : Reports<T>
    {
        /// <summary>
        /// Gets HtmlCloseText
        /// </summary>
        protected string HtmlCloseText
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append("</table>");
                sb.Append("</body>");
                sb.Append("</html>");

                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets HtmlOpenText
        /// </summary>
        protected string HtmlOpenText
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append("<!DOCTYPE html>");
                sb.Append("<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>");
                sb.Append("<head>");
                sb.Append("<meta charset='utf-8' />");
                sb.Append("<title>" + ReportTitle + "</title>");
                sb.Append("<style>");
                sb.Append("body {");
                sb.Append("margin: 0px;");
                sb.Append("}");
                sb.Append("html {");
                sb.Append("overflow-y: scroll;");
                sb.Append("font: 75% Arial, Helvetica, Times;");
                sb.Append("}");
                sb.Append("table {");
                sb.Append("border-collapse: collapse;");
                sb.Append("border: 1px solid #c0c0c0;");
                sb.Append("width: 100%;");
                sb.Append("}");
                sb.Append("table td {");
                sb.Append("border: 1px solid #a0a0a0;");
                sb.Append("padding: 3px;");
                sb.Append("}");
                sb.Append("</style>");
                sb.Append("</head>");
                sb.Append("<body onload='javascript:window.print();'>");
                sb.Append("<table>");

                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets or sets Report Data Table
        /// </summary>
        protected DataTable ReportDataTable { get; set; }

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

            ReportFileName = string.Format("{0}_{1}.html", ReportDataTable.TableName, GetTimeStamp(DateTime.Now));
            ReportFilePath = string.Format("{0}{1}", ReportFilePath, ReportFileName);

            using (var sw = File.CreateText(ReportFilePath))
            {
                sw.Write(HtmlOpenText);

                // Columns
                var rowData = "<tr>";
                for (var i = 0; i < ReportDataTable.Columns.Count; i++)
                {
                    rowData += string.Format("<td style='background-color: #e0e0e0'><b>{0}</b></td>", ReportDataTable.Columns[i]);
                }

                rowData += "</tr>";
                sw.Write(rowData);

                // Rows
                foreach (DataRow row in ReportDataTable.Rows)
                {
                    int i = 0;
                    rowData = "<tr>";

                    foreach (var cell in row.ItemArray)
                    {
                        rowData += FormatValue(cell.ToString(), ReportDataTable.Columns[i++].DataType);
                    }

                    rowData += "</tr>";
                    sw.Write(rowData);
                }

                sw.Write(HtmlCloseText);
            }

            return true;
        }

        /// <summary>
        /// Format Value
        /// </summary>
        /// <param name="value">Cell Value</param>
        /// <param name="type">Cell Value Type</param>
        /// <returns>Formatted Value</returns>
        private string FormatValue(string value, Type type)
        {
            if (type == typeof(DateTime))
            {
                value = string.Format("<td style='text-align:right'>{0}</td>",
                    string.IsNullOrEmpty(value) ? value : DateTime.Parse(value).ToString(Attributes.DateFormat));
            }
            else
            {
                value = string.Format("<td>{0}</td>", value);
            }

            return value;
        }
    }
}