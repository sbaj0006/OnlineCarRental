using System.Web;
using System.Web.Optimization;

namespace ApplicationCarRental
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/moment.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/carRentalJs").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/typeahead.bundle.js",
                      "~/Scripts/AdminMenu.js",
                      "~/Scripts/bootstrap-datetimepicker.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/thumbnail.css",
                      "~/Content/social.css",
                      "~/Content/star.css",
                      "~/Content/typeahead.css",
                      "~/Content/CarDetail.css",
                      "~/Content/site.css",
                      "~/Content/boostrap-datetimepicker.css"));
        }
    }
}
