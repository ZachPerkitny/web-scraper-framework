using ScraperFramework.Shared.Pocos;

namespace ScraperFramework
{
    public interface ICrawlLogger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="crawlDescription"></param>
        /// <param name="crawlResult"></param>
        void LogCrawl(CrawlDescription crawlDescription, CrawlResult crawlResult);
    }
}
