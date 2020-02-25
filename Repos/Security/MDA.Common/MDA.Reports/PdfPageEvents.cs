//-----------------------------------------------------------------------
// <copyright file="PdfPageEvents.cs" company="MDA Corporation">
//     Copyright (c) MDA Corporation. All rights reserved.
// </copyright>
// <author>Lionel Daniel</author>
//-----------------------------------------------------------------------

namespace MDA.Reports
{
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using System.Globalization;

    /// <summary>
    /// PDF Page Events Class Event occurs when a new page is created
    /// </summary>
    /// <typeparam name="T">Report Type</typeparam>
    public class PdfPageEvents<T> : PdfPageEventHelper
    {
        /// <summary>
        /// Initializes a new instance of the PdfPageEvents class.
        /// </summary>
        public PdfPageEvents()
        {
            ContentByte = null;
            DocumentTemplate = null;
            BaseFont = null;
        }

        /// <summary>
        /// Gets or sets PDF Document Object
        /// </summary>
        public PdfReport<T> PdfDocument { get; set; }

        /// <summary>
        /// Gets or sets Base Font
        /// </summary>
        protected BaseFont BaseFont { get; set; }

        /// <summary>
        /// Gets or sets PDF Content Byte
        /// </summary>
        protected PdfContentByte ContentByte { get; set; }

        /// <summary>
        /// Gets or sets Document Template
        /// </summary>
        protected PdfTemplate DocumentTemplate { get; set; }

        /// <summary>
        /// On Close Document
        /// </summary>
        /// <param name="writer">PDF Writer</param>
        /// <param name="document">PDF Document</param>
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            // Now we have the total number of Pages (Page Number of the Last Page) Fill the
            // template with total number of pages
            DocumentTemplate.BeginText();
            DocumentTemplate.SetFontAndSize(BaseFont, 8);
            DocumentTemplate.ShowText((writer.PageNumber).ToString(CultureInfo.InvariantCulture));
            DocumentTemplate.EndText();

            base.OnCloseDocument(writer, document);
        }

        /// <summary>
        /// On End Page
        /// </summary>
        /// <param name="writer">PDF Writer</param>
        /// <param name="document">PDF Document</param>
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            var pagerText = string.Format("Page {0} of ", writer.PageNumber);

            var pagerLength = BaseFont.GetWidthPoint(pagerText, 8);

            var offsetRight = (document.Right - document.RightMargin) - pagerLength;
            var offsetLeft = document.LeftMargin;

            // User Name
            ContentByte.BeginText();
            ContentByte.SetFontAndSize(BaseFont, 8);
            ContentByte.SetTextMatrix(offsetLeft, 11);
            ContentByte.ShowText(PdfDocument.UserName);
            ContentByte.EndText();

            // Pager
            ContentByte.BeginText();
            ContentByte.SetFontAndSize(BaseFont, 8);
            ContentByte.SetTextMatrix(offsetRight, 11);
            ContentByte.ShowText(pagerText);
            ContentByte.EndText();

            // Template to Fill in the total number of Pages. This template will be filled in the
            // end once we have the total number of Pages
            ContentByte.AddTemplate(DocumentTemplate, offsetRight + pagerLength, 11);
            ContentByte.BeginText();
            ContentByte.SetFontAndSize(BaseFont, 8);
            ContentByte.SetTextMatrix(offsetRight, 11);
            ContentByte.EndText();

            // Call the Base class version
            base.OnEndPage(writer, document);
        }

        /// <summary>
        /// On Open Document
        /// </summary>
        /// <param name="writer">PDF Writer</param>
        /// <param name="document">PDF Document</param>
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            BaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            ContentByte = writer.DirectContent;
            DocumentTemplate = ContentByte.CreateTemplate(50, 50);
        }

        /// <summary>
        /// On Start Page
        /// </summary>
        /// <param name="writer">PDF Writer</param>
        /// <param name="document">PDF Document</param>
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            PdfPTable documentHeaderTable;
            if (!string.IsNullOrEmpty(PdfDocument.ApplicationLogo))
            {
                documentHeaderTable = new PdfPTable(3);
                documentHeaderTable.SetWidths(new float[] { 10, 70, 20 });
            }
            else
            {
                documentHeaderTable = new PdfPTable(2);
                documentHeaderTable.SetWidths(new float[] { 80, 20 });
            }

            documentHeaderTable.WidthPercentage = 100;
            documentHeaderTable.DefaultCell.BorderWidth = 0;
            documentHeaderTable.DefaultCell.Padding = 0;
            documentHeaderTable.DefaultCell.PaddingBottom = 10;

            if (!string.IsNullOrEmpty(PdfDocument.ApplicationLogo))
            {
                documentHeaderTable.AddCell(GetLeftHeader());
            }

            documentHeaderTable.AddCell(GetHeader());
            documentHeaderTable.AddCell(GetRightHeader());

            document.Add(documentHeaderTable);
            base.OnStartPage(writer, document);
        }

        /// <summary>
        /// Header to be Displayed in the Center
        /// </summary>
        /// <returns>Header Table</returns>
        protected PdfPTable GetHeader()
        {
            var headerTable = new PdfPTable(1);
            headerTable.DefaultCell.BorderWidth = 0;
            headerTable.DefaultCell.Padding = 0;

            // Add the Document Title
            headerTable.DefaultCell.PaddingBottom = 5;
            headerTable.AddCell(new Phrase(PdfDocument.ReportTitle, PdfDocument.DocumentTitleFont));

            // Add the Remaining
            headerTable.DefaultCell.PaddingBottom = 1;
            if (PdfDocument.HeaderList != null)
            {
                foreach (var reportHeader in PdfDocument.HeaderList)
                {
                    headerTable.AddCell(new Phrase(reportHeader.HeaderText, PdfDocument.DataRowFont));
                }
            }

            // Add selection Header list if any
            if (PdfDocument.AdditionalHeaderList != null)
            {
                headerTable.AddCell(new Phrase("\n"));
                foreach (var reportHeader in PdfDocument.AdditionalHeaderList)
                {
                    headerTable.AddCell(new Phrase(string.Format("{0}\n", reportHeader.HeaderText), PdfDocument.DataRowFont));
                }
                headerTable.AddCell(new Phrase("\n"));
            }

            return headerTable;
        }

        /// <summary>
        /// Header to be Displayed on the Left Side
        /// </summary>
        /// <returns>Header Table</returns>
        protected PdfPTable GetLeftHeader()
        {
            // This side of the Header will only display the application Logo
            var leftHeaderTable = new PdfPTable(1);

            leftHeaderTable.DefaultCell.BorderWidth = 0;
            leftHeaderTable.DefaultCell.Padding = 0;
            leftHeaderTable.DefaultCell.PaddingRight = 10;

            // Add The Application Logo
            var applicationLogo = Image.GetInstance(PdfDocument.ApplicationLogo);
            leftHeaderTable.AddCell(applicationLogo);

            return leftHeaderTable;
        }

        /// <summary>
        /// Header to be Displayed on the Right Side
        /// </summary>
        /// <returns>Header Table</returns>
        protected PdfPTable GetRightHeader()
        {
            var rightHeaderTable = new PdfPTable(1);
            rightHeaderTable.DefaultCell.BorderWidth = 0;
            rightHeaderTable.DefaultCell.Padding = 0;
            rightHeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

            // Add the Company Logo
            rightHeaderTable.DefaultCell.PaddingBottom = 5;

            if (!string.IsNullOrEmpty(PdfDocument.CompanyLogo))
            {
                var companyLogo = Image.GetInstance(PdfDocument.CompanyLogo);
                companyLogo.ScalePercent(50f);

                PdfPCell logoCell = new PdfPCell(companyLogo);
                logoCell.BorderWidth = 0;
                logoCell.Padding = 0;
                logoCell.PaddingBottom = 5;
                logoCell.HorizontalAlignment = Element.ALIGN_RIGHT;

                rightHeaderTable.AddCell(logoCell);
            }

            // Add the Remaining
            rightHeaderTable.DefaultCell.PaddingBottom = 1;
            if (PdfDocument.HeaderList != null)
            {
                foreach (var reportHeader in PdfDocument.HeaderList)
                {
                    rightHeaderTable.AddCell(new Phrase(reportHeader.RightHeaderText, PdfDocument.DataRowFont));
                }
            }

            return rightHeaderTable;
        }
    }
}