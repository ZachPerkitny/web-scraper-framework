using System;
using RestFul.Enum;

namespace RestFul.Exceptions
{
    class APIException : Exception
    {
        public string Detail { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public APIException(string detail, HttpStatusCode statusCode)
        {
            Detail = detail;
            StatusCode = statusCode;
        }
    }
}
