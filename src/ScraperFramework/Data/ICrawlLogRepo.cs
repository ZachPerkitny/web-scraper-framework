using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    public interface ICrawlLogRepo
    {
        /// <summary>
        /// Inserts a new crawl log
        /// </summary>
        /// <param name="log"></param>
        void Insert(CrawlLog log);

        /// <summary>
        /// Selects a crawl log by id
        /// </summary>
        /// <param name="id"></param>
        CrawlLog Select(int id);

        /// <summary>
        /// Selects multiple crawl logs by searchTargetId and keywordId
        /// </summary>
        /// <param name="searchTargetId"></param>
        /// <param name="keywordId"></param>
        IEnumerable<CrawlLog> SelectMany(int searchTargetId, int keywordId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ulong Count();
    }
}
