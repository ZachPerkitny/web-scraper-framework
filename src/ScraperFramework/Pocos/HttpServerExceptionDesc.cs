using System;
using System.Net;

namespace ScraperFramework.Pocos
{
    class HttpServerExceptionDesc
    {
        public string Detail { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
