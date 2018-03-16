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
        private readonly IRestFulServer _restFulServer;
        private readonly IScraperFactory _scraperFactory;
        private readonly ScraperConfig _config;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private readonly List<IScraper> _scrapers = new List<IScraper>();
        private List<Task<Task>> _scraperTasks;

        private bool _disposed = false;

        public Coordinator(IRestFulServer restFulServer, IScraperFactory scraperFactory, ScraperConfig config, 
            CancellationTokenSource cancellationTokenSource)
        {
            _restFulServer = restFulServer ?? throw new ArgumentNullException(nameof(restFulServer));
            _scraperFactory = scraperFactory ?? throw new ArgumentNullException(nameof(scraperFactory));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _cancellationTokenSource = cancellationTokenSource ?? throw new ArgumentNullException(nameof(cancellationTokenSource));
        }

        public void Start()
        {
            Log.Information("Starting Rest API");
            _restFulServer.Start();

            Log.Information("Starting Scrapers");
            for (int i = 0; i < _config.Scrapers; i++)
            {
                _scrapers.Add(_scraperFactory.Create(_cancellationTokenSource.Token));
            }

            _scraperTasks = _scrapers.Select(scraper => Task.Factory.StartNew(async () =>
            {
                await scraper.Start();
            }, TaskCreationOptions.LongRunning)).ToList();
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
