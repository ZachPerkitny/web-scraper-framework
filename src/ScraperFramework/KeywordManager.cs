using System;
using System.Collections.Generic;
using ScraperFramework.Data;
using ScraperFramework.Pocos;

namespace ScraperFramework
{
    class KeywordManager : IKeywordManager
    {
        private readonly IKeywordScrapeDetailRepo _keywordScrapeDetailRepo;
        private readonly IKeywordRepo _keywordRepo;

        public KeywordManager(IKeywordScrapeDetailRepo keywordScrapeDetailRepo, IKeywordRepo keywordRepo)
        {
            _keywordScrapeDetailRepo = keywordScrapeDetailRepo ?? throw new ArgumentNullException(nameof(keywordScrapeDetailRepo));
            _keywordRepo = keywordRepo ?? throw new ArgumentNullException(nameof(keywordRepo));
        }

        public IEnumerable<Keyword> GetKeywordsToCrawl(int count)
        {
            // TODO(zvp): Implement this
            return GetKeywordsFromDB(count);
        }

        /// <summary>
        /// Returns the specified number of keywords from the internal
        /// queue, ensure there are enough elements in the queue.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private IEnumerable<Keyword> GetKeywordsFromCache(int count)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private IEnumerable<Keyword> GetKeywordsFromDB(int count)
        {
            List<Keyword> keywords = new List<Keyword>();
            IEnumerable<Data.Entities.KeywordScrapeDetail> keywordsToScrape = _keywordScrapeDetailRepo
                .SelectNext(count);   
            foreach (Data.Entities.KeywordScrapeDetail keywordScrapeDetail in keywordsToScrape)
            {
                Data.Entities.Keyword keyword = _keywordRepo.Select(keywordScrapeDetail.KeywordID);
                if (keyword != null)
                {
                    keywords.Add(new Keyword
                    {
                        KeywordValue = keyword.Value,
                        KeywordID = keyword.ID,
                        SearchEngineID = keywordScrapeDetail.SearchEngineID,
                        RegionID = keywordScrapeDetail.RegionID,
                        CityID = keywordScrapeDetail.CityID
                    });
                }
            }

            return keywords;
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefillCache()
        {
            throw new NotImplementedException();
        }
    }
}
