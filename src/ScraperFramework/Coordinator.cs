using System;
using System.Threading;
using RestFul;
using Serilog;
using ScraperFramework.Configuration;

namespace ScraperFramework
{
    class Coordinator : ICoordinator
    {
        private readonly IScraperQueue _scraperQueue;
        private readonly IRestFulServer _restFulServer;
        private readonly ScraperConfig _config;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private bool _disposed = false;

        public Coordinator(IScraperQueue scraperQueue, IRestFulServer restFulServer, ScraperConfig config, 
            CancellationTokenSource cancellationTokenSource)
        {
            _scraperQueue = scraperQueue ?? throw new ArgumentNullException(nameof(scraperQueue));
            _restFulServer = restFulServer ?? throw new ArgumentNullException(nameof(restFulServer));
            _config = config;
            _cancellationTokenSource = cancellationTokenSource ?? throw new ArgumentNullException(nameof(cancellationTokenSource));
        }

        public void Start()
        {
            Log.Information("Starting Rest API");
            _restFulServer.Start();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _cancellationTokenSource.Cancel();
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
