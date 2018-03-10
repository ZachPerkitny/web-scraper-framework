using System.Net;

namespace RestAPI.Http
{
    class HttpContext : IHttpContext
    {
        public IHttpRequest Request { get; private set; }

        public IHttpResponse Response { get; private set; }

        public HttpContext(HttpListenerContext listenerContext)
        {
            //Request = listenerContext.Request;
            //Response = listenerContext.Response;
        }
    }
}
