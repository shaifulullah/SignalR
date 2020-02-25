namespace MDA.Security
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/popup")
                .Include("~/Scripts/application/mda.popup.functions.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval")
               .Include("~/Scripts/jquery/jquery.validate*",
                   "~/Scripts/jquery/jquery.unobtrusive*"));

            bundles.Add(new ScriptBundle("~/bundles/jquerycookie")
                .Include("~/Scripts/jquery/jquery.cookie.js",
                    "~/Scripts/jquery/jquery.cookie-1.4.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .Include("~/Scripts/modernizr/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/js")
                .Include("~/Scripts/kendo/kendo.web.min.js",
                    "~/Scripts/application/mda.form.functions.js"));

            bundles.Add(new ScriptBundle("~/bundles/treeview")
                .Include("~/Scripts/application/mda.treeview.functions.js"));

            bundles.Add(new ScriptBundle("~/bundles/treeviewhorizontal")
                .Include("~/Scripts/application/mda.treeview.horizontal.functions.js"));

            bundles.Add(new ScriptBundle("~/bundles/grid")
                .Include("~/Scripts/application/mda.grid.functions.js",
                    "~/Scripts/application/mda.grid.validations.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui")
                .Include("~/Scripts/jquery/jquery-ui-1.12.1.js",
                    "~/Scripts/jquery/jquery-ui-1.12.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/Scripts/bootstrap/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/content/css")
              .Include("~/Content/kendo/kendo.common.min.css",
                  "~/Content/kendo/kendo.blueopal.min.css",
                  "~/Content/kendo/kendo.mda.css",
                  "~/Content/application/site.css"));
        }
    }
}