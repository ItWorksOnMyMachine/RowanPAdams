using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace RowanAdams.Tests
{
	public static class MvcMockHelpers
	{
		public static T SetControllerContext<T>(this T controller, string applicationPath, string url) where T : Controller
		{
			var context = FakeHttpContext(applicationPath, url);
			controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);
			controller.Url = new UrlHelper(new RequestContext(context, new RouteData()), new RouteCollection());
			return controller;
		}

		public static T SetControllerContext<T>(this T controller, HttpMethod method, string url) where T : ApiController
		{
			var config = new HttpConfiguration();
			var request = new HttpRequestMessage(method, url);
			config.MapHttpAttributeRoutes();
			config.EnsureInitialized();
			controller.Request = request;
			controller.Request.SetConfiguration(config);
			return controller;
		}

		private static HttpContextBase FakeHttpContext(string applicationPath = "/")
		{
			var context = new Mock<HttpContextBase>();
			var request = new Mock<HttpRequestBase>();
			var response = new Mock<HttpResponseBase>();
			var session = new Mock<HttpSessionStateBase>();
			var server = new Mock<HttpServerUtilityBase>();

			context.Setup(ctx => ctx.Request).Returns(request.Object);
			context.Setup(ctx => ctx.Response).Returns(response.Object);
			context.Setup(ctx => ctx.Session).Returns(session.Object);
			context.Setup(ctx => ctx.Server).Returns(server.Object);
			context.Setup(x => x.Request).Returns(request.Object);
			context.Setup(x => x.Response).Returns(response.Object);

			request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
			response.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(x => x);
			request.Setup(x => x.ApplicationPath).Returns(applicationPath);

			return context.Object;
		}

		private static HttpContextBase FakeHttpContext(string applicationPath, string url)
		{
			HttpContextBase context = FakeHttpContext(applicationPath);
			context.Request.SetupRequestUrl(url);
			return context;
		}

		private static string GetUrlFileName(string url)
		{
			if (url.Contains("?"))
				return url.Substring(0, url.IndexOf("?", StringComparison.CurrentCulture));
			else
				return url;
		}

		private static NameValueCollection GetQueryStringParameters(string url)
		{
			if (url.Contains("?"))
			{
				NameValueCollection parameters = new NameValueCollection();

				string[] parts = url.Split("?".ToCharArray());
				string[] keys = parts[1].Split("&".ToCharArray());

				foreach (string key in keys)
				{
					string[] part = key.Split("=".ToCharArray());
					parameters.Add(part[0], part[1]);
				}

				return parameters;
			}
			else
			{
				return null;
			}
		}

		private static void SetHttpMethodResult(this HttpRequestBase request, string httpMethod)
		{
			Mock.Get(request)
				.Setup(req => req.HttpMethod)
				.Returns(httpMethod);
		}

		private static void SetupRequestUrl(this HttpRequestBase request, string url)
		{
			if (url == null)
				throw new ArgumentNullException(nameof(url));

			if (url.StartsWith("~/"))
				throw new ArgumentException("Sorry, we do not expect a virtual url starting with \"~/\".");

			var mock = Mock.Get(request);

			mock.Setup(x => x.Url).Returns(new Uri(url, UriKind.Absolute));

			mock.Setup(req => req.QueryString)
				.Returns(GetQueryStringParameters(url));
			mock.Setup(req => req.AppRelativeCurrentExecutionFilePath)
				.Returns(GetUrlFileName(url));
			mock.Setup(req => req.PathInfo)
				.Returns(string.Empty);
		}
	}
}
