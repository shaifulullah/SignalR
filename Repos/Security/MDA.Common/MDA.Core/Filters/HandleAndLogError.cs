namespace MDA.Core.Filters
{
    using Exception;
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.ServiceModel;
    using System.Web;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class HandleAndLogErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// Gets Event Log Source
        /// </summary>
        protected string EventLogSource
        {
            get
            {
                return ConfigurationManager.AppSettings["EventLogSource"];
            }
        }

        /// <summary>
        /// Get Event Log Source Id
        /// </summary>
        protected int EventLogSourceId
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["EventLogSourceId"]);
            }
        }

        /// <summary>
        /// On Exception
        /// </summary>
        /// <param name="exceptionContext">Exception Context</param>
        public override void OnException(ExceptionContext exceptionContext)
        {
            var exception = exceptionContext.Exception;
            if (exceptionContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (exceptionContext.IsChildAction)
            {
                return;
            }

            if (exceptionContext.ExceptionHandled || !exceptionContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            if (new HttpException(null, exception).GetHttpCode() != 500)
            {
                return;
            }

            if (!ExceptionType.IsInstanceOfType(exception))
            {
                return;
            }

            var controllerName = (string)exceptionContext.RouteData.Values["controller"];
            var actionName = (string)exceptionContext.RouteData.Values["action"];

            var model = new HandleErrorInfo(exceptionContext.Exception, controllerName, actionName);
            if (exceptionContext.HttpContext.Request.IsAjaxRequest())
            {
                var urlHelper = new UrlHelper(exceptionContext.RequestContext);
                var redirectUrl = urlHelper.Action("Error", "Error");

                if (exceptionContext.Exception is UnauthorizedException)
                {
                    redirectUrl = urlHelper.Action("UnauthorizedAccess", "Error");
                }
                else if (exceptionContext.Exception is CommunicationObjectFaultedException || exceptionContext.Exception is FaultException)
                {
                    redirectUrl = urlHelper.Action("ServiceError", "Error");
                }

                exceptionContext.Result = new JsonResult
                {
                    Data = new { error = true, url = redirectUrl },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                exceptionContext.Result = new ViewResult
                {
                    ViewName = View,
                    MasterName = Master,
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                    TempData = exceptionContext.Controller.TempData
                };
            }

            var log = new EventLog { Source = EventLogSource };

            var eventLogEntryType = exceptionContext.Exception is UnauthorizedException ? EventLogEntryType.Warning : EventLogEntryType.Error;
            log.WriteEntry(exceptionContext.Exception.ToString(), eventLogEntryType, EventLogSourceId);

            exceptionContext.ExceptionHandled = true;
            exceptionContext.HttpContext.Response.Clear();
            exceptionContext.HttpContext.Response.StatusCode = 500;
            exceptionContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}