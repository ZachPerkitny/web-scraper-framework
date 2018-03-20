using System;
using System.Collections.Generic;
using WebScraper.Pocos;

namespace ScraperFramework.Pocos
{
    class PipelinedCrawlDescription
    {
        public IEnumerable<CrawlDescription> CrawlDescriptions { get; set; }

        public DateTime NextAvailability { get; set; } = DateTime.Now;
    }
}
