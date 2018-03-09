using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

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

            Log.Information("HttpListener Started...");
            _httpListener.Start();
            _listenerTask = Task.Factory.StartNew(async () =>
            {
                while (!cancelToken.IsCancellationRequested)
                {
                    HttpListenerContext ctx = await _httpListener.GetContextAsync();

                    Log.Information("Command Listener Recieved Request.");
                    HttpListenerRequest request = ctx.Request;

                    string httpMethod = request.Url.Segments[1];

                    Log.Information(httpMethod);

                }
            }, TaskCreationOptions.LongRunning);
        }
    }
}
