using System;
using System.Collections.Generic;
using ScraperFramework.Data.Entities;
using WebScraper.Pocos;

namespace ScraperFramework.Services
{
    interface ICrawlService
    {
        /// <summary>
        /// Datetime whose value is the next time 
        /// an endpoint or keyword is available to
        /// be used in a crawl, or crawled.
        /// </summary>
        DateTime NextAvailability { get; }

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
