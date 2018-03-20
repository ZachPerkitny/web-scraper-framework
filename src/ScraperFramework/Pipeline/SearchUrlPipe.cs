using System;
using System.Collections.Generic;
using ScraperFramework.Data;
using WebScraper.Pocos;

namespace ScraperFramework.Pipeline
{
    internal class SearchUrlPipe : Pipe<IEnumerable<CrawlDescription>>
    {
        private readonly ISearchStringRepo _searchStringRepo;

        public SearchUrlPipe(ISearchStringRepo searchStringRepo)
        {
            _searchStringRepo = searchStringRepo ?? throw new ArgumentNullException(nameof(searchStringRepo));
        }

        public override IEnumerable<CrawlDescription> Flow(IEnumerable<CrawlDescription> entity)
        {
            throw new NotImplementedException();
        }
    }
}
