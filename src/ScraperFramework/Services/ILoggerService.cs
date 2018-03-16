using WebScraper.Pocos;

namespace ScraperFramework.Services
{
    interface ILoggerService
    {
        /// <summary>
        /// Logs the crawl result
        /// </summary>
        /// <param name="crawlResult"></param>
        void LogCrawlResult(CrawlResult crawlResult);
    }
}
