using System.Threading.Tasks;
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
        Task LogCrawl(CrawlDescription crawlDescription, CrawlResult crawlResult);
    }
}
