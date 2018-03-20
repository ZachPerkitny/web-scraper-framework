using System;
using System.Threading;
using ScraperFramework.Utils;

namespace ScraperFramework
{
    class ScraperFactory : IScraperFactory
    {
        private readonly IScraperQueue _scraperQueue;

        public ScraperFactory(IScraperQueue scraperQueue)
        {
            _scraperQueue = scraperQueue ?? throw new ArgumentNullException(nameof(scraperQueue));
        }

        public IScraper Create(AsyncManualResetEvent manualResetEvent, CancellationToken cancellationToken)
        {
            return new Scraper(_scraperQueue, manualResetEvent, cancellationToken);
        }
    }
}
