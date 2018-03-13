using System;
using RestFul.Enum;
using RestFul.Http;

namespace RestFul.Result
{
    public class EmptyResult : Result
    {
        /// <summary>
        /// Gets or sets the status code to be
        /// sent back to the client.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }

        public EmptyResult() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpStatusCode"></param>
        public EmptyResult(HttpStatusCode httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
        }

        public override void Execute(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (HttpStatusCode != HttpStatusCode.Undefined)
            {
                context.Response.StatusCode = HttpStatusCode;
            }

            context.Response.Send();
        }
    }
}
