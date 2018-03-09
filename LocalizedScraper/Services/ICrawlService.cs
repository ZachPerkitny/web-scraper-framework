using System;
using System.Collections.Generic;
using ScraperFramework.Data.Entities;

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
        IEnumerable<Keyword> GetKeywordsToCrawl(int count, int searchTargetId);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<SearchTarget> GetSearchTargets();
    }
}
