using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestFul.Http;
using RestFul.Http.Concrete;
using RestFul.Routing;
using RestFul.Routing.Concrete;

namespace RestFul
{
    public class RestFulServer : IRestFulServer
    {
        public bool IsListening
        {
            get { return _httpListener.IsListening; }
        }

        private readonly UriBuilder _uriBuilder = new UriBuilder("http", "localhost", 8000);

        private readonly IRestFulSettings _settings;
        private readonly IHttpListener _httpListener;
        private readonly IRouter _router;
        private bool _starting;
        private Task _httpListenerTask;

        public RestFulServer(Action<IRestFulSettings> configure)
        {
            _settings = new RestFulSettings();
            configure?.Invoke(_settings);

            _uriBuilder.Host = _settings.Host;
            _uriBuilder.Port = _settings.Port;
            _uriBuilder.Scheme = (_settings.UseHTTPs) ? "https" : "http";

            _httpListener = new HttpListener();
            _router = new Router(_settings.Logger);
        }

        public void Start()
        {
            if (IsListening || _starting)
            {
                return;
            }

            _starting = true;

            try
            {
                if (_router.Routes.Count == 0)
                {
                    _router.ScanAssemblies();
                }
                
                _httpListener.Prefixes.Clear();
                _httpListener.Prefixes.Add(_uriBuilder.Uri.ToString());
                _httpListener.Start();
                _httpListenerTask = Task.Factory.StartNew(async () =>
                {
                    HashSet<Task<IHttpContext>> requests = new HashSet<Task<IHttpContext>>();
                    for (int i = 0; i < _settings.ConcurrentRequests; i++)
                    {
                        requests.Add(_httpListener.GetContextAsync());
                    }

                    while (IsListening)
                    {
                        Task<IHttpContext> request = await Task.WhenAny(requests);
                        requests.Remove(request);

                        IHttpContext httpContext = request.Result;
                        _router.Route(httpContext);

                        requests.Add(_httpListener.GetContextAsync());
                    }
                }, TaskCreationOptions.LongRunning);
            }
            catch (Exception)
            {

            }
            finally
            {
                _starting = false;
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
