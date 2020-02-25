namespace MDA.Security.Controllers
{
    using Resources;
    using System;
    using System.Web;
    using System.Web.Mvc;

    public class ErrorController : Controller
    {
        /// <summary>
        /// Error
        /// </summary>
        /// <returns>Error View</returns>
        public ActionResult Error()
        {
            return View("_Error");
        }

        /// <summary>
        /// Session Timeout
        /// </summary>
        /// <returns>SessionTimeout View</returns>
        [AllowAnonymous]
        public ActionResult SessionTimeout()
        {
            // Clear the Session
            Session.Clear();
            Session.Abandon();

            // Delete all Cookies
            foreach (var cookie in Request.Cookies.AllKeys)
            {
                var expireCookie = new HttpCookie(cookie) { Expires = DateTime.Now.AddDays(-1d) };
                Response.Cookies.Add(expireCookie);
            }

            Request.Cookies.Clear();
            return View("_SessionTimeout");
        }

        /// <summary>
        /// Unauthorized Access
        /// </summary>
        /// <returns>UnauthorizedAccess View</returns>
        public ActionResult UnauthorizedAccess()
        {
            return View("_UnauthorizedAccess");
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