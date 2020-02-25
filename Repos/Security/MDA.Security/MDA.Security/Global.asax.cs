namespace MDA.Security
{
    using Globals;
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Called before each action to Set Culture
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Arguments</param>
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (ApplicationGlobals.CultureInfo != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ApplicationGlobals.CultureInfo.Name);
                Thread.CurrentThread.CurrentUICulture = ApplicationGlobals.CultureInfo;
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}