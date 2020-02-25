namespace MDA.Core.Helpers
{
    using Common.Resources;
    using Microsoft.Ajax.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;

    public static class SelectListBoxHelper
    {
        /// <summary>
        /// Builds a SelectListBox Control
        /// </summary>
        /// <param name="html">HTML Helper</param>
        /// <param name="name">SelectListBox Name</param>
        /// <param name="availableList">Available List</param>
        /// <param name="selectedList">Selected List</param>
        /// <param name="htmlAttributes">HTML Attributes</param>
        /// <returns>SelectListBox Control</returns>
        public static SelectListBox SelectListBox(this HtmlHelper html, string name, IList<string> availableList, IList<string> selectedList, object htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            return new SelectListBox(html, name, availableList, selectedList, htmlAttributes);
        }
    }

    /// <summary>
    /// Builds a SelectListBox Control
    /// </summary>
    public class SelectListBox : IHtmlString
    {
        /// <summary>
        /// HTML Helper
        /// </summary>
        private readonly HtmlHelper HtmlHelper;

        /// <summary>
        /// Available List
        /// </summary>
        private IList<string> AvailableList;

        /// <summary>
        /// HTML Attributes
        /// </summary>
        private IDictionary<string, object> HtmlAttributes;

        /// <summary>
        /// Name
        /// </summary>
        private string Name;

        /// <summary>
        /// Selected List
        /// </summary>
        private IList<string> SelectedList;

        /// <summary>
        /// Initializes a new instance of the SelectListBox class.
        /// </summary>
        /// <param name="html">HTML Helper</param>
        /// <param name="name">SelectListBox Name</param>
        /// <param name="availableList">Available List</param>
        /// <param name="selectedList">Selected List</param>
        /// <param name="htmlAttributes">HTML Attributes</param>
        public SelectListBox(HtmlHelper html, string name, IList<string> availableList, IList<string> selectedList, object htmlAttributes)
        {
            HtmlHelper = html;
            Name = name;
            AvailableList = availableList;
            SelectedList = selectedList;

            HtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
        }

        /// <summary>
        /// 00 - List Box Name
        /// </summary>
        protected string ListBoxScript
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.Append(@"    $(function () {{");
                stringBuilder.Append(@"        setButtonStatus_{0}();");

                stringBuilder.Append(@"        $(""#lstBxSelected{0},#lstBxAvailable{0}"").change(function () {{");
                stringBuilder.Append(@"            setButtonStatus_{0}();");
                stringBuilder.Append(@"        }});");

                stringBuilder.Append(@"        $(""#btnAddSelected_{0}"").click(function () {{");
                stringBuilder.Append(@"            $(""#lstBxAvailable{0} > option:selected"").each(function () {{");
                stringBuilder.Append(@"                $(this).remove().appendTo(""#lstBxSelected{0}"");");
                stringBuilder.Append(@"            }});");

                stringBuilder.Append(@"            setSelectedFields_{0}();");
                stringBuilder.Append(@"            setButtonStatus_{0}();");
                stringBuilder.Append(@"        }});");

                stringBuilder.Append(@"        $(""#btnRemoveSelected_{0}"").click(function () {{");
                stringBuilder.Append(@"            $(""#lstBxSelected{0} > option:selected"").each(function () {{");
                stringBuilder.Append(@"                $(this).remove().appendTo(""#lstBxAvailable{0}"");");
                stringBuilder.Append(@"            }});");

                stringBuilder.Append(@"            setSelectedFields_{0}();");
                stringBuilder.Append(@"            setButtonStatus_{0}();");
                stringBuilder.Append(@"        }});");

                stringBuilder.Append(@"        $(""#btnAddAll_{0}"").click(function () {{");
                stringBuilder.Append(@"            $(""#lstBxAvailable{0} > option"").appendTo(""#lstBxSelected{0}"");");

                stringBuilder.Append(@"            setSelectedFields_{0}();");
                stringBuilder.Append(@"            setButtonStatus_{0}();");
                stringBuilder.Append(@"        }});");

                stringBuilder.Append(@"        $(""#btnRemoveAll_{0}"").click(function () {{");
                stringBuilder.Append(@"            $(""#lstBxSelected{0} > option"").appendTo(""#lstBxAvailable{0}"");");

                stringBuilder.Append(@"            setSelectedFields_{0}();");
                stringBuilder.Append(@"            setButtonStatus_{0}();");
                stringBuilder.Append(@"        }});");

                stringBuilder.Append(@"        $(""#btnMoveUp_{0}"").click(function () {{");
                stringBuilder.Append(@"            $(""#lstBxSelected{0} option:selected"").each(function (i, selected) {{");
                stringBuilder.Append(@"                $(this).insertBefore($(this).prev());");
                stringBuilder.Append(@"            }});");

                stringBuilder.Append(@"            setSelectedFields_{0}();");
                stringBuilder.Append(@"            setButtonStatus_{0}();");
                stringBuilder.Append(@"        }});");

                stringBuilder.Append(@"        $(""#btnMoveDown_{0}"").click(function () {{");
                stringBuilder.Append(@"            $($(""#lstBxSelected{0} option:selected"").get().reverse()).each(function (i, selected) {{");
                stringBuilder.Append(@"                $(this).insertAfter($(this).next());");
                stringBuilder.Append(@"            }});");

                stringBuilder.Append(@"            setSelectedFields_{0}();");
                stringBuilder.Append(@"            setButtonStatus_{0}();");
                stringBuilder.Append(@"        }});");

                stringBuilder.Append(@"        function setSelectedFields_{0}() {{");
                stringBuilder.Append(@"            var listBoxOptions = [];");
                stringBuilder.Append(@"            $(""#lstBxSelected{0} > option"").each(function () {{");
                stringBuilder.Append(@"                listBoxOptions.push($(this).val());");
                stringBuilder.Append(@"            }});");

                stringBuilder.Append(@"            $(""#hidListBoxSelected{0}"").val(listBoxOptions.toString());");
                stringBuilder.Append(@"        }}");

                stringBuilder.Append(@"        function setButtonStatus_{0}() {{");
                stringBuilder.Append(@"            var availableListExists = $(""#lstBxAvailable{0} > option"").length == 0;");

                stringBuilder.Append(@"            $(""#btnAddAll_{0}"").prop(""disabled"", availableListExists);");
                stringBuilder.Append(@"            $(""#btnAddSelected_{0}"").prop(""disabled"", availableListExists || $(""#lstBxAvailable{0} > option:selected"").length == 0);");

                stringBuilder.Append(@"            var selectedListLength = $(""#lstBxSelected{0} > option"").length;");
                stringBuilder.Append(@"            var selectedIndex = $(""#lstBxSelected{0}"")[0].selectedIndex;");
                stringBuilder.Append(@"            var isSelected = $(""#lstBxSelected{0} > option:selected"").length == 0;");

                stringBuilder.Append(@"            $(""#btnRemoveSelected_{0}"").prop(""disabled"", selectedListLength == 0 || isSelected);");
                stringBuilder.Append(@"            $(""#btnRemoveAll_{0}"").prop(""disabled"", selectedListLength == 0);");
                stringBuilder.Append(@"            $(""#btnMoveUp_{0}"").prop(""disabled"", selectedListLength <= 1 || selectedIndex == 0 || isSelected);");
                stringBuilder.Append(@"            $(""#btnMoveDown_{0}"").prop(""disabled"", selectedListLength <= 1 || selectedIndex == selectedListLength - 1 || isSelected);");
                stringBuilder.Append(@"        }}");
                stringBuilder.Append(@"    }});");

                var minifier = new Minifier();

                var listBoxScript = string.Format(stringBuilder.ToString(), Name);
                return string.Format("<script>{0}</script>", minifier.MinifyJavaScript(listBoxScript));
            }
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
            tableBuilder.Attributes["style"] = "width: 100%; border-spacing: 0; border-collapse: collapse;";

            StringBuilder sb = new StringBuilder();
            sb.Append(tableBuilder.ToString(TagRenderMode.StartTag));

            var trBuilder = new TagBuilder("tr");
            var tdBuilder = new TagBuilder("td");

            sb.Append(trBuilder.ToString(TagRenderMode.StartTag));

            tdBuilder.Attributes["style"] = "width: 50%;";
            tdBuilder.Attributes["class"] = "form-label";

            // Available Fields Label
            sb.Append(tdBuilder.ToString(TagRenderMode.StartTag));
            sb.Append(Resource.Text_Available_Fields);
            sb.Append(tdBuilder.ToString(TagRenderMode.EndTag));

            // Selected Fields Label
            sb.Append(tdBuilder.ToString(TagRenderMode.StartTag));
            sb.Append(Resource.Text_Selected_Fields);
            sb.Append(tdBuilder.ToString(TagRenderMode.EndTag));

            sb.Append(trBuilder.ToString(TagRenderMode.EndTag));

            // Available Fields ListBox
            var availableListBoxBuilder = new TagBuilder("select");

            availableListBoxBuilder.GenerateId(string.Format("lstBxAvailable{0}", Name));
            availableListBoxBuilder.MergeAttribute("multiple", "multiple");
            availableListBoxBuilder.MergeAttributes(HtmlAttributes);
            availableListBoxBuilder.MergeAttribute("name", string.Format("lstBxAvailable{0}", Name));

            if (AvailableList != null)
            {
                foreach (var item in AvailableList)
                {
                    var optionBuilder = new TagBuilder("option");
                    optionBuilder.MergeAttribute("value", item);

                    optionBuilder.InnerHtml = item;
                    availableListBoxBuilder.InnerHtml += optionBuilder.ToString();
                }
            }

            sb.Append(trBuilder.ToString(TagRenderMode.StartTag));

            sb.Append(tdBuilder.ToString(TagRenderMode.StartTag));
            sb.Append(availableListBoxBuilder.ToString(TagRenderMode.Normal));
            sb.Append(tdBuilder.ToString(TagRenderMode.EndTag));

            // Selected Fields ListBox
            var selectedListBoxBuilder = new TagBuilder("select");

            selectedListBoxBuilder.GenerateId(string.Format("lstBxSelected{0}", Name));
            selectedListBoxBuilder.MergeAttribute("multiple", "multiple");
            selectedListBoxBuilder.MergeAttributes(HtmlAttributes);
            selectedListBoxBuilder.MergeAttribute("name", string.Format("lstBxSelected{0}", Name));

            if (SelectedList != null)
            {
                foreach (var item in SelectedList)
                {
                    var optionBuilder = new TagBuilder("option");
                    optionBuilder.MergeAttribute("value", item);

                    optionBuilder.InnerHtml = item;
                    selectedListBoxBuilder.InnerHtml += optionBuilder.ToString();
                }
            }

            sb.Append(tdBuilder.ToString(TagRenderMode.StartTag));
            sb.Append(selectedListBoxBuilder.ToString(TagRenderMode.Normal));
            sb.Append(tdBuilder.ToString(TagRenderMode.EndTag));

            sb.Append(trBuilder.ToString(TagRenderMode.EndTag));
            sb.Append(trBuilder.ToString(TagRenderMode.StartTag));

            // Available ListBox Buttons
            sb.Append(tdBuilder.ToString(TagRenderMode.StartTag));

            sb.Append(string.Format(@"<button id=""btnAddAll_{0}"" type=""button"" class=""add-all-button"" title=""{1}"" name=""{0}""/>", Name, Resource.Text_Add_All));
            sb.Append(string.Format(@"<button id=""btnAddSelected_{0}"" type=""button"" class=""add-selected-button"" title=""{1}"" name=""{0}""/>", Name, Resource.Text_Add_Selected));

            sb.Append(tdBuilder.ToString(TagRenderMode.EndTag));

            // Selected ListBox Buttons
            sb.Append(tdBuilder.ToString(TagRenderMode.StartTag));

            sb.Append(string.Format(@"<button id=""btnRemoveAll_{0}"" type=""button"" class=""remove-all-button"" title=""{1}"" name=""{0}""/>", Name, Resource.Text_Remove_All));
            sb.Append(string.Format(@"<button id=""btnRemoveSelected_{0}"" type=""button"" class=""remove-selected-button"" title=""{1}"" name=""{0}""/>", Name, Resource.Text_Remove_Selected));
            sb.Append(string.Format(@"<button id=""btnMoveUp_{0}"" type=""button"" class=""move-up-button"" title=""{1}"" name=""{0}""/>", Name, Resource.Text_Move_Up));
            sb.Append(string.Format(@"<button id=""btnMoveDown_{0}"" type=""button"" class=""move-down-button"" title=""{1}"" name=""{0}""/>", Name, Resource.Text_Move_Down));

            sb.Append(tdBuilder.ToString(TagRenderMode.EndTag));
            sb.Append(trBuilder.ToString(TagRenderMode.EndTag));

            sb.Append(tableBuilder.ToString(TagRenderMode.EndTag));

            // Hidden Field for Selected Values
            var hiddenBuilder = new TagBuilder("input");

            hiddenBuilder.MergeAttribute("id", string.Format("hidListBoxSelected{0}", Name));
            hiddenBuilder.MergeAttribute("name", string.Format("hidListBoxSelected{0}", Name));
            hiddenBuilder.MergeAttribute("type", "hidden");
            hiddenBuilder.MergeAttribute("value", SelectedList != null ? string.Join(",", SelectedList) : null);

            sb.Append(hiddenBuilder.ToString(TagRenderMode.SelfClosing));

            sb.Append("<br/>");
            sb.Append(ListBoxScript);

            return sb.ToString();
        }
    }
}