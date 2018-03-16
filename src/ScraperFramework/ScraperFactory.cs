using System;
using System.Threading;
using ScraperFramework.Services;

namespace ScraperFramework
{
    class ScraperFactory : IScraperFactory
    {
        private readonly ILoggerService _loggerService;
        private readonly IScraperQueue _scraperQueue;

        public ScraperFactory(ILoggerService loggerService, IScraperQueue scraperQueue)
        {
            _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
            _scraperQueue = scraperQueue ?? throw new ArgumentNullException(nameof(scraperQueue));
        }

        public IScraper Create(CancellationToken cancellationToken)
        {
            return new Scraper(_loggerService, _scraperQueue, cancellationToken);
        }
    }
}
