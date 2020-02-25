//-----------------------------------------------------------------------
// <copyright file="Button.cs" company="MDA Corporation">
//     Copyright (c) MDA Corporation. All rights reserved.
// </copyright>
// <author>Lionel Daniel</author>
//-----------------------------------------------------------------------
namespace MDA.Core.Helpers
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;

    /// <summary>
    /// Static Button Helper Class
    /// </summary>
    public static class ButonHelper
    {
        /// <summary>
        /// Renders a Button if User has the required Rights
        /// </summary>
        /// <param name="html">HTML Helper</param>
        /// <param name="innerHtml">Inner HTML</param>
        /// <param name="htmlAttributes">HTML Attributes</param>
        /// <param name="canRender">Can Render</param>
        /// <returns>HTML to Render Control</returns>
        public static Button Button(this HtmlHelper html, string innerHtml, object htmlAttributes, bool canRender = true)
        {
            return new Button(html, innerHtml, htmlAttributes, canRender);
        }
    }

    /// <summary>
    /// Renders a Button if User has the required Rights
    /// </summary>
    public class Button : IHtmlString
    {
        /// <summary>
        /// HTML Helper
        /// </summary>
        private readonly HtmlHelper HtmlHelper;

        /// <summary>
        /// Can Render Control
        /// </summary>
        private bool CanRender;

        /// <summary>
        /// HTML Attributes
        /// </summary>
        private IDictionary<string, object> HtmlAttributes;

        /// <summary>
        /// Inner HTML
        /// </summary>
        private string InnerHtml;

        /// <summary>
        /// Initializes a new instance of the Button class.
        /// </summary>
        /// <param name="html">HTML Helper</param>
        /// <param name="innerHtml">Inner HTML</param>
        /// <param name="htmlAttributes">HTML Attributes</param>
        /// <param name="canRender">Can Render</param>
        public Button(HtmlHelper html, string innerHtml, object htmlAttributes, bool canRender)
        {
            HtmlHelper = html;
            InnerHtml = innerHtml;
            CanRender = canRender;
            HtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
        }

        /// <summary>
        /// Render Control
        /// </summary>
        public void Render()
        {
            var writer = HtmlHelper.ViewContext.Writer;
            using (var htmlTextWriter = new HtmlTextWriter(writer))
            {
                htmlTextWriter.Write(ToHtmlString());
            }
        }

        /// <summary>
        /// Implement Interface Member of IHtmlString
        /// </summary>
        /// <returns>HTML to Render Control</returns>
        public string ToHtmlString()
        {
            if (CanRender)
            {
                var buttonBuilder = new TagBuilder("button") { InnerHtml = InnerHtml };
                buttonBuilder.MergeAttributes(HtmlAttributes);

                return buttonBuilder.ToString();
            }

            return string.Empty;
        }
    }
}