using System;
using System.Threading;
using ScraperFramework.Utils;

namespace ScraperFramework
{
    class ScraperFactory : IScraperFactory
    {
        private readonly IScraperQueue _scraperQueue;
        private readonly ICrawlLogger _crawlLogger;

        public ScraperFactory(IScraperQueue scraperQueue, ICrawlLogger crawlLogger)
        {
            _scraperQueue = scraperQueue ?? throw new ArgumentNullException(nameof(scraperQueue));
            _crawlLogger = crawlLogger ?? throw new ArgumentNullException(nameof(crawlLogger));
        }

        public IScraper Create(AsyncManualResetEvent manualResetEvent, CancellationToken cancellationToken)
        {
            return new Scraper(_scraperQueue, _crawlLogger, manualResetEvent, cancellationToken);
        }
    }
}
