using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RestFul;
using Serilog;
using ScraperFramework.Configuration;

namespace ScraperFramework
{
    class Coordinator : ICoordinator
    {
        private readonly List<IScraper> _scrapers;
        private List<Task<Task>> _scraperTasks;
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

            Log.Information("Starting Scrapers");
            for (int i = 0; i < _config.Scrapers; i++)
            {
                _scrapers.Add(new Scraper(_cancellationTokenSource.Token));
            }

            _scraperTasks = _scrapers.Select(scraper => Task.Factory.StartNew(async () =>
            {
                await scraper.Start();
            })).ToList();
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
