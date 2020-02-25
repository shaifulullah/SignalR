namespace MDA.Core.Helpers
{
    using Microsoft.Ajax.Utilities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;

    /// <summary>
    /// Static UserMenuItemsList Helper Class
    /// </summary>
    public static class UserMenuItemsListHelper
    {
        /// <summary>
        /// Builds a UserMenuItemsList Control
        /// </summary>
        /// <param name="htmlHelper">HTML Helper</param>
        /// <param name="userMenuItemsList">List of UserMenuItems</param>
        /// <returns>HTML to Render Control</returns>
        public static UserMenuItemsList UserMenuItemsList(this HtmlHelper htmlHelper, IEnumerable<UserMenuItems> userMenuItemsList)
        {
            return new UserMenuItemsList(htmlHelper, userMenuItemsList);
        }
    }

    /// <summary>
    /// Builds a UserMenuItemsList Control
    /// </summary>
    public class UserMenuItemsList : IHtmlString
    {
        /// <summary>
        /// HTML Helper
        /// </summary>
        private readonly HtmlHelper _htmlHelper;

        /// <summary>
        /// UserMenuItems List
        /// </summary>
        private List<UserMenuItems> _userMenuItemsList;

        /// <summary>
        /// Initializes a new instance of the UserMenuItemsList class.
        /// </summary>
        /// <param name="htmlHelper">HTML Helper</param>
        /// <param name="userMenuItemsList">List of UserMenuItems</param>
        public UserMenuItemsList(HtmlHelper htmlHelper, IEnumerable<UserMenuItems> userMenuItemsList)
        {
            _htmlHelper = htmlHelper;
            _userMenuItemsList = userMenuItemsList.ToList();
        }

        /// <summary>
        /// Gets UserMenuItemsList Script
        /// </summary>
        protected string UserMenuItemsListScript
        {
            get
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.Append(@"$("".dropdown-submenu a.mainLink"").on(""click"", function(e) {{");
                stringBuilder.Append(@"    $("".dropdown-submenu .dropdown-menu"").hide();");
                stringBuilder.Append(@"    $(this).next('ul').toggle();");
                stringBuilder.Append(@"    e.stopPropagation();");
                stringBuilder.Append(@"    e.preventDefault();");
                stringBuilder.Append(@"}});");

                var minifier = new Minifier();
                return string.Format("<script>{0}</script>", minifier.MinifyJavaScript(stringBuilder.ToString()));
            }
        }

        /// <summary>
        /// Render Control
        /// </summary>
        public void Render()
        {
            var writer = _htmlHelper.ViewContext.Writer;
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
            var stringBuilder = new StringBuilder();
            var urlHelper = new UrlHelper(_htmlHelper.ViewContext.RequestContext);

            var ulOuterBuilder = new TagBuilder("ul");
            ulOuterBuilder.Attributes["class"] = "dropdown-menu";
            ulOuterBuilder.Attributes["style"] = "left:-120px;";

            stringBuilder.Append(ulOuterBuilder.ToString(TagRenderMode.StartTag));

            foreach (var userMenuItems in _userMenuItemsList)
            {
                if (userMenuItems.ParentId == 0)
                {
                    var liOuterBuilder = new TagBuilder("li");
                    liOuterBuilder.Attributes["class"] = "dropdown-submenu";

                    stringBuilder.Append(liOuterBuilder.ToString(TagRenderMode.StartTag));

                    var anchorOuterBuilder = new TagBuilder("a");
                    anchorOuterBuilder.Attributes["class"] = (userMenuItems.ActionName == "SignOut" || userMenuItems.ActionName == "SignInAsDifferentUser" || userMenuItems.ActionName == "MySettings") ? string.Empty : "mainLink";
                    anchorOuterBuilder.Attributes["tabindex"] = "-1";

                    var url = string.IsNullOrEmpty(userMenuItems.ActionName) && string.IsNullOrEmpty(userMenuItems.ControllerName) ? string.Empty : urlHelper.Action(userMenuItems.ActionName, userMenuItems.ControllerName);
                    anchorOuterBuilder.Attributes["href"] = url;

                    stringBuilder.Append(anchorOuterBuilder.ToString(TagRenderMode.StartTag));

                    var tableBuilder = new TagBuilder("table");
                    stringBuilder.Append(tableBuilder.ToString(TagRenderMode.StartTag));

                    var trBuilder = new TagBuilder("tr");
                    stringBuilder.Append(trBuilder.ToString(TagRenderMode.StartTag));

                    var tdBuilder = new TagBuilder("td");
                    stringBuilder.Append(tdBuilder.ToString(TagRenderMode.StartTag));

                    var imageBuilder = new TagBuilder("img");
                    imageBuilder.Attributes["src"] = userMenuItems.Image;
                    stringBuilder.Append(imageBuilder.ToString(TagRenderMode.SelfClosing));

                    stringBuilder.Append(tdBuilder.ToString(TagRenderMode.EndTag));

                    tdBuilder = new TagBuilder("td");
                    stringBuilder.Append(tdBuilder.ToString(TagRenderMode.StartTag));
                    stringBuilder.Append("&nbsp;");
                    stringBuilder.Append(tdBuilder.ToString(TagRenderMode.EndTag));

                    var thBuilder = new TagBuilder("th");

                    stringBuilder.Append(thBuilder.ToString(TagRenderMode.StartTag));
                    url = string.IsNullOrEmpty(userMenuItems.ActionName) && string.IsNullOrEmpty(userMenuItems.ControllerName) ? string.Empty : urlHelper.Action(userMenuItems.ActionName, userMenuItems.ControllerName);

                    var anchorInnerBuilder = new TagBuilder("a");
                    anchorInnerBuilder.Attributes["href"] = url;
                    anchorInnerBuilder.Attributes["style"] = "color: #94c0d2";

                    stringBuilder.Append(anchorInnerBuilder.ToString(TagRenderMode.StartTag));
                    stringBuilder.Append(userMenuItems.Code);
                    stringBuilder.Append(anchorInnerBuilder.ToString(TagRenderMode.EndTag));

                    stringBuilder.Append(thBuilder.ToString(TagRenderMode.EndTag));
                    stringBuilder.Append(trBuilder.ToString(TagRenderMode.EndTag));

                    trBuilder = new TagBuilder("tr");
                    stringBuilder.Append(trBuilder.ToString(TagRenderMode.StartTag));

                    tdBuilder = new TagBuilder("td");
                    tdBuilder.Attributes["colspan"] = "2";

                    stringBuilder.Append(tdBuilder.ToString(TagRenderMode.StartTag));
                    stringBuilder.Append("&nbsp;");
                    stringBuilder.Append(tdBuilder.ToString(TagRenderMode.EndTag));

                    tdBuilder = new TagBuilder("td");
                    stringBuilder.Append(tdBuilder.ToString(TagRenderMode.StartTag));
                    stringBuilder.Append(userMenuItems.Description);
                    stringBuilder.Append(tdBuilder.ToString(TagRenderMode.EndTag));

                    stringBuilder.Append(trBuilder.ToString(TagRenderMode.EndTag));
                    stringBuilder.Append(tableBuilder.ToString(TagRenderMode.EndTag));

                    stringBuilder.Append(anchorOuterBuilder.ToString(TagRenderMode.EndTag));

                    var userMenuItemsListForParentId = _userMenuItemsList.Where(x => x.ParentId == userMenuItems.Id).ToList();
                    if (userMenuItemsListForParentId.Count() > 0)
                    {
                        var ulInnerBuilder = new TagBuilder("ul");
                        ulInnerBuilder.Attributes["class"] = "dropdown-menu";
                        stringBuilder.Append(ulInnerBuilder.ToString(TagRenderMode.StartTag));

                        foreach (var childItem in userMenuItemsListForParentId)
                        {
                            var liInnerBuilder = new TagBuilder("li");
                            stringBuilder.Append(liInnerBuilder.ToString(TagRenderMode.StartTag));

                            url = string.IsNullOrEmpty(childItem.ActionName) && string.IsNullOrEmpty(childItem.ControllerName) ? string.Empty : urlHelper.Action(childItem.ActionName, childItem.ControllerName);

                            var anchorBuilder = new TagBuilder("a");
                            anchorBuilder.Attributes["href"] = string.Format("{0}?code={1}", url, childItem.HtmlAttributeCode);

                            stringBuilder.Append(anchorBuilder.ToString(TagRenderMode.StartTag));
                            stringBuilder.Append(childItem.Code);
                            stringBuilder.Append(anchorBuilder.ToString(TagRenderMode.EndTag));

                            stringBuilder.Append(liInnerBuilder.ToString(TagRenderMode.EndTag));
                        }

                        stringBuilder.Append(ulInnerBuilder.ToString(TagRenderMode.EndTag));
                    }

                    stringBuilder.Append(liOuterBuilder.ToString(TagRenderMode.EndTag));
                }
            }

            stringBuilder.Append(ulOuterBuilder.ToString(TagRenderMode.EndTag));
            stringBuilder.Append(UserMenuItemsListScript);

            return stringBuilder.ToString();
        }
    }
}