namespace MDA.Security.Filters
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Session Timeout Redirection If the Session has timed out then the user will be redirected to
    /// the Login Page
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session != null)
            {
                if (filterContext.HttpContext.Session.IsNewSession)
                {
                    var sessionCookie = filterContext.HttpContext.Request.Headers["Cookie"];
                    if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId", StringComparison.Ordinal) >= 0))
                    {
                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                        {
                            var urlHelper = new UrlHelper(filterContext.RequestContext);
                            filterContext.Result = new JsonResult
                            {
                                Data = new { error = true, url = urlHelper.Action("SessionTimeout", "Error") },
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            };

                            filterContext.HttpContext.Response.StatusCode = 502;
                        }
                        else
                        {
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Error" }, { "action", "SessionTimeout" } });
                        }
                    }
                }
            }

            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            base.OnActionExecuting(filterContext);
        }
    }
}