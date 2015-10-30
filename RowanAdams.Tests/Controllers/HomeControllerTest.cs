using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RowanAdams;
using RowanAdams.Controllers;
using RowanAdams.Tests.Fakes.Http;
using Moq;
using RowanAdams.Tests.Fakes.Services;

namespace RowanAdams.Tests.Controllers
{
	[TestClass]
	public class HomeControllerTest
	{
		FakeConfigurationService config = new FakeConfigurationService();

		[TestMethod]
		public void Index()
		{
			// Arrange
			config.SetAppSetting("base", "/RowanAdams/");
			config.SetAppSetting("apiUrl", "http://localhost/RowanAdams/");
			HomeController controller = new HomeController(config).SetControllerContext("/RowanAdams/", "http://localhost/RowanAdams/Home");
			
			// Act
			var result = controller.Index() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual("/RowanAdams/", controller.ViewBag.Base);
			Assert.AreEqual("http://localhost/RowanAdams/", controller.ViewBag.ApiUrl);
		}

		[TestMethod]
		public void Home()
		{
			// Arrange
			HomeController controller = new HomeController(config).SetControllerContext("/RowanAdams/", "http://localhost/RowanAdams/Home/Home");

			// Act
			var result = controller.Home() as PartialViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Log()
		{
			// Arrange
			HomeController controller = new HomeController(config).SetControllerContext("/RowanAdams/", "http://localhost/RowanAdams/Home/Log");

			// Act
			var result = controller.Log() as PartialViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Chores()
		{
			// Arrange
			HomeController controller = new HomeController(config).SetControllerContext("/RowanAdams/", "http://localhost/RowanAdams/Home/Chores");

			// Act
			var result = controller.Chores() as PartialViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void History()
		{
			// Arrange
			HomeController controller = new HomeController(config).SetControllerContext("/RowanAdams/", "http://localhost/RowanAdams/Home/History");

			// Act
			var result = controller.History() as PartialViewResult;

			// Assert
			Assert.IsNotNull(result);
		}
	}
}

