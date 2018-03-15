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

        /// <summary>
        /// If GetKeywordsToCrawl returns null, or an empty list,
        /// it is because there are no endpoints available or there
        /// are no keywords left to crawl. This value can be used
        /// to prevent blind calls to the GetKeywordsToCrawl method.
        /// </summary>
        public DateTime NextAvailability { get; private set; }

        private readonly IEndpointRepo _endpointRepo;
        private readonly IEndpointSearchTargetRepo _endpointSearchTargetRepo;
        private readonly IKeywordRepo _keywordRepo;
        private readonly IKeywordSearchTargetRepo _keywordSearchTargetRepo;
        private readonly ISearchTargetRepo _searchTargetRepo;
        private readonly ICrawlLogRepo _crawlLogRepo;
        
        private readonly Dictionary<int, Keyword> _keywords = new Dictionary<int, Keyword>();
        private readonly Dictionary<int, Dictionary<int, int>> _keywordCrawlCounts =
            new Dictionary<int, Dictionary<int, int>>();

        private readonly Dictionary<int, Endpoint> _endpoints = new Dictionary<int, Endpoint>();
        // Tuple (WebsiteID, CountryID)
        private readonly Dictionary<Tuple<int, int>, DateTime> _endpointCooldowns = new Dictionary<Tuple<int, int>, DateTime>();
        
        private bool _filledCache;

        public CrawlService(IEndpointRepo endpointRepo, IEndpointSearchTargetRepo endpointSearchTargetRepo, IKeywordRepo keywordRepo, 
            IKeywordSearchTargetRepo keywordSearchTargetRepo, ISearchTargetRepo searchTargetRepo, ICrawlLogRepo crawlLogRepo)
        {
            _endpointRepo = endpointRepo ?? throw new ArgumentNullException(nameof(endpointRepo));
            _endpointSearchTargetRepo = endpointSearchTargetRepo ?? throw new ArgumentNullException(nameof(endpointSearchTargetRepo));
            _keywordRepo = keywordRepo ?? throw new ArgumentNullException(nameof(keywordRepo));
            _keywordSearchTargetRepo = keywordSearchTargetRepo ?? throw new ArgumentNullException(nameof(keywordSearchTargetRepo));
            _searchTargetRepo = searchTargetRepo ?? throw new ArgumentNullException(nameof(searchTargetRepo));
            _crawlLogRepo = crawlLogRepo ?? throw new ArgumentNullException(nameof(crawlLogRepo));
        }

        public IEnumerable<CrawlDescription> GetKeywordsToCrawl(int count)
        {
            if (!_filledCache)
            {
                FillCache();
            }

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

        /// <summary>
        /// On initial load, the service fills up a dictionary with 
        /// crawl counts to avoid uneccesary file reads.
        /// </summary>
        private void FillCache()
        {
            

            _filledCache = true;
        }
    }
}
