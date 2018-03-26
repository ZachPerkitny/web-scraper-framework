using System;
using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    public interface IKeywordScrapeDetailRepo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywordScrapeDetail"></param>
        void Insert(KeywordScrapeDetail keywordScrapeDetail);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywordScrapeDetails"></param>
        void InsertMany(IEnumerable<KeywordScrapeDetail> keywordScrapeDetails);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <param name="regionId"></param>
        /// <param name="cityId"></param>
        /// <param name="keywordId"></param>
        /// <returns></returns>
        KeywordScrapeDetail Select(int searchEngineId, int regionId, int cityId, int keywordId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <param name="regionId"></param>
        /// <param name="cityId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IEnumerable<KeywordScrapeDetail> SelectMany(int searchEngineId, int regionId, int cityId, int count);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <param name="regionId"></param>
        /// <param name="cityId"></param>
        /// <param name="keywordId"></param>
        /// <param name="lastCrawl"></param>
        void UpdateLastCrawl(int searchEngineId, int regionId, int cityId, int keywordId, DateTime lastCrawl);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ulong Count();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        KeywordScrapeDetail Max();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        KeywordScrapeDetail Min();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        byte[] GetLatestRevision();
    }
}
