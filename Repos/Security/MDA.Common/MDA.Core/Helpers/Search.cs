//-----------------------------------------------------------------------
// <copyright file="Search.cs" company="MDA Corporation">
//     Copyright (c) MDA Corporation. All rights reserved.
// </copyright>
// <author>Lionel Daniel</author>
//-----------------------------------------------------------------------

namespace MDA.Core.Helpers
{
    using Common.Resources;
    using Microsoft.Ajax.Utilities;
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;

    /// <summary>
    /// Static Search Helper Class
    /// </summary>
    public static class SearchHelper
    {
        /// <summary>
        /// Builds a Search Control
        /// </summary>
        /// <param name="html">HTML Helper</param>
        /// <param name="searchId">Search Id</param>
        /// <returns>HTML to Render Control</returns>
        public static Search Search(this HtmlHelper html, string searchId)
        {
            return new Search(html, searchId);
        }
    }

    /// <summary>
    /// Builds a Search Control
    /// </summary>
    public class Search : IHtmlString
    {
        private string m_form;
        private string m_grid;
        private HtmlHelper m_html;

        private string m_searchId;
        private bool m_showExport = true;
        private bool m_showSearch = true;
        private bool m_showUnSelectAll = true;

        /// <summary>
        /// Initializes a new instance of the Search class.
        /// </summary>
        /// <param name="html">HTML Helper</param>
        /// <param name="searchId">Search Id</param>
        public Search(HtmlHelper html, string searchId)
        {
            if (html == null)
            {
                throw new ArgumentNullException("html");
            }

            m_html = html;
            m_searchId = searchId;
        }

        /// <summary>
        /// Gets Search HTML
        /// 00 - Search Id
        /// 01 - Search Form
        /// </summary>
        protected string SearchHtml
        {
            get
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.Append(@"<div class=""padding-bottom-5"">");
                stringBuilder.Append(@"    <div class=""k-block"">");
                stringBuilder.Append(@"        <table style=""width: 100%"">");
                stringBuilder.Append(@"            <tr>");
                stringBuilder.Append(@"                <td>");

                if (m_showSearch)
                {
                    stringBuilder.Append(@"             <span class=""search-box"">");
                    stringBuilder.Append(@"                 <nobr>");
                    stringBuilder.Append(@"                     <input id=""searchText_{0}"" type=""text"" /><input id=""search_{0}"" type=""button"" />");
                    stringBuilder.Append(@"                 </nobr>");
                    stringBuilder.Append(@"             </span>");
                }

                stringBuilder.Append(@"                </td>");
                stringBuilder.Append(@"                <td style=""text-align: right;"">");

                if (m_showUnSelectAll)
                {
                    stringBuilder.Append(@"                    <button id=""unSelectAll_{0}"" class=""unselect-all-button"" type=""button"">{2}</button>&nbsp;");
                }

                if (m_showExport)
                {
                    stringBuilder.Append(@"<button id=""xlsExport"" value=""Export to XLS"" class=""xls-button"" name=""{1}"">{3}</button>&nbsp;");
                    stringBuilder.Append(@"<button id=""pdfExport"" value=""Export to PDF"" class=""pdf-button"" name=""{1}"">{4}</button>");
                }

                stringBuilder.Append(@"                </td>");
                stringBuilder.Append(@"            </tr>");
                stringBuilder.Append(@"        </table>");
                stringBuilder.Append(@"    </div>");
                stringBuilder.Append(@"</div>");

                return string.Format(stringBuilder.ToString(), m_searchId, m_form, Resource.Text_UnSelect_All, Resource.Text_Export_To_Xls, Resource.Text_Export_To_Pdf);
            }
        }

        /// <summary>
        /// 00 - Search Id
        /// 01 - Grid Id
        /// </summary>
        protected string SearchScript
        {
            get
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.Append(@"    $(function () {{");
                stringBuilder.Append(@"        var grid = $(""#{1}"");");
                stringBuilder.Append(@"        var dataGrid = grid.data().kendoGrid;");

                stringBuilder.Append(@"        var search = $(""#searchText_{0}"");");
                if (m_showSearch)
                {
                    stringBuilder.Append(@"        $(""#search_{0}"").click(function () {{");
                    stringBuilder.Append(@"            var gridColumns = [];");

                    stringBuilder.Append(@"            $.each(dataGrid.columns, function () {{");
                    stringBuilder.Append(@"                if (this.field != null && this.hidden != true && this.skipSearch != true) {{");
                    stringBuilder.Append(@"                    gridColumns.push(this.field);");
                    stringBuilder.Append(@"                }}");
                    stringBuilder.Append(@"            }});");

                    stringBuilder.Append(@"            dataGrid.dataSource.filter(getFilter($.trim(search.val()), gridColumns));");
                    stringBuilder.Append(@"            dataGrid.dataSource.page(1);");
                    stringBuilder.Append(@"            dataGrid.dataSource.read();");

                    stringBuilder.Append(@"            grid.prop(""selectedRows"", """");");
                    stringBuilder.Append(@"            $(""#chBxSelectAll, #chBxSelectRow"", grid).prop(""checked"", false);");
                    stringBuilder.Append(@"        }});");

                    stringBuilder.Append(@"        $(""#searchText_{0}"").on(""input"", function (e) {{");
                    stringBuilder.Append(@"            if (!this.value) {{");
                    stringBuilder.Append(@"            dataGrid.dataSource.filter(null);");

                    stringBuilder.Append(@"            $(""form.k-filter-menu button[type='reset']"").trigger(""click"");");
                    stringBuilder.Append(@"            dataGrid.dataSource.read();");

                    stringBuilder.Append(@"            search.val("""");");
                    stringBuilder.Append(@"            }}");
                    stringBuilder.Append(@"        }});");

                    stringBuilder.Append(@"        $(""#searchText_{0}"").keypress(function (e) {{");
                    stringBuilder.Append(@"            if (e.keyCode == 13) {{");
                    stringBuilder.Append(@"                e.preventDefault();");
                    stringBuilder.Append(@"                $(""#search_{0}"").trigger(""click"");");
                    stringBuilder.Append(@"            }}");
                    stringBuilder.Append(@"        }});");

                    stringBuilder.Append(@"        function getFilter(searchText, columns) {{");
                    stringBuilder.Append(@"            if (searchText == """") {{ return null; }}");

                    stringBuilder.Append(@"            var columnFilter = [];");
                    stringBuilder.Append(@"            $.each(columns, function () {{");
                    stringBuilder.Append(@"                columnFilter.push({{ ""field"": this, ""operator"": ""contains"", ""value"": searchText }});");
                    stringBuilder.Append(@"            }});");

                    stringBuilder.Append(@"            return {{ ""filters"": columnFilter, ""logic"": ""or"" }};");
                    stringBuilder.Append(@"        }}");
                }

                if (m_showUnSelectAll)
                {
                    stringBuilder.Append(@"        $(""#unSelectAll_{0}"").click(function () {{");
                    stringBuilder.Append(@"            grid.prop(""selectedRows"", """");");
                    stringBuilder.Append(@"            $(""#chBxSelectAll, #chBxSelectRow"", grid).prop(""checked"", false);");
                    stringBuilder.Append(@"            $("".gridCheckBox"", grid).css(""visibility"", ""hidden"");");
                    stringBuilder.Append(@"        }});");
                }

                if (m_showExport)
                {
                    stringBuilder.Append(@"        $(""#xlsExport,#pdfExport"").click(function () {{");
                    stringBuilder.Append(@"            var gridColumns = [];");
                    stringBuilder.Append(@"            var gridColumnHeaders = [];");

                    stringBuilder.Append(@"            $.each(dataGrid.columns, function () {{");
                    stringBuilder.Append(@"                if (this.hidden != true && this.field != null && this.title != "" "") {{");
                    stringBuilder.Append(@"                    gridColumns.push(this.field);");
                    stringBuilder.Append(@"                    gridColumnHeaders.push(this.title);");
                    stringBuilder.Append(@"                }}");
                    stringBuilder.Append(@"            }});");

                    stringBuilder.Append(@"            $.cookie(""VISIBLEGRIDCOLUMNS"", JSON.stringify(gridColumns), {{ path: ""/"" }});");
                    stringBuilder.Append(@"            $.cookie(""VISIBLEGRIDCOLUMNHEADERS"", JSON.stringify(gridColumnHeaders), {{ path: ""/"" }});");
                    stringBuilder.Append(@"        }});");
                }

                stringBuilder.Append(@"    }});");

                var minifier = new Minifier();

                var searchScript = string.Format(stringBuilder.ToString(), m_searchId, m_grid);
                return string.Format("<script>{0}</script>", minifier.MinifyJavaScript(searchScript));
            }
        }

        /// <summary>
        /// Sets Action Function Name
        /// </summary>
        /// <param name="form">Form Name</param>
        /// <returns>Search Object</returns>
        public Search Form(string form)
        {
            if (form == null)
            {
                throw new ArgumentNullException("form");
            }

            m_form = form;
            return this;
        }

        /// <summary>
        /// Sets Grid to Filter
        /// </summary>
        /// <param name="grid">Grid to Filter</param>
        /// <returns>Search Object</returns>
        public Search Grid(string grid)
        {
            if (grid == null)
            {
                throw new ArgumentNullException("grid");
            }

            m_grid = grid;
            return this;
        }

        /// <summary>
        /// Render Control
        /// </summary>
        public void Render()
        {
            var writer = m_html.ViewContext.Writer;
            using (var htmlTextWriter = new HtmlTextWriter(writer))
            {
                htmlTextWriter.Write(ToHtmlString());
            }
        }

        /// <summary>
        /// Sets Show Export
        /// </summary>
        /// <param name="showExport">Show Export</param>
        /// <returns>Search Object</returns>
        public Search ShowExport(bool showExport)
        {
            m_showExport = showExport;
            return this;
        }

        /// <summary>
        /// Sets Show Search
        /// </summary>
        /// <param name="showSearch">Show Search</param>
        /// <returns>Search Object</returns>
        public Search ShowSearch(bool showSearch)
        {
            m_showSearch = showSearch;
            return this;
        }

        /// <summary>
        /// Sets Show UnSelect All
        /// </summary>
        /// <param name="showUnSelectAll">Show UnSelect All</param>
        /// <returns>Search Object</returns>
        public Search ShowUnSelectAll(bool showUnSelectAll)
        {
            m_showUnSelectAll = showUnSelectAll;
            return this;
        }

        /// <summary>
        /// Implement Interface Member of IHtmlString
        /// </summary>
        /// <returns>HTML to Render Control</returns>
        public string ToHtmlString()
        {
            return string.Format("{0}{1}{2}", SearchScript, Environment.NewLine, SearchHtml);
        }
    }
}