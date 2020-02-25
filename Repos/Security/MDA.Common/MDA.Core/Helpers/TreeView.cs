//-----------------------------------------------------------------------
// <copyright file="SearchBox.cs" company="MDA Corporation">
//     Copyright (c) MDA Corporation. All rights reserved.
// </copyright>
// <author>Lionel Daniel</author>
//-----------------------------------------------------------------------

namespace MDA.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;
    using System.Web.WebPages;

    /// <summary>
    /// Static TreeView Helper Class
    /// </summary>
    public static class TreeViewHelper
    {
        /// <summary>
        /// Builds an HTML Tree from a Recursive Collection
        /// </summary>
        /// <typeparam name="T">Tree Type</typeparam>
        /// <param name="html">HTML Helper</param>
        /// <param name="items">List of Tree Items</param>
        /// <returns>HTML to Render Control</returns>
        public static TreeView<T> TreeView<T>(this HtmlHelper html, IEnumerable<T> items)
        {
            return new TreeView<T>(html, items);
        }
    }

    /// <summary>
    /// Builds an HTML Tree from a Recursive Collection
    /// </summary>
    /// <typeparam name="T">Tree Type</typeparam>
    public class TreeView<T> : IHtmlString
    {
        private readonly HtmlHelper m_html;
        private readonly IEnumerable<T> m_items;

        private IDictionary<string, object> m_childHtmlAttributes = new Dictionary<string, object>();
        private Func<T, string> m_code;
        private Func<T, string> m_displayProperty = item => item.ToString();
        private Func<T, bool> m_hasChildren;
        private IDictionary<string, object> m_htmlAttributes = new Dictionary<string, object>();
        private Func<T, int> m_id;
        private Func<T, HelperResult> m_itemTemplate;
        private Func<T, string> m_parentCode;
        private Func<T, int> m_parentId;

        /// <summary>
        /// Initializes a new instance of the TreeView class.
        /// </summary>
        /// <param name="html">HTML Helper</param>
        /// <param name="items">List of Tree Items</param>
        public TreeView(HtmlHelper html, IEnumerable<T> items)
        {
            if (html == null)
            {
                throw new ArgumentNullException("html");
            }

            m_html = html;
            m_items = items;

            m_itemTemplate = item => new HelperResult(writer => writer.Write(m_displayProperty(item)));
        }

        /// <summary>
        /// HTML Attributes applied to Children Nodes
        /// </summary>
        /// <param name="htmlAttributes">HTML Attributes</param>
        /// <returns>TreeView Object</returns>
        public TreeView<T> ChildrenHtmlAttributes(object htmlAttributes)
        {
            ChildrenHtmlAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return this;
        }

        /// <summary>
        /// HTML Attributes applied to Children Nodes
        /// </summary>
        /// <param name="htmlAttributes">HTML Attributes</param>
        /// <returns>TreeView Object</returns>
        public TreeView<T> ChildrenHtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null)
            {
                throw new ArgumentNullException("ChildrenHtmlAttributes");
            }

            m_childHtmlAttributes = htmlAttributes;
            return this;
        }

        /// <summary>
        /// Code Field
        /// </summary>
        /// <param name="selector">Code Field Name</param>
        /// <returns>TreeView Object</returns>
        public TreeView<T> Code(Func<T, string> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("Code");
            }

            m_code = selector;
            return this;
        }

        /// <summary>
        /// Has children Field
        /// </summary>
        /// <param name="selector">Has children Field Name</param>
        /// <returns>TreeView Object</returns>
        public TreeView<T> HasChildren(Func<T, bool> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("HasChildren");
            }

            m_hasChildren = selector;
            return this;
        }

        /// <summary>
        /// Html Attributes applied to Root Nodes
        /// </summary>
        /// <param name="htmlAttributes">HTML Attributes</param>
        /// <returns>TreeView Object</returns>
        public TreeView<T> HtmlAttributes(object htmlAttributes)
        {
            HtmlAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return this;
        }

        /// <summary>
        /// HTML Attributes applied to Root Nodes
        /// </summary>
        /// <param name="htmlAttributes">HTML Attributes</param>
        /// <returns>TreeView Object</returns>
        public TreeView<T> HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null)
            {
                throw new ArgumentNullException("HtmlAttributes");
            }

            m_htmlAttributes = htmlAttributes;
            return this;
        }

        /// <summary>
        /// Id Field
        /// </summary>
        /// <param name="selector">Id Field Name</param>
        /// <returns>TreeView Object</returns>
        public TreeView<T> Id(Func<T, int> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("Id");
            }

            m_id = selector;
            return this;
        }

        /// <summary>
        /// The Template used to render each Item
        /// </summary>
        /// <param name="itemTemplate">Item Template</param>
        /// <returns>TreeView Object</returns>
        public TreeView<T> ItemTemplate(Func<T, HelperResult> itemTemplate)
        {
            if (itemTemplate == null)
            {
                throw new ArgumentNullException("ItemTemplate");
            }

            m_itemTemplate = itemTemplate;
            return this;
        }

        /// <summary>
        /// The Text rendered for each Item
        /// </summary>
        /// <param name="selector">Item Text</param>
        /// <returns>TreeView Object</returns>
        public TreeView<T> ItemText(Func<T, string> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("ItemText");
            }

            m_displayProperty = selector;
            return this;
        }

        /// <summary>
        /// Parent Code Field
        /// </summary>
        /// <param name="selector">Parent Code Field Name</param>
        /// <returns>TreeView Object</returns>
        public TreeView<T> ParentCode(Func<T, string> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("ParentCode");
            }

            m_parentCode = selector;
            return this;
        }

        /// <summary>
        /// Parent Id Field
        /// </summary>
        /// <param name="selector">Parent Id Field Name</param>
        /// <returns>TreeView Object</returns>
        public TreeView<T> ParentId(Func<T, int> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("ParentId");
            }

            m_parentId = selector;
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
        /// Implement Interface Member of IHtmlString
        /// </summary>
        /// <returns>HTML to Render Control</returns>
        public string ToHtmlString()
        {
            var ulBuilder = new TagBuilder("ul");

            ulBuilder.MergeAttributes(m_htmlAttributes);
            BuildTree(ulBuilder, 0);

            return ulBuilder.ToString();
        }

        /// <summary>
        /// Builds Tree from Recursive Collection
        /// </summary>
        /// <param name="tag">Tag Name</param>
        /// <param name="id">Tree Item Id</param>
        /// <param name="code">Item Code</param>
        protected void BuildTree(TagBuilder tag, int id, string code = null)
        {
            foreach (var item in m_items.ToList())
            {
                if (m_parentId(item) == id && (m_parentCode == null || m_parentCode(item) == (code)))
                {
                    // Open <LI>
                    var li = GetLi(item);
                    tag.InnerHtml += li.ToString(TagRenderMode.StartTag) + li.InnerHtml;

                    // If Node has Children add them to the tree
                    if (m_hasChildren(item))
                    {
                        var ulBuilder = new TagBuilder("ul");

                        ulBuilder.MergeAttributes(m_htmlAttributes);
                        BuildTree(ulBuilder, m_id(item), m_code == null ? null : m_code(item));
                        tag.InnerHtml += ulBuilder.ToString();
                    }

                    // Close </LI>
                    tag.InnerHtml += li.ToString(TagRenderMode.EndTag);
                }
            }
        }

        /// <summary>
        /// LI tag for Tree Item
        /// </summary>
        /// <param name="item">Tree Item</param>
        /// <returns>LI Tag</returns>
        protected TagBuilder GetLi(T item)
        {
            var liBuilder = new TagBuilder("li");

            liBuilder.Attributes.Add("id", m_id(item).ToString(CultureInfo.InvariantCulture));
            if (m_hasChildren(item))
            {
                liBuilder.MergeAttributes(m_childHtmlAttributes);
            }

            liBuilder.InnerHtml = m_itemTemplate(item).ToHtmlString();
            return liBuilder;
        }
    }
}