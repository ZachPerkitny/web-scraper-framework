using System.Net;

namespace RestFul.Http.Concrete
{
    class HttpContext : IHttpContext
    {
        public IHttpRequest Request { get; private set; }

        public IHttpResponse Response { get; private set; }

        public HttpContext(HttpListenerContext listenerContext)
        {
            Request = new HttpRequest(listenerContext.Request);
            Response = new HttpResponse(listenerContext.Response);
        }
    }
}
