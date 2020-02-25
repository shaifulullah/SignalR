using System.Collections.Generic;
using System.Globalization;

namespace MDA.Reports
{
    public class ReportAttributes
    {
        public ReportAttributes()
        {
            DateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            ControlBreakDetailInsertNewLine = false;
            DecimalFormat = "0.##";
            NumberFormat = "0";
        }

        /// <summary>
        /// Gets or sets ControlBreakColumns
        /// </summary>
        public Dictionary<string, string[]> ControlBreakColumns { get; set; }

        /// <summary>
        /// Gets or sets ControlBreakDetailInsertNewLine
        /// </summary>
        public bool ControlBreakDetailInsertNewLine { get; set; }

        /// <summary>
        /// Gets or sets DateFormat
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// Gets or sets DecimalFormat
        /// </summary>
        public string DecimalFormat { get; set; }

        /// <summary>
        /// Gets or sets GrandTotalColumns
        /// </summary>
        public string[] GrandTotalColumns { get; set; }

        /// <summary>
        /// Gets or sets NumberFormat
        /// </summary>
        public string NumberFormat { get; set; }

        /// <summary>
        /// Gets or sets ReportColumns
        /// </summary>
        public string[] ReportColumns { get; set; }

        /// <summary>
        /// Gets or sets SortColumn
        /// </summary>
        public string SortColumn { get; set; }

        /// <summary>
        /// Gets or sets SubTotalColumns
        /// </summary>
        public Dictionary<string, string[]> SubTotalColumns { get; set; }
    }
}