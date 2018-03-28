using System;
using System.Collections.Generic;
using System.Linq;
using ScraperFramework.Data;
using ScraperFramework.Pocos;

namespace ScraperFramework
{
    class KeywordManager : IKeywordManager
    {
        private const int LIMIT_MULTIPLIER = 4;

        private readonly IKeywordScrapeDetailRepo _keywordScrapeDetailRepo;
        private readonly IKeywordRepo _keywordRepo;

        private readonly Queue<Keyword> _keywords = new Queue<Keyword>();
        private int _keywordCacheLimit = 1000;

        private readonly object _locker = new object();

        public KeywordManager(IKeywordScrapeDetailRepo keywordScrapeDetailRepo, IKeywordRepo keywordRepo)
        {
            _keywordScrapeDetailRepo = keywordScrapeDetailRepo ?? throw new ArgumentNullException(nameof(keywordScrapeDetailRepo));
            _keywordRepo = keywordRepo ?? throw new ArgumentNullException(nameof(keywordRepo));
        }

        public IEnumerable<Keyword> GetKeywordsToCrawl(int count)
        {
            // TODO(zvp): this needs to be refactored, naively
            // implemented
            IEnumerable<Keyword> keywords;
            lock (_locker)
            {
                // assume that requests of this size
                // will continue to occur and increase
                // cache limit
                if (count > (_keywordCacheLimit / LIMIT_MULTIPLIER))
                {
                    // adjust cache size
                    _keywordCacheLimit = count * LIMIT_MULTIPLIER;
                }

                if (count > _keywords.Count)
                {
                    keywords = GetKeywordsFromCache(_keywords.Count)
                        .Concat(GetKeywordsFromDB(count - _keywords.Count));
                }
                else
                {
                    keywords = GetKeywordsFromCache(count);
                }

                if (_keywords.Count == 0)
                {
                    RefillCache();
                }
            }

            return keywords;
        }

        /// <summary>
        /// Returns the specified number of keywords from the internal
        /// queue, ensure there are enough elements in the queue.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private IEnumerable<Keyword> GetKeywordsFromCache(int count)
        {
            List<Keyword> keywords = new List<Keyword>();
            for (int i = 0; i < count; i++)
            {
                keywords.Add(_keywords.Dequeue());
            }

            return keywords;
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
            IEnumerable<Keyword> keywords = GetKeywordsFromDB(_keywordCacheLimit);
            foreach (Keyword keyword in keywords)
            {
                _keywords.Enqueue(keyword);
            }
        }
    }
}
