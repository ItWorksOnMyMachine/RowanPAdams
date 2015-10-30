using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RowanAdams
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			// Account Login or Register sends to Mvc controller, other's go to ApiController
			routes.MapRoute(
				name: "Login",
				url: "Account/Login",
				defaults: new { controller = "MvcAccount", action = "Login" });

			routes.MapRoute(
				name: "Register",
				url: "Account/Register",
				defaults: new { controller = "MvcAccount", action = "Register" });

			// Anything in Home is a view to send back to the client
			routes.MapRoute(
				name: "HomeDefault",
				url: "Home/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

			// Any routes not at the Home controller are local state and should be handled by the client.
			routes.MapRoute(
				name: "Default",
				url: "{*url}",
				defaults: new { controller = "Home", action = "Index" });

		}
	}
}
