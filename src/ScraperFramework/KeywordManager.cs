using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
        private Task _refillTask;
        private bool _startedRefillTask = false;

        private bool _disposed = false;

        public KeywordManager(IKeywordScrapeDetailRepo keywordScrapeDetailRepo, IKeywordRepo keywordRepo)
        {
            _keywordScrapeDetailRepo = keywordScrapeDetailRepo ?? throw new ArgumentNullException(nameof(keywordScrapeDetailRepo));
            _keywordRepo = keywordRepo ?? throw new ArgumentNullException(nameof(keywordRepo));
        }

        public IEnumerable<Keyword> GetKeywords(int count)
        {
            // TODO(zvp): this needs to be refactored, naively
            // implemented
            IEnumerable<Keyword> keywords;

            if (!_startedRefillTask)
            {
                _refillTask = Task.Factory.StartNew(RefillCache, TaskCreationOptions.LongRunning);
                _startedRefillTask = true;
            }

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
            }

            // Signal refill 
            _autoResetEvent.Set();

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
        private Task RefillCache()
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                // wait for signal
                _autoResetEvent.WaitOne();
                
                lock (_locker)
                {
                    IEnumerable<Keyword> keywords = GetKeywordsFromDB(
                        _keywordCacheLimit - _keywords.Count);
                    foreach (Keyword keyword in keywords)
                    {
                        _keywords.Enqueue(keyword);
                    }
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _cts.Cancel();
                    _cts.Dispose();
                    _autoResetEvent.Dispose();
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
