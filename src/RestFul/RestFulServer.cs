using System;
using System.Threading.Tasks;
using RestFul.Configuration;
using RestFul.Enum;
using RestFul.Http;
using RestFul.Loggers;
using RestFul.Result;
using RestFul.Routing;
using RestFul.Serializer;

namespace RestFul
{
    public class RestFulServer : IRestFulServer
    {
        private readonly UriBuilder _uriBuilder;

        private readonly IRestFulLogger _logger;
        private readonly ISerializer _serializer;
        private readonly IHttpListener _httpListener;
        private readonly IRouter _router;
        private bool _starting;
        private bool _hasRun;
        //private bool _stopping;
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

        public bool IsListening
        {
            get { return _httpListener.IsListening; }
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
                if (!_hasRun)
                {
                    _router.Initialize();
                    _httpListener.Prefixes.Add(_uriBuilder.Uri.ToString());
                }

                _httpListener.Start();
                _httpListenerTask = Task.Factory.StartNew(async () =>
                {
                    while (IsListening)
                    {
                        HttpContext context = await _httpListener.GetContextAsync();
                        _logger.Information("Received Request {0} {1}", context.Request.HttpMethod,
                            context.Request.Path);
                        try
                        {
                            IResult result = _router.Route(context);
                            result.Execute(context);
                        }
                        catch (Exception ex)
                        {
                            context.Response.StatusCode = HttpStatusCode.InternalServerError;
                            context.Response.Send();
                            _logger.Error("Internal Server Error ({0}): {1}", ex.GetType(), ex.Message);
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
                _hasRun = true;
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
