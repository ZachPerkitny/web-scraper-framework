using System;
using System.Net;
using ScraperFramework.Pocos;

namespace ScraperFramework.Exceptions
{
    abstract class HttpServerException : Exception
    {
        public HttpServerExceptionDesc ExceptionDesc { get; }

        public HttpServerException(string detail, HttpStatusCode statusCode)
        {
            ExceptionDesc = new HttpServerExceptionDesc
            {
                Detail = detail,
                StatusCode = statusCode
            };
        }
    }

    class BadRequest : HttpServerException
    {
        public BadRequest()
            : base("Bad Request.", HttpStatusCode.BadRequest) { }

        public BadRequest(string detail)
            : base(detail, HttpStatusCode.BadRequest) { }

        public BadRequest(string detail, params object[] format)
            : this(string.Format(detail, format)) { }
    }

    class NotFound : HttpServerException
    {
        public NotFound()
            : base("Not Found.", HttpStatusCode.NotFound) { }

        public NotFound(string detail)
            : base(detail, HttpStatusCode.NotFound) { }

        public NotFound(string detail, params object[] format)
            : this(string.Format(detail, format)) { }
    }

    class InternalServerError : HttpServerException
    {
        public InternalServerError()
            : base("Internal Server Error.", HttpStatusCode.InternalServerError) { }

        public InternalServerError(string detail)
            : base(detail, HttpStatusCode.InternalServerError) { }

        public InternalServerError(string detail, params object[] format)
            : this(string.Format(detail, format)) { }
    }
}
