using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RowanAdams.Services;

namespace RowanAdams.Controllers
{
	public class HomeController : Controller
	{
		private IConfigurationService _config;

        public HomeController(IConfigurationService config)
        {
	        _config = config;
        }

		public ActionResult Index()
		{
			ViewBag.Base = _config.GetAppSetting("base");
			ViewBag.ApiUrl = _config.GetAppSetting("apiUrl");
			return View();
		}

		public ActionResult Home()
		{
			return PartialView();
		}

		//[Authorize]
		public ActionResult Log()
		{
			return PartialView();
		}

		//[Authorize]
		public ActionResult Chores()
		{
			return PartialView();
		}

		//[Authorize]
		public ActionResult History()
		{
			return PartialView();
		}


		//public ActionResult One()
		//{
		//	return PartialView();
		//}

		//public ActionResult Two(int? num)
		//{
		//	ViewBag.Num = num;
		//	return PartialView();
		//}

		//[Authorize]
		//public ActionResult Three()
		//{
		//	return PartialView();
		//}

	}
}
