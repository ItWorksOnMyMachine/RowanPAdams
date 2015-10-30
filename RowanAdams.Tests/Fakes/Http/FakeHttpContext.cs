using System;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.SessionState;

namespace RowanAdams.Tests.Fakes.Http
{
    public class FakeHttpContext : HttpContextBase
    {
        private readonly FakePrincipal _principal;
        private readonly NameValueCollection _formParams;
        private readonly NameValueCollection _queryStringParams;
        private readonly HttpCookieCollection _cookies;
        private readonly SessionStateItemCollection _sessionItems;
        private readonly NameValueCollection _headers;
		private readonly Uri _uri;

		public FakeHttpContext(FakePrincipal principal, NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies, SessionStateItemCollection sessionItems, NameValueCollection headers, Uri uri)
        {
            _principal = principal;
            _formParams = formParams;
            _queryStringParams = queryStringParams;
            _cookies = cookies;
            _sessionItems = sessionItems;
            _headers = headers;
			_uri = uri;
        }

        public override HttpRequestBase Request
        {
            get { return new FakeHttpRequest(_formParams, _queryStringParams, _cookies, _headers, _uri); }
        }

        public override HttpResponseBase Response
        {
            get { return new FakeHttpResponse(); }
        }

        public override IPrincipal User
        {
            get { return _principal; }
            set { throw new NotSupportedException(); }
        }

        public override HttpSessionStateBase Session
        {
            get { return new FakeHttpSessionState(_sessionItems); }
        }

    }
}