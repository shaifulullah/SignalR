//-----------------------------------------------------------------------
// <copyright file="Reports.cs" company="MDA Corporation">
//     Copyright (c) MDA Corporation. All rights reserved.
// </copyright>
// <author>Lionel Daniel</author>
//-----------------------------------------------------------------------
namespace MDA.Reports
{
    using iTextSharp.text;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// Cell Attributes
    /// </summary>
    public enum CellAttribute
    {
        /// <summary>
        /// Cell Color
        /// </summary>
        COLOR,

        /// <summary>
        /// Cell Background Color
        /// </summary>
        BGCOLOR
    }

    /// <summary>
    /// Cell Format
    /// </summary>
    public enum CellFormat
    {
        /// <summary>
        /// Cell Format Normal
        /// </summary>
        NORMAL,

        /// <summary>
        /// Cell Format Number
        /// </summary>
        NUMBER,

        /// <summary>
        /// Cell Format Currency
        /// </summary>
        CURRENCY,

        /// <summary>
        /// Cell Format Percent
        /// </summary>
        PERCENT
    }

    /// <summary>
    /// Font Type
    /// </summary>
    public enum FontType
    {
        FONT_COLOR,
        FONT_WEIGHT
    }

    /// <summary>
    /// Format Element
    /// </summary>
    public enum FormatElement
    {
        CELL_FONT,
        CELL_BACKGROUND,
        CELL_ROW_FONT,
        CELL_ROW_BACKGROUND,
        COLUMN_HEADER_BACKGROUND,
        CELL_ROW_BOTTOM_BORDER,
        CELL_ROW_FORMAT
    }

    /// <summary>
    /// Format Field Type
    /// </summary>
    public enum FormatFieldType
    {
        COLUMN,
        VALUE,
        MULTIPLE_VALUES
    }

    /// <summary>
    /// Format Operator
    /// </summary>
    public enum FormatOperator
    {
        EQ,
        NEQ,
        CONTROLBREAK,
        HEADER
    }

    /// <summary>
    /// Report Layout
    /// </summary>
    public enum Layout
    {
        /// <summary>
        /// Report Layout Flow, no Page Breaks
        /// </summary>
        FLOW,

        /// <summary>
        /// Report Layout Page, Page Breaks
        /// </summary>
        PAGE
    }

    /// <summary>
    /// Page Orientation
    /// </summary>
    public enum PageOrientation
    {
        /// <summary>
        /// Page Orientation Portrait
        /// </summary>
        PORTRAIT,

        /// <summary>
        /// Page Orientation Landscape
        /// </summary>
        LANDSCAPE
    }

    /// <summary>
    /// Report Format
    /// </summary>
    public enum ReportFormat
    {
        /// <summary>
        /// PDF Report
        /// </summary>
        PDF = 1,

        /// <summary>
        /// Excel Report
        /// </summary>
        XLS,

        /// <summary>
        /// Html Report
        /// </summary>
        HTML
    }

    /// <summary>
    /// Show Grid
    /// </summary>
    public enum ShowGrid
    {
        /// <summary>
        /// Hide Grid
        /// </summary>
        HIDE,

        /// <summary>
        /// Show Grid
        /// </summary>
        SHOW
    }

    /// <summary>
    /// Abstract Base class for Reports
    /// </summary>
    /// <typeparam name="T">Report Type</typeparam>
    public abstract class Reports<T>
    {
        /// <summary>
        /// Initializes a new instance of the Reports class.
        /// </summary>
        protected Reports()
        {
            ReportOrientation = PageOrientation.PORTRAIT;
            ShowGridLines = ShowGrid.SHOW;
            Attributes = new ReportAttributes();
            ReportPageSize = PageSize.A4;
            UserName = string.Empty;

            AlternateReportData = null;
        }

        /// <summary>
        /// Gets or sets Additional Header List Additional Header Information for the Report to be Displayed
        /// </summary>
        public ReportHeader[] AdditionalHeaderList { get; set; }

        /// <summary>
        /// Gets or sets Alternate Report Data
        /// </summary>
        public DataTable AlternateReportData { get; set; }

        /// <summary>
        /// Gets or sets the Application Logo. The Application Logo will appear on the Left Top
        /// Corner of every Page. The Application Logo is ignored for Excel Spreadsheets
        /// </summary>
        public string ApplicationLogo { get; set; }

        /// <summary>
        /// Gets or sets Attributes
        /// </summary>
        public ReportAttributes Attributes { get; set; }

        /// <summary>
        /// Gets or sets List of Column Headers
        /// </summary>
        public string[] ColumnHeaders { get; set; }

        /// <summary>
        /// Gets or sets Column Width in Percent The column widths of all the columns that appear in
        /// a Report. Specify ColumnWidthInPercent only for PDF Reports.
        /// </summary>
        public float[] ColumnWidthInPercent { get; set; }

        /// <summary>
        /// Gets or sets the Company Logo. The Company Logo will appear on the Right Top Corner of
        /// every Page. The Company Logo is ignored for Excel Spreadsheets
        /// </summary>
        public string CompanyLogo { get; set; }

        /// <summary>
        /// Gets or sets Header List Optional extra headers to be displayed below the Report Title.
        /// </summary>
        public ReportHeader[] HeaderList { get; set; }

        /// <summary>
        /// Gets or sets Columns to Output on Report
        /// </summary>
        public string[] OutputColumns { get; set; }

        /// <summary>
        /// Gets or sets Report Data
        /// </summary>
        public IEnumerable<T> ReportData { get; set; }

        /// <summary>
        /// Gets or sets the Report File Name. This is the File Name with which the Report is created.
        /// </summary>
        public string ReportFileName { get; set; }

        /// <summary>
        /// Gets or sets the Report File Path. This is the Physical Path where the Report is created.
        /// </summary>
        public string ReportFilePath { get; set; }

        /// <summary>
        /// Gets or sets the Report Footer.
        /// </summary>
        public string ReportFooter { get; set; }

        /// <summary>
        /// Gets or sets the Report Page Orientation.
        /// </summary>
        public PageOrientation ReportOrientation { get; set; }

        /// <summary>
        /// Gets or sets the Report Page Size. The Report Page Size is ignored in Excel Spreadsheets.
        /// </summary>
        public Rectangle ReportPageSize { get; set; }

        /// <summary>
        /// Gets or sets the Report Title.
        /// </summary>
        public string ReportTitle { get; set; }

        /// <summary>
        /// Gets or sets Show Grid Lines. The Show Grid Lines is ignored in Excel Spreadsheets.
        /// </summary>
        public ShowGrid ShowGridLines { get; set; }

        /// <summary>
        /// Gets or sets the Source File. Used when appending page to Source File.
        /// </summary>
        public string SourceFile { get; set; }

        /// <summary>
        /// Gets or sets the UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Override to provide your own implementation of GenerateDocument().
        /// </summary>
        /// <returns>True on Success, False on Failure</returns>
        public abstract bool GenerateReport();

        /// <summary>
        /// Convert a IEnumerable(T) to a DataTable.
        /// </summary>
        /// <param name="items">Report Collection</param>
        /// <param name="xmlConvert">XML Convert</param>
        /// <returns>Data Table</returns>
        public DataTable ToDataTable(IEnumerable<T> items, bool xmlConvert)
        {
            if (AlternateReportData == null)
            {
                var dataTable = new DataTable(typeof(T).Name);

                // Get the Columns
                if (OutputColumns == null || OutputColumns.Length <= 0)
                {
                    var propertyNameList = new List<string>();

                    typeof(T).GetProperties().ToList().ForEach(x => propertyNameList.Add(x.Name));
                    OutputColumns = propertyNameList.ToArray();
                }

                var columnIndex = 0;
                foreach (var column in OutputColumns)
                {
                    if (ColumnHeaders == null || ColumnHeaders.Length <= 0)
                    {
                        dataTable.Columns.Add(GetDisplayName(typeof(T), column), GetPropertyType(typeof(T), column));
                    }
                    else
                    {
                        dataTable.Columns.Add(ColumnHeaders[columnIndex++], GetPropertyType(typeof(T), column));
                    }
                }

                // Get the Rows
                foreach (var item in items)
                {
                    var valuesList = new List<object>();
                    foreach (var column in OutputColumns)
                    {
                        var value = GetPropertyValue(item, column);

                        var propertyType = GetPropertyType(typeof(T), column);
                        value = (xmlConvert && value != null && propertyType == typeof(string)) ? XmlConvert.EncodeName(value.ToString().Trim()) : value;

                        valuesList.Add(value);
                    }

                    dataTable.Rows.Add(valuesList.ToArray());
                }

                return dataTable;
            }
            else
            {
                foreach (DataColumn dataColumn in AlternateReportData.Columns)
                {
                    dataColumn.ColumnName = string.IsNullOrEmpty(dataColumn.Caption) ? dataColumn.ColumnName : dataColumn.Caption;
                }

                return AlternateReportData;
            }
        }

        /// <summary>
        /// Get Display Name
        /// </summary>
        /// <param name="type">Data Type</param>
        /// <param name="propertyName">Property Name</param>
        /// <returns>Display Name</returns>
        protected string GetDisplayName(Type type, string propertyName)
        {
            var displayName = string.Empty;
            foreach (var property in propertyName.Split('.').Select(x => type.GetProperty(x)))
            {
                type = GetUnderlyingType(property.PropertyType);

                var attributes = property.GetCustomAttributes(typeof(DisplayAttribute), true);
                displayName = (attributes.Length == 0) ? null : (attributes[0] as DisplayAttribute).GetName();
            }

            return displayName;
        }

        /// <summary>
        /// Get Property Type
        /// </summary>
        /// <param name="type">Data Type</param>
        /// <param name="propertyName">Property Name</param>
        /// <returns>Property Type</returns>
        protected Type GetPropertyType(Type type, string propertyName)
        {
            foreach (var property in propertyName.Split('.').Select(x => type.GetProperty(x)))
            {
                type = GetUnderlyingType(property.PropertyType);
            }

            return type;
        }

        /// <summary>
        /// Get Property Value
        /// </summary>
        /// <param name="dataObject">Data Object</param>
        /// <param name="propertyName">Property Name</param>
        /// <returns>Property Value</returns>
        protected object GetPropertyValue(object dataObject, string propertyName)
        {
            foreach (var property in propertyName.Split('.').Select(x => dataObject.GetType().GetProperty(x)))
            {
                dataObject = property.GetValue(dataObject, null);
            }

            return dataObject;
        }

        /// <summary>
        /// Get Time Stamp
        /// </summary>
        /// <param name="value">Date Time Value</param>
        /// <returns>Date Time Stamp</returns>
        protected string GetTimeStamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        /// <summary>
        /// Return underlying type if type is nullable otherwise return the type
        /// </summary>
        /// <param name="type">Data Type</param>
        /// <returns>Type of T</returns>
        protected Type GetUnderlyingType(Type type)
        {
            if (type != null && IsNullable(type))
            {
                if (!type.IsValueType)
                {
                    return type;
                }

                return Nullable.GetUnderlyingType(type);
            }

            return type;
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        /// <param name="type">Data Type</param>
        /// <returns>True or False</returns>
        protected bool IsNullable(Type type)
        {
            return !type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
    }
}