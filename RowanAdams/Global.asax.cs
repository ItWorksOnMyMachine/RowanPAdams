using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RowanAdams
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		protected void Application_PreSendRequestHeaders()
		{
			Response.Headers.Remove("Server");
			Response.Headers.Remove("X-AspNet-Version");
			Response.Headers.Remove("X-AspNetMvc-Version");
		}

		protected void Application_BeginRequest()
		{
			var newUrl = String.Empty;

			if (ConfigurationManager.AppSettings["requireSubdomain"] == "true")
			{
				var validSubdomains = ConfigurationManager.AppSettings["validSubdomains"].Split(new[] { ',' });
				var firstDot = Context.Request.Url.Host.IndexOf('.');
				var subDomain = Context.Request.Url.Host.Substring(0, firstDot);

				if (validSubdomains.All(sd => String.Compare(sd, subDomain, StringComparison.InvariantCultureIgnoreCase) != 0))
					newUrl = Context.Request.Url.ToString().Replace("://" + Context.Request.Url.Host, $"://{ConfigurationManager.AppSettings["preferredSubdomain"]}.{Context.Request.Url.Host}");
			}

			if (newUrl != String.Empty)
				Response.Redirect(newUrl);
		}
	}
}
