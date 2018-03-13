using System;
using System.Net;
using System.Threading.Tasks;
using RestFul.Serializer;

namespace RestFul.Http
{
    class HttpListener : IHttpListener
    {
        private readonly System.Net.HttpListener _listener;
        private readonly ISerializer _serializer;

        public HttpListener(ISerializer serializer)
        {
            // TODO (zvp): Authentication ?
            _listener = new System.Net.HttpListener();
            _serializer = serializer;
        }

        public bool IsListening
        {
            get { return _listener.IsListening;  }
        }

        public HttpListenerPrefixCollection Prefixes
        {
            get { return _listener.Prefixes; }
        }

        public void Abort()
        {
            _listener.Abort();
        }

        public void BeginGetContext(AsyncCallback callback, object state)
        {
            _listener.BeginGetContext(callback, state);
        }

        public void Close()
        {
            _listener.Close();
        }

        public HttpContext EndGetContext(IAsyncResult asyncResult)
        {
            HttpListenerContext listenerContext = _listener.EndGetContext(asyncResult);

            return new HttpContext(listenerContext, _serializer);
        }

        public HttpContext GetContext()
        {
            HttpListenerContext listenerContext = _listener.GetContext();

            return new HttpContext(listenerContext, _serializer);
        }

        public async Task<HttpContext> GetContextAsync()
        {
            HttpListenerContext listenerContext = await _listener.GetContextAsync();

            return new HttpContext(listenerContext, _serializer);
        }

        public void Start()
        {
            _listener.Start();
        }

        public void Stop()
        {
            _listener.Stop();
        }
    }
}
