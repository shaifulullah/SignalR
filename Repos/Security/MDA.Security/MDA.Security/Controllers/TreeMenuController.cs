namespace MDA.Security.Controllers
{
    using BLL;
    using Core.Filters;
    using Helpers;
    using IBLL;
    using Models;
    using Resources;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using ViewModels;

    [HandleAndLogError(ExceptionType = typeof(Exception), View = "_Error")]
    public class TreeMenuController : Controller
    {
        /// <summary>
        /// Get TreeMenu List
        /// </summary>
        /// <returns>TreeMenuPartial View</returns>
        [ChildActionOnly]
        public ActionResult TreeMenuList()
        {
            ITreeMenuBll iTreeMenuBll = new TreeMenuBll();
            var treeMenuList = iTreeMenuBll.GetTreeMenuList();

            var treeMenus = treeMenuList as List<TreeMenu> ?? treeMenuList.ToList();
            foreach (var treeMenu in treeMenus)
            {
                if (treeMenu.Code != null)
                {
                    if (!AuthorizeUser.CanPerformAction(treeMenu.Code, DataOperation.ACCESS))
                    {
                        treeMenuList = treeMenus.Where(x => x.Code != treeMenu.Code);
                    }

                    var url = HttpContext.Request.Url;
                    if (url.ToString().ToUpper().EndsWith(treeMenu.URL.ToUpper().TrimStart('.')))
                    {
                        try
                        {
                            Response.Cookies.Add(new HttpCookie("SelectedNode", treeMenu.Id.ToString(CultureInfo.InvariantCulture)));
                            Response.Cookies.Add(new HttpCookie("ExpandedNode", treeMenu.LnParentId.ToString(CultureInfo.InvariantCulture)));
                        }
                        catch { }
                    }
                }
            }

            return PartialView("_TreeMenuPartial", treeMenuList);
        }

        /// <summary>
        /// Handle Unknown Action
        /// </summary>
        /// <param name="actionName">Action Name</param>
        protected override void HandleUnknownAction(string actionName)
        {
            throw new UnauthorizedAccessException(string.Format(ResourceStrings.Message_Unknown_Action, actionName));
        }
    }
}