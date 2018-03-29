using System;
using System.Collections.Generic;
using ScraperFramework.Shared.Pocos;

namespace ScraperFramework.Pocos
{
    internal class PipelinedCrawlDescription
    {
        public LinkedList<CrawlDescription> CrawlDescriptions { get; set; }

        public List<LinkedListNode<CrawlDescription>> MarkedForRemoval { get; set; }

        public DateTime NextAvailability { get; set; } = DateTime.Now;
    }
}
