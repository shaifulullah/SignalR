//-----------------------------------------------------------------------
// <copyright file="SearchBox.cs" company="MDA Corporation">
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
    /// Static SearchBox Helper Class
    /// </summary>
    public static class SearchBoxHelper
    {
        /// <summary>
        /// Builds a Search Control
        /// </summary>
        /// <param name="html">HTML Helper</param>
        /// <param name="buttonId">Button Id</param>
        /// <returns>HTML to Render Control</returns>
        public static SearchBox SearchBox(this HtmlHelper html, string buttonId)
        {
            return new SearchBox(html, buttonId);
        }
    }

    /// <summary>
    /// Builds a Search Control
    /// </summary>
    public class SearchBox : IHtmlString
    {
        private readonly HtmlHelper m_html;

        private string m_action;
        private string m_bindData;
        private string m_buttonId;
        private string[] m_columns;
        private string m_controlId;
        private string m_controller;
        private string m_filter;
        private string m_idColumn = "Id";
        private bool m_isModal = true;
        private bool m_multiSelect = true;
        private string m_operator;
        private string m_returnValue = "Id";
        private string m_sort;
        private string m_sortDirection;
        private string m_title;
        private string m_value;
        private int m_width = 900;

        /// <summary>
        /// Initializes a new instance of the SearchBox class.
        /// </summary>
        /// <param name="html">HTML Helper</param>
        /// <param name="buttonId">Button Id</param>
        public SearchBox(HtmlHelper html, string buttonId)
        {
            if (html == null)
            {
                throw new ArgumentNullException("html");
            }

            m_html = html;
            m_buttonId = buttonId;
        }

        /// <summary>
        /// Gets Search Box Html
        /// </summary>
        public string SearchBoxHtml
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append(@"<div id=""window_{0}"">");
                sb.Append(@"    <div class=""padding-bottom-5"">");
                sb.Append(@"        <input type=""submit"" class=""select-button"" value=""{1}"" id=""imgBtSelect_{0}"" />");
                sb.Append(@"        <input type=""reset"" class=""cancel-button"" value=""{2}"" id=""imgBtCancel_{0}"" />");
                sb.Append(@"    </div>");

                sb.Append(@"    <div class=""padding-bottom-5"">");
                sb.Append(@"        <div class=""k-block"">");
                sb.Append(@"            <table style=""width: 100%"">");
                sb.Append(@"                <tr>");
                sb.Append(@"                    <td>");
                sb.Append(@"                        <span class=""search-box"">");
                sb.Append(@"                            <nobr>");
                sb.Append(@"                                <input id=""searchText_{0}"" type=""text"" /><input id=""search_{0}"" type=""button"" />");
                sb.Append(@"                            </nobr>");
                sb.Append(@"                        </span>");
                sb.Append(@"                    </td>");
                sb.Append(@"                </tr>");
                sb.Append(@"            </table>");
                sb.Append(@"        </div>");
                sb.Append(@"    </div>");

                sb.Append(@"    <div id=""grid_{0}""></div>");
                sb.Append(@"</div>");

                return string.Format(sb.ToString(), m_buttonId, Resource.Text_Select, Resource.Text_Cancel);
            }
        }

        /// <summary>
        /// Gets Fields to Display in the Grid
        /// </summary>
        protected string GridFields
        {
            get
            {
                var gridFieldList = string.Empty;

                var gridFieldTemplate = @"{0} field: ""{2}"", title: ""{3}"", hidden: {4}, width: ""{5}%"", encoded: {6} {1},";
                var gridFieldSkipSearchTemplate = @"{0} field: ""{2}"", title: ""{3}"", hidden: {4}, width: ""{5}%"", encoded: {6}, filterable: {7}, sortable: {8}, groupable: {9}, skipSearch: {10} {1},";
                var gridFieldCheckBoxTemplate = @"{0} field: ""{2}"", title: ""{3}"", hidden: {4}, width: ""{5}%"", encoded: {6}, template: '<input disabled=""disabled"" type=""checkbox"" #={2}?""checked=checked"":""""# />'{1},";

                foreach (var gridField in m_columns)
                {
                    var columnAttributes = Array.ConvertAll(gridField.Split(':'), x => x.Trim());
                    var encodedValue = columnAttributes.Length > 5 ? "false" : "true";

                    if (columnAttributes[2] == "boolean")
                    {
                        gridFieldList += string.Format(gridFieldCheckBoxTemplate, "{", "}", columnAttributes[0], columnAttributes[1], columnAttributes[3], columnAttributes[4], encodedValue);
                    }
                    else if (columnAttributes.Length > 8)
                    {
                        gridFieldList += string.Format(gridFieldSkipSearchTemplate, "{", "}", columnAttributes[0], columnAttributes[1], columnAttributes[3], columnAttributes[4], encodedValue, columnAttributes[5], columnAttributes[6], columnAttributes[7], columnAttributes[8]);
                    }
                    else
                    {
                        gridFieldList += string.Format(gridFieldTemplate, "{", "}", columnAttributes[0], columnAttributes[1], columnAttributes[3], columnAttributes[4], encodedValue);
                    }
                }

                return gridFieldList.TrimEnd(',');
            }
        }

        /// <summary>
        /// Gets Fields from the Model
        /// </summary>
        protected string ModelFields
        {
            get
            {
                var modelFieldList = string.Empty;
                var modelFieldTemplate = @"""{2}"": {0} type: ""{3}"" {1},";

                foreach (var modelField in m_columns)
                {
                    var columnAttributes = Array.ConvertAll(modelField.Split(':'), x => x.Trim());
                    modelFieldList += string.Format(modelFieldTemplate, "{", "}", columnAttributes[0], columnAttributes[2]);
                }

                return modelFieldList.TrimEnd(',');
            }
        }

        /// <summary>
        /// Gets Search Box Script
        /// </summary>
        protected string SearchBoxScript
        {
            get
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.Append(@"$(function () {{");
                stringBuilder.Append(@"    var window = $(""#window_{0}"").kendoWindow({{");
                stringBuilder.Append(@"        modal: {2},");
                stringBuilder.Append(@"        open: function () {{");
                stringBuilder.Append(@"           this.wrapper.css({{ top: 10 }});");
                stringBuilder.Append(@"        }},");
                stringBuilder.Append(@"        close: function () {{");
                stringBuilder.Append(@"           $(""#grid_{0}"").data().kendoGrid.dataSource.filter(null);");
                stringBuilder.Append(@"           $(""#searchText_{0}"").val("""");");
                stringBuilder.Append(@"        }},");
                stringBuilder.Append(@"        visible: false,");
                stringBuilder.Append(@"        width: ""{3}px""");
                stringBuilder.Append(@"    }}).data().kendoWindow;");

                stringBuilder.Append(@"    var grid = $(""#grid_{0}"").kendoGrid({{");
                stringBuilder.Append(@"       columns: [{{");
                stringBuilder.Append(@"            headerTemplate: ""<input id='chBxSelectAll_{0}' type='checkbox' />"",");
                stringBuilder.Append(@"            template: ""<input id='chBxSelectRow_{0}' class='gridCheckBox' type='checkbox' />"",");
                stringBuilder.Append(@"            filterable: false, sortable: false, groupable: false, width: '4%'");
                stringBuilder.Append(@"            }},");
                stringBuilder.Append(@"            {4}");
                stringBuilder.Append(@"        ],");
                stringBuilder.Append(@"        pageable: {{input: false, numeric: true, refresh: true, pageSizes: [15, 20, 30, 40, 50]}},");
                stringBuilder.Append(@"        groupable: true,");
                stringBuilder.Append(@"        sortable: {{ allowUnsort: false }},");
                stringBuilder.Append(@"        filterable: true,");
                stringBuilder.Append(@"        resizable: true,");
                stringBuilder.Append(@"        autoBind: false,");
                stringBuilder.Append(@"        dataBound: onDataBoundWindow,");
                stringBuilder.Append(@"        dataSource: {{");
                stringBuilder.Append(@"            serverPaging: true,");
                stringBuilder.Append(@"            serverFiltering: true,");
                stringBuilder.Append(@"            serverSorting: true,");
                stringBuilder.Append(@"            pageSize: 15,");
                stringBuilder.Append(@"            sort: {{ field: ""{5}"", dir: ""{6}"" }},");
                stringBuilder.Append(@"            {7}");
                stringBuilder.Append(@"            schema: {{");
                stringBuilder.Append(@"                data: ""Data"", total: ""Total"",");
                stringBuilder.Append(@"                model: {{ id: ""{8}"", fields: {{ {9} }} }}");
                stringBuilder.Append(@"            }},");
                stringBuilder.Append(@"            transport: {{");
                stringBuilder.Append(@"                read: {{ url: ""{10}/{11}"", contentType: ""application/json; charset=utf-8"", type: ""POST"", dataType: ""json"" }},");
                stringBuilder.Append(@"                parameterMap: function (data) {{ return JSON.stringify(data); }}");
                stringBuilder.Append(@"            }}");
                stringBuilder.Append(@"        }}");
                stringBuilder.Append(@"}});");

                stringBuilder.Append(@"    var dataGrid = grid.data().kendoGrid;");
                stringBuilder.Append(@"    var search = $(""#searchText_{0}"");");

                stringBuilder.Append(@"    var checkBoxSelectRow = ""#chBxSelectRow_{0}"";");
                stringBuilder.Append(@"    var checkBoxSelectAll = ""#chBxSelectAll_{0}"";");

                stringBuilder.Append(@"    var returnValues = [];");

                stringBuilder.Append(@"    $(""body"").on(""click"", ""#{0}"", function (e) {{");
                stringBuilder.Append(@"        returnValues = [];");

                stringBuilder.Append(@"        window.center().open();");
                stringBuilder.Append(@"        $(""#window_{0}"").parent().find("".k-window-title"").text(""{1}"");");

                stringBuilder.Append(@"        grid.prop(""selectedRows"", null);");
                stringBuilder.Append(@"        dataGrid.dataSource.read();");
                stringBuilder.Append(@"    }});");

                stringBuilder.Append(@"    $(document).on(""click"", ""#imgBtSelect_{0}"", function () {{");
                if (!string.IsNullOrEmpty(m_controlId))
                {
                    stringBuilder.Append(@"        $(""#{12}"").val(returnValues);");
                }
                stringBuilder.Append(@"        {13}(returnValues.toString());");
                stringBuilder.Append(@"        window.close();");
                stringBuilder.Append(@"    }});");

                stringBuilder.Append(@"    $(document).on(""click"", ""#imgBtCancel_{0}"", function () {{");
                stringBuilder.Append(@"        returnValues = [];");
                if (!string.IsNullOrEmpty(m_controlId))
                {
                    stringBuilder.Append(@"        $(""#{12}"").val('');");
                }
                stringBuilder.Append(@"        {13}('');");
                stringBuilder.Append(@"        window.close();");
                stringBuilder.Append(@"    }});");

                stringBuilder.Append(@"    $(""#grid_{0}"").on(""dblclick"", ""tr"", function () {{");
                stringBuilder.Append(@"        var row = $(this).closest(""tr"");");
                stringBuilder.Append(@"        var dataItem = dataGrid.dataItem(row);");
                stringBuilder.Append(@"        if (dataItem != undefined) {{");
                if (!string.IsNullOrEmpty(m_controlId))
                {
                    stringBuilder.Append(@"             $(""#{12}"").val(dataItem[""{15}""].toString());");
                }
                stringBuilder.Append(@"             {13}(dataItem[""{15}""].toString());");
                stringBuilder.Append(@"         }}");
                stringBuilder.Append(@"        window.close();");
                stringBuilder.Append(@"    }});");

                stringBuilder.Append(@"    $(document).on(""click"", checkBoxSelectRow, function (e) {{");
                stringBuilder.Append(@"        setCheckBoxState(e.target);");
                stringBuilder.Append(@"        $(checkBoxSelectAll, grid).prop(""checked"", (grid.find(""input:checkbox.gridCheckBox:not(:checked)"").length == 0));");
                stringBuilder.Append(@"    }});");

                stringBuilder.Append(@"    $(document).on(""click"", checkBoxSelectAll, function (e) {{");
                stringBuilder.Append(@"        if ({14}) {{");
                stringBuilder.Append(@"            grid.find(""input:checkbox.gridCheckBox"").each(function () {{");
                stringBuilder.Append(@"                this.checked = e.target.checked;");
                stringBuilder.Append(@"                setCheckBoxState(this);");
                stringBuilder.Append(@"            }});");
                stringBuilder.Append(@"        }}");
                stringBuilder.Append(@"    }});");

                stringBuilder.Append(@"    $(""[id^=grid]"").on(""mouseover"", ""tr"", function () {{");
                stringBuilder.Append(@"        setCheckBoxVisibility(this, true);");
                stringBuilder.Append(@"    }});");

                stringBuilder.Append(@"    $(""[id^=grid]"").on(""mouseout"", ""tr"", function () {{");
                stringBuilder.Append(@"        setCheckBoxVisibility(this, $(this).find("".gridCheckBox"").prop(""checked""));");
                stringBuilder.Append(@"    }});");

                stringBuilder.Append(@"    function setCheckBoxState(checkBox) {{");
                stringBuilder.Append(@"        var row = $(checkBox).closest(""tr"");");
                stringBuilder.Append(@"        var dataItem = dataGrid.dataItem(row);");
                stringBuilder.Append(@"        var selectedIds = grid.prop(""selectedRows"") == undefined || grid.prop(""selectedRows"") == """" ? [] : grid.prop(""selectedRows"").split("","");");

                stringBuilder.Append(@"        if (!{14}) {{");
                stringBuilder.Append(@"            returnValues = [], selectedIds = [];");
                stringBuilder.Append(@"            grid.find(""input:checkbox.gridCheckBox"").not(checkBox).each(function () {{");
                stringBuilder.Append(@"                this.checked = false;");
                stringBuilder.Append(@"                $(this).css(""visibility"", ""hidden"");");
                stringBuilder.Append(@"            }});");
                stringBuilder.Append(@"        }}");

                stringBuilder.Append(@"        var index = selectedIds.indexOf(dataItem.id.toString());");
                stringBuilder.Append(@"        if (checkBox.checked) {{");
                stringBuilder.Append(@"            if (index < 0) {{");
                stringBuilder.Append(@"                selectedIds.push(dataItem.id.toString());");
                stringBuilder.Append(@"                dataItem[""{15}""] != null ? returnValues.push(dataItem[""{15}""].toString()) : """";");
                stringBuilder.Append(@"            }}");
                stringBuilder.Append(@"        }} else {{");
                stringBuilder.Append(@"            if (index >= 0) {{");
                stringBuilder.Append(@"                selectedIds.splice(index, 1);");
                stringBuilder.Append(@"                returnValues.splice(returnValues.indexOf(dataItem[""{15}""].toString()), 1);");
                stringBuilder.Append(@"            }}");
                stringBuilder.Append(@"        }}");

                stringBuilder.Append(@"        setCheckBoxVisibility(row, checkBox.checked);");
                stringBuilder.Append(@"        grid.prop(""selectedRows"", selectedIds.toString());");
                stringBuilder.Append(@"    }}");

                stringBuilder.Append(@"    function setCheckBoxVisibility(row, isVisible)");
                stringBuilder.Append(@"    {{");
                stringBuilder.Append(@"        var checkBoxVisibility = isVisible ? ""visible"" : ""hidden"";");
                stringBuilder.Append(@"        $(row).find("".gridCheckBox"").css(""visibility"", checkBoxVisibility);");
                stringBuilder.Append(@"    }}");

                stringBuilder.Append(@"    function onDataBoundWindow() {{");
                stringBuilder.Append(@"        var view = this.dataSource.view();");
                stringBuilder.Append(@"        var selectedIds = grid.prop(""selectedRows"") == undefined ? [] : grid.prop(""selectedRows"").split("","");");

                stringBuilder.Append(@"        $.each(view, function () {{");
                stringBuilder.Append(@"            if (this.id != undefined && selectedIds.indexOf(this.id.toString()) >= 0) {{");
                stringBuilder.Append(@"                dataGrid.tbody.find(""tr[data-uid='"" + this.uid + ""']"").find("".gridCheckBox"").attr(""checked"", true);");
                stringBuilder.Append(@"            }}");
                stringBuilder.Append(@"        }});");

                stringBuilder.Append(@"        grid.find(""thead th"").map(function () {{");
                stringBuilder.Append(@"            $(this).attr(""title"", $(this).attr('data-title'));");
                stringBuilder.Append(@"        }});");

                stringBuilder.Append(@"        grid.find(""tbody td"").map(function () {{");
                stringBuilder.Append(@"            $(this).attr(""title"", $(this).text());");
                stringBuilder.Append(@"        }});");

                stringBuilder.Append(@"        $(checkBoxSelectAll, grid).prop(""checked"", (grid.find(""input:checkbox.gridCheckBox:not(:checked)"").length == 0));");
                stringBuilder.Append(@"    }}");

                stringBuilder.Append(@"    $(""#search_{0}"").click(function () {{");
                stringBuilder.Append(@"        var gridColumns = [];");
                stringBuilder.Append(@"        $.each(dataGrid.columns, function () {{");
                stringBuilder.Append(@"            if (this.field != null && this.hidden != true && this.skipSearch != true) {{");
                stringBuilder.Append(@"                gridColumns.push(this.field);");
                stringBuilder.Append(@"            }}");
                stringBuilder.Append(@"        }});");

                stringBuilder.Append(@"        dataGrid.dataSource.filter(getFilter($.trim(search.val()), gridColumns));");
                stringBuilder.Append(@"        dataGrid.dataSource.read();");
                stringBuilder.Append(@"        $(checkBoxSelectRow, checkBoxSelectAll, grid).prop(""checked"", false);");
                stringBuilder.Append(@"    }});");

                stringBuilder.Append(@"    $(""#searchText_{0}"").on(""input"", function (e) {{");
                stringBuilder.Append(@"        if (!this.value) {{");
                stringBuilder.Append(@"            dataGrid.dataSource.filter(null);");
                stringBuilder.Append(@"            $(""form.k-filter-menu button[type='reset']"").trigger(""click"");");
                stringBuilder.Append(@"            dataGrid.dataSource.read();");
                stringBuilder.Append(@"            search.val("""");");
                stringBuilder.Append(@"        }}");
                stringBuilder.Append(@"    }});");

                stringBuilder.Append(@"    $(""#searchText_{0}"").keypress(function (e) {{");
                stringBuilder.Append(@"        if (e.keyCode == 13) {{");
                stringBuilder.Append(@"            e.preventDefault();");
                stringBuilder.Append(@"            $(""#search_{0}"").trigger(""click"");");
                stringBuilder.Append(@"        }}");
                stringBuilder.Append(@"    }});");

                stringBuilder.Append(@"    function getFilter(searchText, columns) {{");
                stringBuilder.Append(@"        if (searchText == """") {{ return null; }}");

                stringBuilder.Append(@"        var columnFilter = [];");
                stringBuilder.Append(@"        $.each(columns, function () {{");
                stringBuilder.Append(@"            columnFilter.push({{ ""field"": this, ""operator"": ""contains"", ""value"": searchText }});");
                stringBuilder.Append(@"        }});");

                stringBuilder.Append(@"        return {{ ""filters"": columnFilter, ""logic"": ""or"" }};");
                stringBuilder.Append(@"    }}");
                stringBuilder.Append(@"}});");

                var filter = string.IsNullOrEmpty(m_filter) ? string.Empty : string.Format("filter: {{ field: '{0}', operator: '{1}', value: '{2}' }},", m_filter, m_operator, m_value);

                var searchBoxScript = string.Format(stringBuilder.ToString(), m_buttonId, m_title, m_isModal.ToString().ToLower(), m_width, GridFields, m_sort, m_sortDirection,
                    filter, m_idColumn, ModelFields, m_controller, m_action, m_controlId, m_bindData, m_multiSelect.ToString().ToLower(), m_returnValue);

                var minifier = new Minifier();
                return string.Format("<script>{0}</script>", minifier.MinifyJavaScript(searchBoxScript));
            }
        }

        /// <summary>
        /// Sets Action Function Name
        /// </summary>
        /// <param name="action">Action Function Name</param>
        /// <returns>SearchBox Object</returns>
        public SearchBox Action(string action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            m_action = action;
            return this;
        }

        /// <summary>
        /// Sets Java script function to Bind Data
        /// </summary>
        /// <param name="bindData">Java script Function Name</param>
        /// <returns>SearchBox Object</returns>
        public SearchBox BindData(string bindData)
        {
            if (bindData == null)
            {
                throw new ArgumentNullException("bindData");
            }

            m_bindData = bindData;
            return this;
        }

        /// <summary>
        /// Sets the output Columns
        /// ColumName: HeaderText:DataType:True:50
        /// Column Name, Header Text, Data type, Hidden and Column Width separated by Colon(:),
        /// Column Names separated by Comma(,)
        /// </summary>
        /// <param name="columns">Column List</param>
        /// <returns>SearchBox Object</returns>
        public SearchBox Columns(string columns)
        {
            if (columns == null)
            {
                throw new ArgumentNullException("columns");
            }

            m_columns = Array.ConvertAll(columns.Split(','), x => x.Trim());
            return this;
        }

        /// <summary>
        /// Search Box Control where the return Value is displayed
        /// </summary>
        /// <param name="controlId">Control Id</param>
        /// <returns>SearchBox Object</returns>
        public SearchBox ControlId(string controlId)
        {
            if (controlId == null)
            {
                throw new ArgumentNullException("controlId");
            }

            m_controlId = controlId;
            return this;
        }

        /// <summary>
        /// Sets Controller Class Name
        /// </summary>
        /// <param name="controller">Controller Class Name</param>
        /// <returns>SearchBox Object</returns>
        public SearchBox Controller(string controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            m_controller = controller;
            return this;
        }

        /// <summary>
        /// Filter
        /// </summary>
        /// <param name="filter">Field:Operator:Value</param>
        /// <returns>SearchBox Object</returns>
        public SearchBox Filter(string filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            var filters = filter.Split(':');
            m_filter = filters[0]; m_operator = filters[1]; m_value = filters[2];

            return this;
        }

        /// <summary>
        /// Sets the Id Column
        /// </summary>
        /// <param name="idColumn">Id Column Name</param>
        /// <returns>SearchBox Object</returns>
        public SearchBox Id(string idColumn)
        {
            if (idColumn == null)
            {
                throw new ArgumentNullException("idColumn");
            }

            m_idColumn = idColumn;
            return this;
        }

        /// <summary>
        /// Sets Modal State True by Default
        /// </summary>
        /// <param name="isModal">Dialog is Modal</param>
        /// <returns>SearchBox Object</returns>
        public SearchBox IsModal(bool isModal)
        {
            m_isModal = isModal;
            return this;
        }

        /// <summary>
        /// Sets Multi Select Grid True by Default
        /// </summary>
        /// <param name="multiSelect">Multi Select Grid</param>
        /// <returns>SearchBox Object</returns>
        public SearchBox MultiSelect(bool multiSelect)
        {
            m_multiSelect = multiSelect;
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
        /// Column whose values are to be Returned
        /// </summary>
        /// <param name="returnValue">Return Column Name</param>
        /// <returns>SearchBox Object</returns>
        public SearchBox ReturnValue(string returnValue)
        {
            if (returnValue == null)
            {
                throw new ArgumentNullException("returnValue");
            }

            m_returnValue = returnValue;
            return this;
        }

        /// <summary>
        /// Sets Sort By Column and Sort Direction
        /// </summary>
        /// <param name="sort">Sort Column</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <returns>SearchBox Object</returns>
        public SearchBox Sort(string sort, string sortDirection = "asc")
        {
            if (sort == null)
            {
                throw new ArgumentNullException("sort");
            }

            m_sort = sort;
            m_sortDirection = sortDirection;

            return this;
        }

        /// <summary>
        /// Sets Window Title
        /// </summary>
        /// <param name="title">Window Title</param>
        /// <returns>SearchBox Object</returns>
        public SearchBox Title(string title)
        {
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }

            m_title = string.Format("{0} - {1} {2}", title, m_multiSelect ? Resource.Text_Multiple : Resource.Text_Single, Resource.Text_Select);
            return this;
        }

        /// <summary>
        /// Implement Interface Member of IHtmlString
        /// </summary>
        /// <returns>HTML to Render Control</returns>
        public string ToHtmlString()
        {
            return string.Format("{0}{1}{2}", SearchBoxScript, Environment.NewLine, SearchBoxHtml);
        }

        /// <summary>
        /// Sets Window Width in Pixels 900 by Default
        /// </summary>
        /// <param name="width">Window Width</param>
        /// <returns>SearchBox Object</returns>
        public SearchBox Width(int width)
        {
            m_width = width;
            return this;
        }
    }
}