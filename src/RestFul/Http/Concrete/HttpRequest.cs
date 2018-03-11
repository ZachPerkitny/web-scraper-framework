using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using RestFul.Enum;
using RestFul.Extensions;

namespace RestFul.Http.Concrete
{
    class HttpRequest : IHttpRequest
    {
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

        public string[] AcceptTypes
        {
            get { return _httpListenerRequest.AcceptTypes; }
        }

        public int ClientCertificateError
        {
            get { return _httpListenerRequest.ClientCertificateError; }
        }

        public Encoding ContentEncoding
        {
            get { return _httpListenerRequest.ContentEncoding; }
        }

        public long ContentLength64
        {
            get { return _httpListenerRequest.ContentLength64; }
        }

        public string ContentType
        {
            get { return _httpListenerRequest.ContentType; }
        }

        public CookieCollection Cookies
        {
            get { return _httpListenerRequest.Cookies; }
        }

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

        public bool HasEntityBody
        {
            get { return _httpListenerRequest.HasEntityBody; }
        }

        public NameValueCollection Headers
        {
            get { return _httpListenerRequest.Headers; }
        }

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

        public Stream InputStream
        {
            get { return _httpListenerRequest.InputStream; }
        }
    }
}
