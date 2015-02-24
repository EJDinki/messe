using System.Web;
using System.Web.Optimization;

namespace DiscoveryCenter
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

            bundles.Add(new ScriptBundle("~/bundles/jqplot").Include(
                        "~/Scripts/jqplot/jquery.jqplot.min.js",
                        "~/Scripts/jqplot/jqplot.pieRenderer.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/SurveyView").Include(
                      "~/Scripts/bootstrap-slider.js"));

            bundles.Add(new ScriptBundle("~/bundles/Admin").Include(
                "~/Scripts/jquery-ui.min.js"));


            //styles
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/Admin").Include(
                      "~/Content/Admin.css",
                      "~/Content/jquery-ui.min.css",
                      "~/Content/jquery-ui.structure.min.css",
                      "~/Content/jquery-ui.theme.min.css"));

            bundles.Add(new StyleBundle("~/Content/SurveyView").Include(
                      "~/Content/bootstrap-slider.css",
                      "~/Content/SurveyView.css"));

            bundles.Add(new StyleBundle("~/Content/jqplot").Include(
                      "~/Content/jquery.jqplot.css"));

            

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
