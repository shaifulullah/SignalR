//-----------------------------------------------------------------------
// <copyright file="ReportHeader.cs" company="MDA Corporation">
//     Copyright (c) MDA Corporation. All rights reserved.
// </copyright>
// <author>Lionel Daniel</author>
//-----------------------------------------------------------------------
namespace MDA.Reports
{
    /// <summary>
    /// Report Header Class Header Displayed on each page
    /// </summary>
    public class ReportHeader
    {
        /// <summary>
        /// Initializes a new instance of the ReportHeader class.
        /// </summary>
        /// <param name="headerText">Header Text</param>
        /// <param name="rightHeaderText">Right Header Text</param>
        public ReportHeader(string headerText, string rightHeaderText)
        {
            HeaderText = headerText;
            RightHeaderText = rightHeaderText;
        }

        /// <summary>
        /// Gets or sets Header Text. The Header Text Appears in the Rows below the Report Title
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// Gets or sets Right Header Text. The Right Header Text Appears in the Rows below the
        /// Application Logo
        /// </summary>
        public string RightHeaderText { get; set; }
    }
}