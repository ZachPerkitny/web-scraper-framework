using System.Collections.Generic;
using ScraperFramework.Data.Entities;
using ScraperFramework.Pocos;

namespace ScraperFramework.Services
{
    interface ICrawlService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="crawlLog"></param>
        void LogCrawl(CrawlLog crawlLog);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="searchTargetId"></param>
        IEnumerable<CrawlDescription> GetKeywordsToCrawl(int count);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<SearchTarget> GetSearchTargets();
    }
}
