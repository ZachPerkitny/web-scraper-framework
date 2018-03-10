using System.IO;
using System.Net;
using System.Text;
using HttpStatusCode = RestAPI.Enum.HttpStatusCode;

namespace RestAPI.Http
{
    public interface IHttpResponse
    {
        /// <summary>
        /// Gets or sets response's content encoding
        /// </summary>
        Encoding ContentEncoding { get; set; }

        /// <summary>
        /// Gets or sets the length of the response's data body
        /// </summary>
        long ContentLength64 { get; set; }

        /// <summary>
        /// Gets or sets the response's content type
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the response's cookies
        /// </summary>
        CookieCollection Cookies { get; set; }

        /// <summary>
        /// Gets or sets the collection of headers
        /// </summary>
        WebHeaderCollection Headers { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the connection
        /// should be persistent
        /// </summary>
        bool KeepAlive { get; set; }

        /// <summary>
        /// Gets the response's output stream
        /// </summary>
        Stream OutputStream { get; }

        /// <summary>
        /// Gets or sets the status code to be returned
        /// </summary>
        HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Closes the connection without sending a response
        /// </summary>
        void Abort();

        /// <summary>
        /// Adds an HTTP Header to the response
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void AddHeader(string name, string value);

        /// <summary>
        /// Sends the response to the client
        /// </summary>
        void Send();

        /// <summary>
        /// Sends a response to the client
        /// </summary>
        /// <param name="data"></param>
        void SendResponse(byte[] data);

        /// <summary>
        /// Sets the response to redirect the client to the specified
        /// URL
        /// </summary>
        /// <param name="url"></param>
        void Redirect(string url);

        /// <summary>
        /// Adds or updates a Cookie to be sent in the response
        /// </summary>
        /// <param name="cookie"></param>
        void SetCookie(Cookie cookie);
    }
}
