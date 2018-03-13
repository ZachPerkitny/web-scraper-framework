using System;
using RestFul.Enum;
using RestFul.Http;
using RestFul.Serializer;

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

        public void Execute(IHttpContext context, ISerializer serializer)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.Response.StatusCode = HttpStatusCode;

            if (Response != null)
            {
                byte[] serializedResponse = serializer.Serialize(Response);
                context.Response.SendResponse(serializedResponse);
            }
            else
            {
                context.Response.Send();
            }
        }
    }
}
