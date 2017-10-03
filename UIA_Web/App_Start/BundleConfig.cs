using System.Web;
using System.Web.Optimization;

namespace UIA_Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/head").Include(
                        "~/Scripts/header_modernizer.js",
            "~/Scripts/secondary.js"));

            bundles.Add(new ScriptBundle("~/bundles/myscripts").Include(
                      "~/Scripts/core.min.js",
                      "~/Scripts/script.js"));

            bundles.Add(new StyleBundle("~/Content/mycss").Include(
                      "~/Content/style.css", new CssRewriteUrlTransform()));
        }
    }
}
