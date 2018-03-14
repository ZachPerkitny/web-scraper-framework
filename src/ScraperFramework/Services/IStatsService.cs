using ScraperFramework.Pocos;

namespace ScraperFramework.Services
{
    public interface IStatsService
    {
        /// <summary>
        /// 
        /// </summary>
        CrawlStats GetCrawlStats();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTargetId"></param>
        CrawlStats GetCrawlStatsForSearchTarget(int searchTargetId);
    }
}
