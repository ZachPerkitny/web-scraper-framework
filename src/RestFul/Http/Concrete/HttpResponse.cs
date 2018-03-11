using System;
using System.IO;
using System.Net;
using System.Text;
using HttpStatusCode = RestFul.Enum.HttpStatusCode;

namespace RestFul.Http.Concrete
{
    class HttpResponse : IHttpResponse
    {
        private readonly HttpListenerResponse _httpListenerResponse;

        public HttpResponse(HttpListenerResponse httpListenerResponse)
        {
            _httpListenerResponse = httpListenerResponse;
        }

        public Encoding ContentEncoding {
            get { return _httpListenerResponse.ContentEncoding; }
            set { _httpListenerResponse.ContentEncoding = value; }
        }

        public long ContentLength64 {
            get { return _httpListenerResponse.ContentLength64; }
            set { _httpListenerResponse.ContentLength64 = value; }
        }

        public string ContentType {
            get { return _httpListenerResponse.ContentType; }
            set { _httpListenerResponse.ContentType = value; }
        }

        public CookieCollection Cookies
        {
            get { return _httpListenerResponse.Cookies; }
            set { _httpListenerResponse.Cookies = value; }
        }

        public WebHeaderCollection Headers
        {
            get { return _httpListenerResponse.Headers; }
            set { _httpListenerResponse.Headers = value; }
        }

        public bool KeepAlive
        {
            get { return _httpListenerResponse.KeepAlive; }
            set { _httpListenerResponse.KeepAlive = value; }
        }

        public HttpStatusCode StatusCode
        {
            get { return (HttpStatusCode)_httpListenerResponse.StatusCode; }
            set { _httpListenerResponse.StatusCode = (int)value; }
        }

        public Stream OutputStream
        {
            get { return _httpListenerResponse.OutputStream; }
        }

        public void Abort()
        {
            _httpListenerResponse.Abort();
        }

        public void AddHeader(string name, string value)
        {
            _httpListenerResponse.AddHeader(name, value);
        }

        public void Redirect(string url)
        {
            _httpListenerResponse.Redirect(url);
        }

        public void Send()
        {
            _httpListenerResponse.Close();
        }

        public void SendResponse(byte[] data)
        {
            try
            {
                ContentLength64 = data.Length;
                OutputStream.Write(data, 0, data.Length);
                OutputStream.Close();
            }
            catch(Exception)
            {
                OutputStream.Dispose();
                throw;
            }
            finally
            {
                Send();
            }
        }

        public void SetCookie(Cookie cookie)
        {
            _httpListenerResponse.SetCookie(cookie);
        }
    }
}
