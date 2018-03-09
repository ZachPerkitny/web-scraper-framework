using System;
using System.Collections.Generic;
using System.Linq;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;

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

        public CrawlService(IKeywordRepo keywordRepo, ISearchTargetRepo searchTargetRepo, ICrawlLogRepo crawlLogRepo)
        {
            _keywordRepo = keywordRepo ?? throw new ArgumentNullException(nameof(keywordRepo));
            _searchTargetRepo = searchTargetRepo ?? throw new ArgumentNullException(nameof(searchTargetRepo));
            _crawlLogRepo = crawlLogRepo ?? throw new ArgumentNullException(nameof(crawlLogRepo));
        }

        public IEnumerable<Keyword> GetKeywordsToCrawl(int count, int searchTargetId)
        {
            if (_keywordCrawlCounts[searchTargetId] == null)
            {
                FillCache(searchTargetId);
            }

            // select random count keywords
            var toBeCrawled = new List<Keyword>();

            return toBeCrawled;
        }

        public IEnumerable<SearchTarget> GetSearchTargets()
        {
            IEnumerable<SearchTarget> searchTargets = _searchTargetRepo.SelectAll();
            return searchTargets;
        }

        public void LogCrawl(CrawlLog crawlLog)
        {
            if (_keywordCrawlCounts[crawlLog.SearchTargetID] == null)
            {
                FillCache(crawlLog.SearchTargetID);
            }

            _crawlLogRepo.Insert(crawlLog);
            _keywordCrawlCounts[crawlLog.SearchTargetID][crawlLog.KeywordID] += 1;
        }

        private void FillCache(int searchTargetId)
        {
            IEnumerable<Keyword> keywords = _keywordRepo.SelectAll();

            _keywordCrawlCounts[searchTargetId] = new Dictionary<int, int>();

            foreach (var keyword in keywords)
            {
                int crawlCount = _crawlLogRepo.SelectMany(searchTargetId, keyword.ID).Count();
                _keywordCrawlCounts[searchTargetId].Add(keyword.ID, crawlCount);
            }
        }
    }
}
