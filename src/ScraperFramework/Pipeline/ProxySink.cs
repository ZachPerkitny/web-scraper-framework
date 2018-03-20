using System;
using System.Collections.Generic;
using ScraperFramework.Data;
using WebScraper.Pocos;

namespace ScraperFramework.Pipeline
{
    internal class ProxySink : Sink<IEnumerable<CrawlDescription>>
    {
        private readonly IProxyRepo _proxyRepo;

        public ProxySink(IProxyRepo proxyRepo)
        {
            _proxyRepo = proxyRepo ?? throw new ArgumentNullException(nameof(proxyRepo));
        }

        public override IEnumerable<CrawlDescription> Drain()
        {
            throw new NotImplementedException();
        }
    }
}
