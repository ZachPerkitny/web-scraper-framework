using System;
using System.Threading.Tasks;
using RestFul.Configuration;
using RestFul.Enum;
using RestFul.Exceptions;
using RestFul.Http;
using RestFul.Http.Concrete;
using RestFul.Loggers;
using RestFul.Routing;
using RestFul.Routing.Concrete;
using RestFul.Serializer;

namespace RestFul
{
    public class RestFulServer : IRestFulServer
    {
        public bool IsListening
        {
            get { return _httpListener.IsListening; }
        }

        private readonly UriBuilder _uriBuilder;

        private readonly IRestFulLogger _logger;
        private readonly ISerializer _serializer;
        private readonly IHttpListener _httpListener;
        private readonly IRouter _router;
        private bool _starting;
        private Task _httpListenerTask;

        public static IRestFulServer Create(Action<IRestFulSettings> configure)
        {
            IRestFulSettings settings = new RestFulSettings();
            configure?.Invoke(settings);
            return new RestFulServer(settings);
        }

        public RestFulServer(IRestFulSettings settings)
        {
            _uriBuilder = new UriBuilder
            {
                Host = settings.Host,
                Port = settings.Port,
                Scheme = (settings.UseHTTPs) ? "https" : "http"
            };

            _logger = settings.Logger;
            _serializer = settings.Serializer;
            _httpListener = new HttpListener();
            _router = new Router(_logger);
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
                    while (IsListening)
                    {
                        IHttpContext context = await _httpListener.GetContextAsync();
                        try
                        {
                            _router.Route(context);
                        }
                        catch (APIException ex)
                        {
                            context.Response.StatusCode = ex.StatusCode;
                            byte[] response = _serializer.Serialize(new
                            {
                                ex.Detail,
                                ex.StatusCode
                            });
                            context.Response.SendResponse(response);
                        }
                        catch (Exception)
                        {
                            context.Response.StatusCode = HttpStatusCode.InternalServerError;
                            context.Response.Send();
                        }
                    }
                }, TaskCreationOptions.LongRunning);
            }
            catch (Exception ex)
            {
                _logger.Error($"RestFulServer: {ex.Message}\r\n{ex.StackTrace}");
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
