using System;
using ScraperFramework.Data;
using ScraperFramework.Pocos;

namespace ScraperFramework.Pipeline
{
    internal class CrawlDescriptionPipeline : PipeLine<PipelinedCrawlDescription>
    {
        private readonly IProxyRepo _proxyRepo;

        public CrawlDescriptionPipeline(IProxyRepo proxyRepo)
        {
            _proxyRepo = proxyRepo ?? throw new ArgumentNullException(nameof(proxyRepo));
        }

        public override PipelinedCrawlDescription Drain()
        {
            throw new NotImplementedException();
        }
    }
}
