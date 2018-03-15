using System;
using System.Collections.Generic;
using System.Linq;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;
using WebScraper.Pocos;

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
            IEnumerable<CrawlDescription> toBeCrawled = _keywordRepo.SelectAll()
                .Select(k => new CrawlDescription
                {
                    Keyword = k.Value,
                    KeywordID = k.ID,
                    EndpointAddress = "54.226.252.167:8888"
                });
            
            return toBeCrawled;
        }

        public IEnumerable<SearchTarget> GetSearchTargets()
        {
            IEnumerable<SearchTarget> searchTargets = _searchTargetRepo.SelectAll();
            return searchTargets;
        }

        public void LogCrawl(CrawlLog crawlLog)
        {
            _crawlLogRepo.Insert(crawlLog);
        }
    }
}
