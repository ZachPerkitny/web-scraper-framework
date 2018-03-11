using System;
using System.Net;
using System.Threading.Tasks;

namespace RestFul.Http.Concrete
{
    class HttpListener : IHttpListener
    {
        private readonly System.Net.HttpListener _listener;

        public HttpListener()
        {
            _listener = new System.Net.HttpListener();
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

        public IHttpContext EndGetContext(IAsyncResult asyncResult)
        {
            HttpListenerContext listenerContext = _listener.EndGetContext(asyncResult);

            return new HttpContext(listenerContext);
        }

        public IHttpContext GetContext()
        {
            HttpListenerContext listenerContext = _listener.GetContext();

            return new HttpContext(listenerContext);
        }

        public async Task<IHttpContext> GetContextAsync()
        {
            HttpListenerContext listenerContext = await _listener.GetContextAsync();

            return new HttpContext(listenerContext);
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
