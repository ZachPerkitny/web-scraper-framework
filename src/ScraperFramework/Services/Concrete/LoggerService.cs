using ScraperFramework.Data;
using ScraperFramework.Data.Entities;
using WebScraper.Pocos;

namespace ScraperFramework.Services.Concrete
{
    class LoggerService : ILoggerService
    {
        private readonly ICrawlLogRepo _crawlLogRepo;

        public LoggerService(ICrawlLogRepo crawlLogRepo)
        {
            _crawlLogRepo = crawlLogRepo;
        }

        public void LogCrawlResult(CrawlResult crawlResult)
        {
            CrawlLog crawlLog = new CrawlLog
            {
                KeywordID = crawlResult.KeywordID,
                SearchTargetID = crawlResult.SearchTargetID,
                EndpointID = crawlResult.EndpointID,
                CrawlResultID = crawlResult.CrawlResultID
            };

            //TODO (zvp): Do Something with the ads

            _crawlLogRepo.Insert(crawlLog);
        }
    }
}
