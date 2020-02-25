namespace MDA.Core.Helpers
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;

    /// <summary>
    /// Static Action Link Helper Class
    /// </summary>
    public static class ActionLinkHelper
    {
        /// <summary>
        /// Renders a Action Link if User has the Access
        /// </summary>
        /// <param name="html">HTML Helper</param>
        /// <param name="link">Link</param>
        /// <param name="action">Action</param>
        /// <param name="controller">Controller</param>
        /// <param name="htmlAttributes">HTML Attributes</param>
        /// <param name="canRender">Can Render</param>
        /// <returns>HTML to Render Control</returns>
        public static ActionLink ActionLink(this HtmlHelper html, string link, string action, string controller, object htmlAttributes, bool canRender = true)
        {
            return new ActionLink(html, link, action, controller, htmlAttributes, canRender);
        }
    }

    /// <summary>
    /// Renders a Action Link if User has the Access
    /// </summary>
    public class ActionLink : IHtmlString
    {
        /// <summary>
        /// HTML Helper
        /// </summary>
        private readonly HtmlHelper HtmlHelper;

        /// <summary>
        /// Action
        /// </summary>
        private string Action;

        /// <summary>
        /// Can Render Control
        /// </summary>
        private bool CanRender;

        /// <summary>
        /// Controller
        /// </summary>
        private string Controller;

        /// <summary>
        /// HTML Attributes
        /// </summary>
        private IDictionary<string, object> HtmlAttributes;

        /// <summary>
        /// Link
        /// </summary>
        private string Link;

        /// <summary>
        /// Initializes a new instance of the ActionLink class.
        /// </summary>
        /// <param name="html">HTML Helper</param>
        /// <param name="link">Link</param>
        /// <param name="action">Action</param>
        /// <param name="controller">Controller</param>
        /// <param name="htmlAttributes">HTML Attributes</param>
        /// <param name="canRender">Can Render</param>
        public ActionLink(HtmlHelper html, string link, string action, string controller, object htmlAttributes, bool canRender)
        {
            HtmlHelper = html;
            Link = link;
            Action = action;
            Controller = controller;
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
                var anchorBuilder = new TagBuilder("a");
                anchorBuilder.MergeAttributes(HtmlAttributes);

                var urlHelper = new UrlHelper(HtmlHelper.ViewContext.RequestContext);
                var url = urlHelper.Action(Action, Controller);

                anchorBuilder.Attributes["href"] = url;
                anchorBuilder.SetInnerText(Link);

                return anchorBuilder.ToString();
            }

            return string.Empty;
        }
    }
}