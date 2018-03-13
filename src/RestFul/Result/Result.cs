using System;
using RestFul.Enum;
using RestFul.Http;

namespace RestFul.Result
{
    public class Result : IResult
    {
        public HttpStatusCode HttpStatusCode { get; }

        public object Response { get; }

        public Result(HttpStatusCode httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
        }

        public Result(HttpStatusCode httpStatusCode, object response)
        {
            HttpStatusCode = httpStatusCode;
            Response = response;
        }

        public void Execute(IHttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.Response.StatusCode = HttpStatusCode;

            if (Response != null)
            {
                
            }
        }
    }
}
