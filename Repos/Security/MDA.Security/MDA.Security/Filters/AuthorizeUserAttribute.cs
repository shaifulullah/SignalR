namespace MDA.Security.Filters
{
    using BLL;
    using Core.Exception;
    using Globals;
    using Helpers;
    using IBLL;
    using Models;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using ViewModels;

    /// <summary>
    /// Authorize User Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Gets or sets Data Operation (CREATE/READ/UPDATE)
        /// </summary>
        public DataOperation Operation { get; set; }

        /// <summary>
        /// Gets or sets Data Operations (CREATE/READ/UPDATE)
        /// </summary>
        public DataOperation[] Operations { get; set; }

        /// <summary>
        /// Gets or sets Security Item Code
        /// </summary>
        public string SecurityItemCode { get; set; }

        /// <summary>
        /// On Authorization
        /// </summary>
        /// <param name="filterContext">Filter Context</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var canSkipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

            var actionName = filterContext.ActionDescriptor.ActionName;
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            var logData = string.Format("Controller: {0}, Action: {1}", controllerName, actionName);

            if (((Operation > 0 || Operations.Length > 0) && SecurityItemCode.Length > 0) && canSkipAuthorization == false && !string.IsNullOrEmpty(ApplicationGlobals.UserName))
            {
                var canPerformAction = false;
                if (Operation > 0)
                {
                    canPerformAction = AuthorizeUser.CanPerformAction(SecurityItemCode, Operation);
                }
                else
                {
                    Operations.ToList().ForEach(x =>
                    {
                        canPerformAction = canPerformAction || AuthorizeUser.CanPerformAction(SecurityItemCode, x);
                    });
                }

                if (!canPerformAction)
                {
                    // Insert User Log
                    IUserLogBll iUserLogBll = new UserLogBll();
                    iUserLogBll.InsertUserLog(new UserLog
                    {
                        LnUserAccountId = ApplicationGlobals.UserId,
                        ActionDateTime = DateTime.UtcNow,
                        LnLogActionId = 3,
                        Domain = logData,
                        HasLonSucceeded = true,
                        ClientIPAddress = ApplicationGlobals.ClientIpAddress,
                        Application = ApplicationGlobals.ApplicationName
                    }, ApplicationGlobals.UserName);

                    throw new UnauthorizedException("Access Denied");
                }
            }

            base.OnAuthorization(filterContext);
        }
    }
}