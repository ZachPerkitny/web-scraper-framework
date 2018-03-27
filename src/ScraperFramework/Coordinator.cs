using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RestFul;
using Serilog;
using ScraperFramework.Configuration;
using ScraperFramework.Sync;
using ScraperFramework.Utils;

namespace ScraperFramework
{
    class Coordinator : ICoordinator
    {
        private readonly IRestFulServer _restFulServer;
        private readonly IScraperFactory _scraperFactory;
        private readonly ISyncer _syncer;
        private readonly ScraperConfig _config;
        private readonly AsyncManualResetEvent _manualResetEvent;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private readonly List<IScraper> _scrapers = new List<IScraper>();
        private List<Task<Task>> _scraperTasks;

        private bool _disposed = false;

        public Coordinator(IRestFulServer restFulServer, IScraperFactory scraperFactory, ISyncer syncer,
            ScraperConfig config, CancellationTokenSource cancellationTokenSource)
        {
            _restFulServer = restFulServer ?? throw new ArgumentNullException(nameof(restFulServer));
            _scraperFactory = scraperFactory ?? throw new ArgumentNullException(nameof(scraperFactory));
            _syncer = syncer ?? throw new ArgumentNullException(nameof(syncer));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _manualResetEvent = new AsyncManualResetEvent(true);
            _cancellationTokenSource = cancellationTokenSource ?? throw new ArgumentNullException(nameof(cancellationTokenSource));
        }

        public int ScraperCount
        {
            get { return _scrapers.Count; }
        }

        public void Start()
        {
            Log.Information("Starting Rest API");
            _restFulServer.Start();

            Log.Information("Starting Sync Timer");
            _syncer.StartSyncTimer(true);

            Log.Information("Starting Scrapers");
            for (int i = 0; i < _config.Scrapers; i++)
            {
                _scrapers.Add(_scraperFactory.Create(_manualResetEvent, _cancellationTokenSource.Token));
            }

            _scraperTasks = _scrapers.Select(scraper => Task.Factory.StartNew(async () =>
            {
                await scraper.Start();
            }, TaskCreationOptions.LongRunning)).ToList();
        }

        public void Pause()
        {
            _manualResetEvent.Reset();
        }

        public void Resume()
        {
            _manualResetEvent.Set();
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
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
