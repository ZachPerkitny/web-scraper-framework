using System;
using System.Threading;
using Serilog;

namespace ScraperFramework
{
    class Controller : IController
    {
        private readonly IHttpServer _httpServer;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private bool _disposed = false;

        public Controller(IHttpServer httpServer, CancellationTokenSource cancellationTokenSource)
        {
            _httpServer = httpServer ?? throw new ArgumentNullException(nameof(httpServer));
            _cancellationTokenSource = cancellationTokenSource ?? throw new ArgumentNullException(nameof(cancellationTokenSource));
        }

        public void Start()
        {
            Log.Information("Starting Command Listener");
            _httpServer.Listen(new string[]
            {
                "http://localhost:5000/"
            }, _cancellationTokenSource.Token);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _cancellationTokenSource.Cancel();
                    _httpServer.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
