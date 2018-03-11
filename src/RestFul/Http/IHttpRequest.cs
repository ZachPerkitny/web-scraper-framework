using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using RestFul.Enum;

namespace RestFul.Http
{
    public interface IHttpRequest
    {
        /// <summary>
        /// Gets the MIME Types accepted by the client
        /// </summary>
        string[] AcceptTypes { get; }

        /// <summary>
        /// 
        /// </summary>
        int ClientCertificateError { get; }

        /// <summary>
        /// Gets the content encoding
        /// </summary>
        Encoding ContentEncoding { get; }

        /// <summary>
        /// Gets the length of the data body
        /// </summary>
        long ContentLength64 { get; }

        /// <summary>
        /// Gets the MIME Type of the data body
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Gets the data body
        /// </summary>
        /// <returns></returns>
        string DataBody { get; }

        /// <summary>
        /// Gets the cookies sent with the request
        /// </summary>
        CookieCollection Cookies { get; }

        /// <summary>
        /// Indicates whether the request has a data body
        /// </summary>
        bool HasEntityBody { get; }

        /// <summary>
        /// Gets header key/value pairs
        /// </summary>
        NameValueCollection Headers { get; }

        /// <summary>
        /// Gets the HTTP Method Specified by the client
        /// </summary>
        HttpMethod HttpMethod { get; }

        /// <summary>
        /// Gets a stream that contains the data body
        /// </summary>
        Stream InputStream { get; }

        /// <summary>
        /// Returns the request path
        /// </summary>
        string Path { get; }
    }
}
