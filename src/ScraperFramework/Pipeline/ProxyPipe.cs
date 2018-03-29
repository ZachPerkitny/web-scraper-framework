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
            HashSet<Tuple<short, short>> proxyShortages = new HashSet<Tuple<short, short>>();
            LinkedListNode<CrawlDescription> node = pipelinedCrawlDescription.CrawlDescriptions.First;
            while (node != null)
            {
                Proxy proxy = _proxyManager.GetAvailableProxy(
                    node.Value.SearchEngineID, node.Value.RegionID);
                if (proxy != null)
                {
                    // add proxy to crawl description
                    node.Value.ProxyID = proxy.ProxyID;
                    node.Value.IP = proxy.IP;
                    node.Value.Port = proxy.Port;
                    node.Value.ProxyRegion = proxy.RegionID;
                }
                else
                {
                    // add the search engine and region so we can
                    // calculate the next availability
                    proxyShortages.Add(new Tuple<short, short>(
                        node.Value.SearchEngineID, node.Value.RegionID));
                    // mark the node for deletion
                    pipelinedCrawlDescription.MarkedForRemoval.Add(node);
                }

                node = node.Next;
            }

            // if there are no proxies available for some of the
            // crawl descriptions
            if (proxyShortages.Count > 0)
            {
                DateTime nextAvailability = _proxyManager.GetNextAvailability(proxyShortages);
                // if some other pipe set the next availability
                // because of a lack of resources, let it take
                // precedence if it is greater than the time
                // a proxy is available
                if (nextAvailability > pipelinedCrawlDescription.NextAvailability)
                {
                    pipelinedCrawlDescription.NextAvailability = nextAvailability;
                }
            }

            if (_connection != null)
            {
                _connection.Flow(pipelinedCrawlDescription);
            }

            return pipelinedCrawlDescription;
        }
    }
}
