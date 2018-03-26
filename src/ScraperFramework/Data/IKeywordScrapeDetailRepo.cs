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
        KeywordScrapeDetail Select(short searchEngineId, short regionId, short cityId, int keywordId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <param name="regionId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        int SelectNext(short searchEngineId, short regionId, short cityId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <param name="regionId"></param>
        /// <param name="cityId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IEnumerable<KeywordScrapeDetail> SelectMany(short searchEngineId, short regionId, short cityId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <param name="regionId"></param>
        /// <param name="cityId"></param>
        /// <param name="keywordId"></param>
        /// <param name="lastCrawl"></param>
        void UpdateLastCrawl(short searchEngineId, short regionId, short cityId, int keywordId, DateTime lastCrawl);

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
