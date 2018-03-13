using System;
using System.Threading.Tasks;
using RestFul.Configuration;
using RestFul.Enum;
using RestFul.Exceptions;
using RestFul.Http;
using RestFul.Loggers;
using RestFul.Result;
using RestFul.Routing;
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

        public RestFulServer(IRestFulSettings settings, IRestFulLogger logger, ISerializer serializer,
            IHttpListener httpListener, IRouter router)
        {
            _uriBuilder = new UriBuilder
            {
                Host = settings.Host,
                Port = settings.Port,
                Scheme = (settings.UseHTTPs) ? "https" : "http"
            };

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _httpListener = httpListener ?? throw new ArgumentNullException(nameof(httpListener));
            _router = router ?? throw new ArgumentNullException(nameof(router));
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
                        HttpContext context = await _httpListener.GetContextAsync();
                        try
                        {
                            IResult result = _router.Route(context);
                            result.Execute(context);
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
