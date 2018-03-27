using System;
using System.Collections.Generic;
using ScraperFramework.Pocos;
using ScraperFramework.Shared.Pocos;

namespace ScraperFramework.Pipeline
{
    internal class ProxyPipe : Pipe<PipelinedCrawlDescription>
    {
        private readonly IProxyManager _proxyManager;

        public ProxyPipe(IProxyManager proxyManager)
        {
            _proxyManager = proxyManager ?? throw new ArgumentNullException(nameof(proxyManager));
        }

        public override PipelinedCrawlDescription Flow(PipelinedCrawlDescription pipelinedCrawlDescription)
        {
            LinkedListNode<CrawlDescription> node = pipelinedCrawlDescription.CrawlDescriptions.First;
            while (node != null)
            {
                LinkedListNode<CrawlDescription> next = node.Next;
                Proxy proxy = _proxyManager.GetAvailableProxy(
                    node.Value.SearchEngineID, node.Value.RegionID);

                if (proxy != null)
                {
                    // add proxy to crawl description
                    node.Value.ProxyID = proxy.ProxyID;
                    node.Value.IP = proxy.IP;
                    node.Value.Port = proxy.Port;
                }
                else
                {
                    // drop it
                    pipelinedCrawlDescription.CrawlDescriptions.Remove(node);
                    // set next availability
                    DateTime nextAvailability = _proxyManager.GetNextAvailability(
                        node.Value.SearchEngineID, node.Value.RegionID);
                    if (nextAvailability < pipelinedCrawlDescription.NextAvailability)
                    {
                        pipelinedCrawlDescription.NextAvailability = nextAvailability;
                    }
                }

                node = next;
            }

            // if we removed all nodes because of a lack of proxies, return early
            if (pipelinedCrawlDescription.CrawlDescriptions.Count == 0)
            {
                return pipelinedCrawlDescription;
            }

            if (_connection != null)
            {
                return _connection.Flow(pipelinedCrawlDescription);
            }

            return pipelinedCrawlDescription;
        }
    }
}
