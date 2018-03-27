using System;
using System.Collections.Generic;
using System.Linq;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;
using ScraperFramework.Pocos;
using ScraperFramework.Shared.Pocos;

namespace ScraperFramework.Pipeline
{
    /// <summary>
    /// A Keyword Driven Crawl Description Pipeline
    /// </summary>
    internal class KeywordDrivenPipeline : PipeLine<PipelinedCrawlDescription>
    {
        private const int BATCH_SIZE = 5000;

        private readonly IKeywordScrapeDetailRepo _keywordScrapeDetailRepo;
        private readonly IKeywordRepo _keywordRepo;

        public KeywordDrivenPipeline(IKeywordScrapeDetailRepo keywordScrapeDetailRepo, IKeywordRepo keywordRepo)
        {
            _keywordScrapeDetailRepo = keywordScrapeDetailRepo ?? throw new ArgumentNullException(nameof(keywordScrapeDetailRepo));
            _keywordRepo = keywordRepo ?? throw new ArgumentNullException(nameof(keywordRepo));
        }

        public override PipelinedCrawlDescription Drain()
        {
            PipelinedCrawlDescription pipelinedCrawlDescription = new PipelinedCrawlDescription
            {
                CrawlDescriptions = new LinkedList<CrawlDescription>()
            };

            IEnumerable<KeywordScrapeDetail> keywordsToScrape = _keywordScrapeDetailRepo.SelectNext(BATCH_SIZE);
            if (keywordsToScrape.Any())
            {
                foreach (KeywordScrapeDetail keywordScrapeDetail in keywordsToScrape)
                {
                    Keyword keyword = _keywordRepo.Select(keywordScrapeDetail.KeywordID);
                    if (keyword != null)
                    {
                        pipelinedCrawlDescription.CrawlDescriptions.AddLast(new CrawlDescription
                        {
                            SearchEngineID = keywordScrapeDetail.SearchEngineID,
                            RegionID = keywordScrapeDetail.RegionID,
                            CityID = keywordScrapeDetail.CityID,
                            Keyword = keyword.Value,
                            KeywordID = keyword.ID
                        });
                    }
                }
            }
            else
            {
                // TODO(zvp): If this happens it just means that
                // there are no keyword scrape details stored ?
                // Add constant delay ?
                return pipelinedCrawlDescription;
            }

            if (_rootPipe != null)
            {
                return _rootPipe.Flow(pipelinedCrawlDescription);
            }

            return pipelinedCrawlDescription;
        }
    }
}
