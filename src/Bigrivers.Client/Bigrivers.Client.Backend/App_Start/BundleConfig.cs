using System.Web;
using System.Web.Optimization;

namespace Bigrivers.Client.Backend
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/backend").Include(
                      "~/Scripts/Edit.js",
                      "~/Scripts/Admin/functions.js",
                      "~/Scripts/Admin/ckeditor/ckeditor.js",
                      "~/Scripts/jquery-ui-1.11.4.custom/jquery-ui.js",
                      "~/Scripts/jquery-ui-1.11.4.custom/external/addon/jquery-ui-timepicker-addon.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/site.css",
                      "~/Content/admin.css",
                      "~/Content/Sidebar.css",
                      "~/Content/Form.css",
                      "~/Scripts/jquery-ui-1.11.4.custom/jquery-ui.css",
                      "~/Scripts/jquery-ui-1.11.4.custom/external/addon/jquery-ui-timepicker-addon.css"));
        }
    }
}
