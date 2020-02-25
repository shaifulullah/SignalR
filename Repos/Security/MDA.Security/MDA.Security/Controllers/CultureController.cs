namespace MDA.Security.Controllers
{
    using Core.Filters;
    using Globals;
    using Resources;
    using System;
    using System.Globalization;
    using System.Web.Mvc;

    [HandleAndLogError(ExceptionType = typeof(Exception), View = "_Error")]
    public class CultureController : Controller
    {
        /// <summary>
        /// Set Culture
        /// </summary>
        /// <param name="code">Culture Code</param>
        /// <returns></returns>
        public RedirectResult SetCulture(string code)
        {
            ApplicationGlobals.CultureInfo = new CultureInfo(code);
            return Redirect(Request.UrlReferrer.ToString());
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