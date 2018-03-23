using System;
using System.Collections.Generic;
using ScraperFramework.Shared.Pocos;

namespace ScraperFramework.Pocos
{
    internal class PipelinedCrawlDescription
    {
        public List<CrawlDescription> CrawlDescriptions { get; set; }

        public DateTime NextAvailability { get; set; } = DateTime.Now;
    }
}
