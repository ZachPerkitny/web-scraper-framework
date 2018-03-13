using System;
using System.Collections.Generic;
using System.Linq;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;
using ScraperFramework.Pocos;

namespace ScraperFramework.Services.Concrete
{
    class CrawlService : ICrawlService
    {
        private const ushort MAX_TKO = 3;

        private readonly IKeywordRepo _keywordRepo;
        private readonly ISearchTargetRepo _searchTargetRepo;
        private readonly ICrawlLogRepo _crawlLogRepo;

        private readonly Dictionary<int, Dictionary<int, int>> _keywordCrawlCounts = 
            new Dictionary<int, Dictionary<int, int>>();
        private bool _filledCache;

        public CrawlService(IKeywordRepo keywordRepo, ISearchTargetRepo searchTargetRepo, ICrawlLogRepo crawlLogRepo)
        {
            _keywordRepo = keywordRepo ?? throw new ArgumentNullException(nameof(keywordRepo));
            _searchTargetRepo = searchTargetRepo ?? throw new ArgumentNullException(nameof(searchTargetRepo));
            _crawlLogRepo = crawlLogRepo ?? throw new ArgumentNullException(nameof(crawlLogRepo));
        }

        public IEnumerable<CrawlDescription> GetKeywordsToCrawl(int count)
        {
            if (!_filledCache)
            {
                FillCache();
            }

            // select random count keywords
            var toBeCrawled = new List<CrawlDescription>();

            return toBeCrawled;
        }

        public IEnumerable<SearchTarget> GetSearchTargets()
        {
            IEnumerable<SearchTarget> searchTargets = _searchTargetRepo.SelectAll();
            return searchTargets;
        }

        public void LogCrawl(CrawlLog crawlLog)
        {
            if (!_filledCache)
            {
                FillCache();
            }

            _crawlLogRepo.Insert(crawlLog);
            _keywordCrawlCounts[crawlLog.SearchTargetID][crawlLog.KeywordID] += 1;
        }

        private void FillCache()
        {
            IEnumerable<SearchTarget> searchTargets = _searchTargetRepo.SelectAll();
            foreach (var searchTarget in searchTargets)
            {
                IEnumerable<Keyword> keywords = _keywordRepo.SelectAll();

                _keywordCrawlCounts[searchTarget.ID] = new Dictionary<int, int>();

                foreach (var keyword in keywords)
                {
                    int crawlCount = _crawlLogRepo.SelectMany(searchTarget.ID, keyword.ID).Count();
                    _keywordCrawlCounts[searchTarget.ID].Add(keyword.ID, crawlCount);
                }
            }
            _filledCache = true;
        }
    }
}
