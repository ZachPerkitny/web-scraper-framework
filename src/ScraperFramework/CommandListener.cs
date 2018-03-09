using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using ScraperFramework.Attributes;

namespace ScraperFramework
{
    class CommandListener : ICommandListener
    {
        private readonly HttpListener _httpListener;
        private Task _listenerTask;

        public CommandListener()
        {
            _httpListener = new HttpListener();
        }

        public void Listen(string[] prefixes, CancellationToken cancelToken)
        {
            if (prefixes == null || prefixes.Length == 0)
            {
                throw new ArgumentException("URL Prefixes are Required");
            }

            foreach (string prefix in prefixes)
            {
                _httpListener.Prefixes.Add(prefix);
            }

            _httpListener.Start();
            _listenerTask = Task.Factory.StartNew(async () =>
            {
                while (!cancelToken.IsCancellationRequested)
                {
                    Log.Information("Command Listener Waiting For Requests");
                    HttpListenerContext ctx = await _httpListener.GetContextAsync();
                    
                    HttpListenerRequest request = ctx.Request;
                    Log.Information("Command Listener Recieved Request {0} {1}", request.HttpMethod, request.Url.Segments[1]);

                    string httpMethod = request.Url.Segments[1];

                    Log.Information(httpMethod);

                }
            }, TaskCreationOptions.LongRunning);
        }

        [Command("GET", "/stats")]
        private void GetStats()
        {
            Log.Information("Getting Stats");
        }
    }
}
