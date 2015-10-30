using System;
using System.Collections.Specialized;
using System.Web;

namespace RowanAdams.Tests.Fakes.Http
{
    public class FakeHttpRequest : HttpRequestBase
    {
        private readonly NameValueCollection _formParams;
        private readonly NameValueCollection _queryStringParams;
        private readonly HttpCookieCollection _cookies;
        private readonly NameValueCollection _headers;
		private readonly Uri _uri;

		public FakeHttpRequest(NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies, NameValueCollection headers, Uri uri)
        {
            _formParams = formParams;
            _queryStringParams = queryStringParams;
            _cookies = cookies;
            _headers = headers;
			_uri = uri;
        }

        public override NameValueCollection Headers
        {
            get { return _headers; }
        }

        public override NameValueCollection Form
        {
            get { return _formParams; }
        }

        public override NameValueCollection QueryString
        {
            get { return _queryStringParams; }
        }

        public override HttpCookieCollection Cookies
        {
            get { return _cookies; }
        }

	    public override Uri Url { get { return _uri; } }
	}
}