namespace MDA.Security.Filters
{
    using Helpers;
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Is New Session If the Session is new Validates the User
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class IsNewSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session != null)
            {
                if (filterContext.HttpContext.ApplicationInstance.Session.IsNewSession)
                {
                    var isValidLogin = ApplicationLogin.ValidateApplicationLogin(HttpContext.Current.User.Identity.Name, HttpContext.Current.Request.UserHostAddress);
                    if (!isValidLogin)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Error" }, { "Action", "UnauthorizedAccess" } });
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}