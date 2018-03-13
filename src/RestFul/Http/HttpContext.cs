using System;
using System.Net;
using RestFul.Serializer;

namespace RestFul.Http
{
    public class HttpContext
    {
        /// <summary>
        /// Gets an object that represents the client's request
        /// </summary>
        public HttpRequest Request { get; private set; }

        /// <summary>
        /// Gets an object that will be sent back to the client
        /// </summary>
        public HttpResponse Response { get; private set; }

        /// <summary>
        /// Serializer used to serialize responses
        /// </summary>
        public ISerializer Serializer { get; private set; }

        /// <summary>
        /// Initializes a new HttpContext object with context from the listener
        /// and the Serializer to use in the Result Excecutions.
        /// </summary>
        /// <param name="listenerContext"></param>
        /// <param name="serializer"></param>
        public HttpContext(HttpListenerContext listenerContext, ISerializer serializer)
        {
            Request = new HttpRequest(listenerContext.Request);
            Response = new HttpResponse(listenerContext.Response);
            Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }
    }
}
