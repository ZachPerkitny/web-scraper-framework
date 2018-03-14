using System;
using ScraperFramework.Data;
using System.Linq;
using ScraperFramework.Pocos;

namespace ScraperFramework.Services.Concrete
{
    class StatsService : IStatsService
    {
        private readonly ICrawlLogRepo _crawlLogRepo;

        public StatsService(ICrawlLogRepo crawlLogRepo)
        {
            _crawlLogRepo = crawlLogRepo ?? throw new ArgumentNullException(nameof(crawlLogRepo));
        }

        public CrawlStats GetCrawlStats()
        {
            return new CrawlStats
            {
                CrawlCount = _crawlLogRepo.Count()
            };

        }

        public CrawlStats GetCrawlStatsForSearchTarget(int searchTargetId)
        {
            return new CrawlStats
            {
                // TODO (zvp): this seems inefficient
                CrawlCount = Convert.ToUInt64(_crawlLogRepo.SelectMany(searchTargetId).Count())
            };
        }
    }
}
