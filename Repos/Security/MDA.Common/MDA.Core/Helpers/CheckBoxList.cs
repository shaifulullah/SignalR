namespace MDA.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;

    /// <summary>
    /// Static CheckBoxList Helper Class
    /// </summary>
    public static class CheckBoxListHelper
    {
        /// <summary>
        /// Builds a CheckBoxList Control
        /// </summary>
        /// <param name="html">HTML Helper</param>
        /// <param name="name">CheckBoxList Name</param>
        /// <param name="itemList">Item List</param>
        /// <param name="selectedIds">Selected Id's</param>
        /// <param name="columns">Number of Columns</param>
        /// <param name="htmlAttributes">HTML Attributes</param>
        /// <returns>CheckBoxList Control</returns>
        public static CheckBoxList CheckBoxList(this HtmlHelper html, string name, IList<SelectListItem> itemList, IList<string> selectedIds = null, int columns = 3, object htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            return new CheckBoxList(html, name, itemList, selectedIds, columns, htmlAttributes);
        }
    }

    /// <summary>
    /// Builds a CheckBoxList Control
    /// </summary>
    public class CheckBoxList : IHtmlString
    {
        /// <summary>
        /// HTML Helper
        /// </summary>
        private readonly HtmlHelper HtmlHelper;

        /// <summary>
        /// Number of Columns
        /// </summary>
        private int Columns;

        /// <summary>
        /// HTML Attributes
        /// </summary>
        private IDictionary<string, object> HtmlAttributes;

        /// <summary>
        /// Item List
        /// </summary>
        private IList<SelectListItem> ItemList;

        /// <summary>
        /// CheckBoxList Name
        /// </summary>
        private string Name;

        /// <summary>
        /// Selected Id's
        /// </summary>
        private IList<string> SelectedIds;

        /// <summary>
        /// Initializes a new instance of the CheckBoxList class.
        /// </summary>
        /// <param name="html">HTML Helper</param>
        /// <param name="name">CheckBoxList Name</param>
        /// <param name="itemList">Item List</param>
        /// <param name="selectedIds">Selected Id's</param>
        /// <param name="columns">Number of Columns</param>
        /// <param name="htmlAttributes">HTML Attributes</param>
        public CheckBoxList(HtmlHelper html, string name, IList<SelectListItem> itemList, IList<string> selectedIds, int columns, object htmlAttributes)
        {
            HtmlHelper = html;
            Name = name;
            ItemList = itemList;
            SelectedIds = selectedIds;
            Columns = columns;

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
            var tableBuilder = new TagBuilder("table");
            tableBuilder.Attributes["style"] = "width: 100%; border-spacing: 0; border-collapse: separate;";

            StringBuilder sb = new StringBuilder();
            sb.Append(tableBuilder.ToString(TagRenderMode.StartTag));

            int i = 0;
            foreach (var item in ItemList)
            {
                if (i++ % Columns == 0)
                {
                    var trBuilder = new TagBuilder("tr");
                    sb.Append(i == 0 ? trBuilder.ToString(TagRenderMode.StartTag) : trBuilder.ToString(TagRenderMode.EndTag));
                }

                // Build the CheckBox
                var checkBoxBuilder = new TagBuilder("input");

                checkBoxBuilder.MergeAttribute("type", "checkbox");
                checkBoxBuilder.MergeAttribute("value", item.Value);
                checkBoxBuilder.MergeAttribute("name", Name);
                checkBoxBuilder.MergeAttribute("data_field", item.Text);

                checkBoxBuilder.GenerateId(string.Format("chBx{0}{1}", Name, item.Value));

                // Check the CheckBox if Selected
                if (SelectedIds != null && SelectedIds.Contains(item.Value))
                {
                    checkBoxBuilder.MergeAttribute("checked", "checked");
                }

                // Merge HTML Attributes
                checkBoxBuilder.MergeAttributes(HtmlAttributes);

                // CheckBox Label
                var labelBuilder = new TagBuilder("label");

                labelBuilder.MergeAttribute("for", checkBoxBuilder.Attributes["id"]);
                labelBuilder.InnerHtml = item.Text;

                // Add CheckBox and Label to Table
                var tdBuilder = new TagBuilder("td");
                sb.Append(tdBuilder.ToString(TagRenderMode.StartTag));

                sb.Append(checkBoxBuilder.ToString(TagRenderMode.Normal));
                sb.Append(labelBuilder.ToString(TagRenderMode.Normal));

                sb.Append(tdBuilder.ToString(TagRenderMode.EndTag));
            }

            sb.Append(tableBuilder.ToString(TagRenderMode.EndTag));
            sb.Append("<br/>");

            return sb.ToString();
        }
    }
}