using ScraperFramework.Shared.Pocos;

namespace ScraperFramework
{
    public interface ICrawlLogger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="crawlResult"></param>
        void LogCrawl(CrawlResult crawlResult);
    }
}
