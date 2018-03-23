using System;
using System.Collections.Generic;
using System.Linq;
using ScraperFramework.Pocos;
using ScraperFramework.Shared.Pocos;

namespace ScraperFramework.Pipeline
{
    /// <summary>
    /// A Proxy Driven Crawl Description Pipeline
    /// </summary>
    internal class CrawlDescriptionPipeline : PipeLine<PipelinedCrawlDescription>
    {
        private readonly IProxyManager _proxyManager;

        public CrawlDescriptionPipeline(IProxyManager proxyManager)
        {
            _proxyManager = proxyManager ?? throw new ArgumentNullException(nameof(proxyManager));
        }

        public override PipelinedCrawlDescription Drain()
        {
            PipelinedCrawlDescription pipelinedCrawlDescription = new PipelinedCrawlDescription
            {
                CrawlDescriptions = new List<CrawlDescription>()
            };

            IEnumerable<Proxy> availableProxies = _proxyManager.GetAvailableProxies();
            if (availableProxies.Any())
            {
                foreach (Proxy proxy in availableProxies)
                {
                    pipelinedCrawlDescription.CrawlDescriptions.Add(new CrawlDescription
                    {
                        ProxyID = proxy.ProxyID,
                        IP = proxy.IP,
                        Port = proxy.Port,
                        RegionID = proxy.RegionID
                    });
                }
            }
            else
            {
                // nothing to drain, exit early
                pipelinedCrawlDescription.NextAvailability = _proxyManager.GetNextAvailability();
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
