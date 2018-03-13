using System;
using System.IO;
using System.Net;
using System.Text;
using HttpStatusCode = RestFul.Enum.HttpStatusCode;

namespace RestFul.Http
{
    public class HttpResponse
    {
        private readonly HttpListenerResponse _httpListenerResponse;

        public HttpResponse(HttpListenerResponse httpListenerResponse)
        {
            _httpListenerResponse = httpListenerResponse;
        }

        /// <summary>
        /// Gets or sets response's content encoding
        /// </summary>
        public Encoding ContentEncoding {
            get { return _httpListenerResponse.ContentEncoding; }
            set { _httpListenerResponse.ContentEncoding = value; }
        }

        /// <summary>
        /// Gets or sets the length of the response's data body
        /// </summary>
        public long ContentLength64 {
            get { return _httpListenerResponse.ContentLength64; }
            set { _httpListenerResponse.ContentLength64 = value; }
        }

        /// <summary>
        /// Gets or sets the response's content type
        /// </summary>
        public string ContentType {
            get { return _httpListenerResponse.ContentType; }
            set { _httpListenerResponse.ContentType = value; }
        }

        /// <summary>
        /// Gets or sets the response's cookies
        /// </summary>
        public CookieCollection Cookies
        {
            get { return _httpListenerResponse.Cookies; }
            set { _httpListenerResponse.Cookies = value; }
        }

        /// <summary>
        /// Gets or sets the collection of headers
        /// </summary>
        public WebHeaderCollection Headers
        {
            get { return _httpListenerResponse.Headers; }
            set { _httpListenerResponse.Headers = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the connection
        /// should be persistent
        /// </summary>
        public bool KeepAlive
        {
            get { return _httpListenerResponse.KeepAlive; }
            set { _httpListenerResponse.KeepAlive = value; }
        }

        /// <summary>
        /// Gets the response's output stream
        /// </summary>
        public Stream OutputStream
        {
            get { return _httpListenerResponse.OutputStream; }
        }

        /// <summary>
        /// Gets or sets the status code to be returned
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get { return (HttpStatusCode)_httpListenerResponse.StatusCode; }
            set { _httpListenerResponse.StatusCode = (int)value; }
        }

        /// <summary>
        /// Adds an HTTP Header to the response
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddHeader(string name, string value)
        {
            _httpListenerResponse.AddHeader(name, value);
        }

        /// <summary>
        /// Adds or updates a Cookie to be sent in the response
        /// </summary>
        /// <param name="cookie"></param>
        public void SetCookie(Cookie cookie)
        {
            _httpListenerResponse.SetCookie(cookie);
        }

        /// <summary>
        /// Closes the connection without sending a response
        /// </summary>
        internal protected void Abort()
        {
            _httpListenerResponse.Abort();
        }

        /// <summary>
        /// Sets the response to redirect the client to the specified
        /// URL
        /// </summary>
        /// <param name="url"></param>
        internal void Redirect(string url)
        {
            _httpListenerResponse.Redirect(url);
        }

        /// <summary>
        /// Sends the response to the client
        /// </summary>
        internal void Send()
        {
            _httpListenerResponse.Close();
        }

        /// <summary>
        /// Sends a response to the client
        /// </summary>
        /// <param name="data"></param>
        internal void SendResponse(byte[] data)
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
    }
}
