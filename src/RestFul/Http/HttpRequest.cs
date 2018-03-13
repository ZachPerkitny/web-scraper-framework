using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using RestFul.Enum;
using RestFul.Extensions;

namespace RestFul.Http
{
    public class HttpRequest
    {
        /// <summary>
        /// Returns the request path
        /// </summary>
        public string Path { get; private set; }

        private readonly HttpListenerRequest _httpListenerRequest;
        private string _data;
        private bool _convertedHttpMethod;
        private HttpMethod _httpMethod;

        public HttpRequest(HttpListenerRequest httpListenerRequest)
        {
            _httpListenerRequest = httpListenerRequest;
            Path = httpListenerRequest.RawUrl.Split(new[] { '?' }, 2)[0];
        }

        /// <summary>
        /// Gets the MIME Types accepted by the client
        /// </summary>
        public string[] AcceptTypes
        {
            get { return _httpListenerRequest.AcceptTypes; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ClientCertificateError
        {
            get { return _httpListenerRequest.ClientCertificateError; }
        }

        /// <summary>
        /// Gets the content encoding
        /// </summary>
        public Encoding ContentEncoding
        {
            get { return _httpListenerRequest.ContentEncoding; }
        }

        /// <summary>
        /// Gets the length of the data body
        /// </summary>
        public long ContentLength64
        {
            get { return _httpListenerRequest.ContentLength64; }
        }

        /// <summary>
        /// Gets the MIME Type of the data body
        /// </summary>
        public string ContentType
        {
            get { return _httpListenerRequest.ContentType; }
        }

        /// <summary>
        /// Gets the cookies sent with the request
        /// </summary>
        public CookieCollection Cookies
        {
            get { return _httpListenerRequest.Cookies; }
        }

        /// <summary>
        /// Gets the data body
        /// </summary>
        /// <returns></returns>
        public string DataBody
        {
            get
            {
                if (_data != null)
                {
                    return _data;
                }

                using (StreamReader reader = new StreamReader(InputStream, ContentEncoding))
                {
                    _data = reader.ReadToEnd();
                }

                return _data;
            }
        }

        /// <summary>
        /// Indicates whether the request has a data body
        /// </summary>
        public bool HasEntityBody
        {
            get { return _httpListenerRequest.HasEntityBody; }
        }

        /// <summary>
        /// Gets header key/value pairs
        /// </summary>
        public NameValueCollection Headers
        {
            get { return _httpListenerRequest.Headers; }
        }

        /// <summary>
        /// Gets the HTTP Method Specified by the client
        /// </summary>
        public HttpMethod HttpMethod
        {
            get
            {
                if (_convertedHttpMethod)
                {
                    return _httpMethod;
                }

                _httpMethod = _httpListenerRequest.HttpMethod.ToHttpMethod();
                _convertedHttpMethod = true;
                return _httpMethod;
            }
        }

        /// <summary>
        /// Gets a stream that contains the data body
        /// </summary>
        public Stream InputStream
        {
            get { return _httpListenerRequest.InputStream; }
        }
    }
}
