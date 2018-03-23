using System;
using System.Collections.Generic;
using ScraperFramework.Shared.Pocos;

namespace ScraperFramework.Pocos
{
    internal class PipelinedCrawlDescription
    {
        public IEnumerable<CrawlDescription> CrawlDescriptions { get; set; }

        public DateTime NextAvailability { get; set; } = DateTime.Now;
    }
}
