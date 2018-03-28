using System;
using System.Collections.Generic;
using System.Linq;
using ScraperFramework.Pocos;
using ScraperFramework.Shared.Pocos;

namespace ScraperFramework.Pipeline
{
    /// <summary>
    /// A Keyword Driven Crawl Description Pipeline
    /// </summary>
    internal class KeywordDrivenPipeline : PipeLine<PipelinedCrawlDescription>
    {
        private const int BATCH_SIZE = 1000;

        private readonly IKeywordManager _keywordManager;

        public KeywordDrivenPipeline(IKeywordManager keywordManager)
        {
            _keywordManager = keywordManager ?? throw new ArgumentNullException(nameof(keywordManager));
        }

        public override PipelinedCrawlDescription Drain()
        {
            PipelinedCrawlDescription pipelinedCrawlDescription = new PipelinedCrawlDescription
            {
                CrawlDescriptions = new LinkedList<CrawlDescription>()
            };

            IEnumerable<Keyword> keywordsToScrape = _keywordManager.GetKeywords(BATCH_SIZE);
            if (keywordsToScrape.Any())
            {
                foreach (Keyword keyword in keywordsToScrape)
                {
                    pipelinedCrawlDescription.CrawlDescriptions.AddLast(new CrawlDescription
                    {
                        Keyword = keyword.KeywordValue,
                        KeywordID = keyword.KeywordID,
                        SearchEngineID = keyword.SearchEngineID,
                        RegionID = keyword.RegionID,
                        CityID = keyword.CityID
                    });
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
