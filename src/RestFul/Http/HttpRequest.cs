using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
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
        private string[] _strParams;

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

        /// <summary>
        /// Gets a boolean value that indicates whether
        /// the client making the request is authenticated
        /// or not.
        /// </summary>
        public bool IsAuthenticated
        {
            get { return _httpListenerRequest.IsAuthenticated; }
        }

        /// <summary>
        /// Gets a boolean value that indicates whether
        /// the request was sent from a local computer
        /// or not.
        /// </summary>
        public bool IsLocal
        {
            get { return _httpListenerRequest.IsLocal; }
        }

        /// <summary>
        /// Gets a boolean value that indicates whether
        /// the request is using SSL.
        /// </summary>
        public bool IsSecureConnection
        {
            get { return _httpListenerRequest.IsSecureConnection; }
        }

        /// <summary>
        /// Gets a boolean value that indicates that the
        /// connection should be persistent.
        /// </summary>
        public bool KeepAlive
        {
            get { return _httpListenerRequest.KeepAlive; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IPEndPoint LocalEndPoint
        {
            get { return _httpListenerRequest.LocalEndPoint; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Version ProtocolVersion
        {
            get { return _httpListenerRequest.ProtocolVersion; }
        }

        /// <summary>
        /// 
        /// </summary>
        public NameValueCollection QueryString
        {
            get { return _httpListenerRequest.QueryString; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RawUrl
        {
            get { return _httpListenerRequest.RawUrl; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            get { return _httpListenerRequest.RemoteEndPoint; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid RequestTraceIdentifier
        {
            get { return _httpListenerRequest.RequestTraceIdentifier; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ServiceName
        {
            get { return _httpListenerRequest.ServiceName; }
        }

        /// <summary>
        /// Gets an array of the request parameters.
        /// </summary>
        public string[] StrParams
        {
            get
            {
                if (_strParams != null)
                {
                    return _strParams;
                }

                _strParams = _httpListenerRequest.Url.Segments.Skip(1)
                    .Select(p => p.Replace("/", "")).ToArray();

                return _strParams;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TransportContext TransportContext
        {
            get { return _httpListenerRequest.TransportContext; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Uri Url
        {
            get { return _httpListenerRequest.Url; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Uri UriReferrer
        {
            get { return _httpListenerRequest.UrlReferrer; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserAgent
        {
            get { return _httpListenerRequest.UserAgent; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserHostAddress
        {
            get { return _httpListenerRequest.UserHostAddress; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserHostName
        {
            get { return _httpListenerRequest.UserHostName; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] UserLanguages
        {
            get { return _httpListenerRequest.UserLanguages; }
        }
    }
}
