using System;
using System.Collections.Generic;
using System.Linq;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;
using ScraperFramework.Pocos;
using ScraperFramework.Shared.Pocos;


namespace ScraperFramework.Pipeline
{
    internal class KeywordPipe : Pipe<PipelinedCrawlDescription>
    {
        private readonly IKeywordScrapeDetailRepo _keywordScrapeDetailRepo;
        private readonly IKeywordRepo _keywordRepo;

        public KeywordPipe(IKeywordScrapeDetailRepo keywordScrapeDetailRepo, IKeywordRepo keywordRepo)
        {
            _keywordScrapeDetailRepo = keywordScrapeDetailRepo ?? throw new ArgumentNullException(nameof(keywordScrapeDetailRepo));
            _keywordRepo = keywordRepo ?? throw new ArgumentNullException(nameof(keywordRepo));
        }

        public override PipelinedCrawlDescription Flow(PipelinedCrawlDescription pipelinedCrawlDescription)
        {
            //var x = _keywordScrapeDetailRepo.SelectNext(3, 1, 0);
            return pipelinedCrawlDescription;
        }
    }
}
