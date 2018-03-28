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
            HashSet<Tuple<short, short>> droppedCrawlDescriptions = new HashSet<Tuple<short, short>>();
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
                    node.Value.ProxyRegion = proxy.RegionID;
                }
                else
                {
                    // save dropped (seid, regionid) pairs to calculate next availability
                    droppedCrawlDescriptions.Add(new Tuple<short, short>(
                        node.Value.SearchEngineID, node.Value.RegionID));
                    // drop it
                    pipelinedCrawlDescription.CrawlDescriptions.Remove(node);

                }

                node = next;
            }

            // if we removed all nodes because of a lack of proxies, return early
            if (pipelinedCrawlDescription.CrawlDescriptions.Count == 0)
            {
                pipelinedCrawlDescription.NextAvailability = _proxyManager.GetNextAvailability(droppedCrawlDescriptions);
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
