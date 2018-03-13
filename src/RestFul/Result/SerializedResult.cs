using System;
using RestFul.Enum;
using RestFul.Http;

namespace RestFul.Result
{
    public class SerializedResult : Result
    {
        /// <summary>
        /// Gets or sets the object to be serialized and
        /// sent as a response to the client.
        /// </summary>
        public object Response { get; set; }

        /// <summary>
        /// Gets or sets the status code to be
        /// sent back to the client.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        public SerializedResult(object response)
        {
            Response = response;
        }

        /// <summary>
        /// Initializes a SerializedResult with the provided response
        /// object and status code.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="httpStatusCode"></param>
        public SerializedResult(object response, HttpStatusCode httpStatusCode)
            : this(response)
        {
            HttpStatusCode = httpStatusCode;
        }

        public override void Execute(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // it would appear that setting it to an invalid status
            // code just sends a 200 response.
            if (HttpStatusCode != HttpStatusCode.Undefined)
            {
                context.Response.StatusCode = HttpStatusCode;
            }

            byte[] serializedResponse = context.Serializer.Serialize(Response);
            context.Response.SendResponse(serializedResponse);
        }
    }
}
