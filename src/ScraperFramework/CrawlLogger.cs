using System;
using System.Threading.Tasks;
using ScraperFramework.Data;
using ScraperFramework.Shared.Pocos;

namespace ScraperFramework
{
    class CrawlLogger : ICrawlLogger
    {
        private readonly IProxyManager _proxyManager;
        private readonly IKeywordScrapeDetailRepo _keywordScrapeDetailRepo;

        public CrawlLogger(IProxyManager proxyManager, IKeywordScrapeDetailRepo keywordScrapeDetailRepo)
        {
            _proxyManager = proxyManager ?? throw new ArgumentNullException(nameof(proxyManager));
            _keywordScrapeDetailRepo = keywordScrapeDetailRepo ?? throw new ArgumentNullException(nameof(keywordScrapeDetailRepo));
        }

        public async Task LogCrawl(CrawlDescription crawlDescription, CrawlResult crawlResult)
        {
            // mark proxy as used to unlock for later usage
            _proxyManager.UnLock(crawlDescription.SearchEngineID, crawlDescription.RegionID, 
                crawlDescription.ProxyID, crawlResult.CrawlResultID);

            // update last crawl
            await Task.Run(() =>
            {
                _keywordScrapeDetailRepo.UpdateLastCrawl(
                    crawlDescription.SearchEngineID,
                    crawlDescription.RegionID,
                    crawlDescription.CityID,
                    crawlDescription.KeywordID,
                    DateTime.Now);
            });

            // TODO(zvp): Write to flat files
        }
    }
}
