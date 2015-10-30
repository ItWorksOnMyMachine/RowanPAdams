using System.Web;

namespace RowanAdams.Tests.Fakes.Http
{
    public class FakeHttpResponse : HttpResponseBase
    {
        public override int StatusCode { get; set; }
    }
}