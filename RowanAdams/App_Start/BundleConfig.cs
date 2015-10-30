using System.Web;
using System.Web.Optimization;

namespace RowanAdams
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/daterangepicker.css",
					  "~/Content/bootstrap-theme.css",
					  //"~/Content/Themes/darkly.css",
					  "~/Content/site.css"));

			bundles.Add(new ScriptBundle("~/bundles/application")
				.Include("~/Scripts/moment.js")
				.Include("~/Scripts/daterangepicker.js")
				.Include("~/Scripts/Application/lib.js")
				.IncludeDirectory("~/Scripts/Application/Services", "*.js")
				.IncludeDirectory("~/Scripts/Application/Factories", "*.js")
				.IncludeDirectory("~/Scripts/Application/Controllers", "*.js")
				.Include("~/Scripts/Application/application.js"));
		}
	}
}
