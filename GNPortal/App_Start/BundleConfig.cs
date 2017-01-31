using System.Web;
using System.Web.Optimization;

namespace GenomeNext.Portal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-val").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        "~/Scripts/jquery-ui*"));

            bundles.Add(new ScriptBundle("~/bundles/fineuploader").Include(
                        "~/Scripts/s3.jquery.fine-uploader-5.5.0/*.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/jquerybootgrid").Include(
                        "~/Scripts/jquery.bootgrid.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquerynestable").Include(
                        "~/Scripts/jquery.nestable.js"));

            bundles.Add(new ScriptBundle("~/bundles/tagit").Include(
                        "~/Scripts/tag-it.js"));
           
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/jquery-ui").Include(
                      "~/Content/themes/base/*.css"));

            bundles.Add(new StyleBundle("~/Content/fineuploader").Include(
                    "~/Content/s3.jquery.fine-uploader-5.5.0/*.css"));

            bundles.Add(new StyleBundle("~/Content/jquerybootgrid").Include(
                    "~/Content/jquery.bootgrid.css"));

            bundles.Add(new StyleBundle("~/Content/tagit").Include(
                    "~/Content/jquery.tagit.css"));
            bundles.Add(new StyleBundle("~/Content/tagitzendesk").Include(
                    "~/Content/tagit.ui-zendesk.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
