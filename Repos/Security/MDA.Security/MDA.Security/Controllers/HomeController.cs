namespace MDA.Security.Controllers
{
    using Core.Filters;
    using Core.Helpers;
    using Filters;
    using Globals;
    using Resources;
    using System;
    using System.Web;
    using System.Web.Mvc;

    [HandleAndLogError(ExceptionType = typeof(Exception), View = "_Error")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Index
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// My Settings
        /// </summary>
        /// <returns>Index View</returns>
        public RedirectToRouteResult MySettings()
        {
            return RedirectToAction("Index", "UserApplicationFavourites");
        }

        /// <summary>
        /// Set TreeMenuLocation
        /// </summary>
        /// <param name="code">TreeMenuLocation Code</param>
        /// <returns></returns>
        public RedirectResult SetTreeMenuLocation(string code)
        {
            ApplicationGlobals.CanDisplayTreeMenuVertical = code == TreeMenuLocation.Vertical;
            return Redirect(Request.UrlReferrer.ToString());
        }

        /// <summary>
        /// Sign In As Different User
        /// </summary>
        /// <returns>Index View</returns>
        public ActionResult SignInAsDifferentUser()
        {
            if ((bool)(Session["IsEntered"] ?? false))
            {
                Session["IsEntered"] = false;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["IsEntered"] = true;
                return new HttpUnauthorizedResult();
            }
        }

        /// <summary>
        /// SignOut
        /// </summary>
        /// <returns>SignOut View</returns>
        public ActionResult SignOut()
        {
            Session.Clear();
            Session.Abandon();

            // Delete all Cookies
            foreach (var cookie in Request.Cookies.AllKeys)
            {
                var expireCookie = new HttpCookie(cookie) { Expires = DateTime.Now.AddDays(-1d) };
                Response.Cookies.Add(expireCookie);
            }

            Request.Cookies.Clear();
            return View("SignOut");
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