using System;
using System.Collections.Generic;
using ScraperFramework.Data;
using WebScraper.Pocos;

namespace ScraperFramework.Pipeline
{
    internal class UULEPipe : Pipe<IEnumerable<CrawlDescription>>
    {
        private readonly ILocalUULERepo _localUULERepo;
        private readonly IInternationalUULERepo _internationalUULERepo;

        public UULEPipe(ILocalUULERepo localUULERepo, 
            IInternationalUULERepo internationalUULERepo)
        {
            _localUULERepo = localUULERepo ?? throw new ArgumentNullException(nameof(localUULERepo));
        }

        public override IEnumerable<CrawlDescription> Flow(IEnumerable<CrawlDescription> crawlDescriptions)
        {
            throw new NotImplementedException();
        }
    }
}
