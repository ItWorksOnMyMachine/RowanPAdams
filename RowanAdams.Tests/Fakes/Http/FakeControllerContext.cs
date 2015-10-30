using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace RowanAdams.Tests.Fakes.Http
{
    public class FakeControllerContext : ControllerContext
    {
        public FakeControllerContext (
            IController controller,
            string userName = null,
            string[] roles = null,
            NameValueCollection formParams = null,
            NameValueCollection queryStringParams = null,
            HttpCookieCollection cookies = null,
            SessionStateItemCollection sessionItems = null,
            NameValueCollection headers = null,
			Uri uri = null
            ) : base(
                new FakeHttpContext(
                    new FakePrincipal(
                        new FakeIdentity(userName), 
                        roles), 
                    formParams, 
                    queryStringParams, 
                    cookies, 
                    sessionItems,
                    headers,
					uri), 
                new RouteData(), 
                controller as ControllerBase) { }
    }
}