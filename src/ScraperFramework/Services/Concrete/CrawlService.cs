using System;
using System.Collections.Generic;
using System.Linq;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;
using WebScraper.Pocos;

namespace ScraperFramework.Services.Concrete
{
    class CrawlService : ICrawlService
    {
        private const ushort MAX_TKO = 3;

        /// <summary>
        /// If GetKeywordsToCrawl returns null, or an empty list,
        /// it is because there are no endpoints available or there
        /// are no keywords left to crawl. This value can be used
        /// to prevent blind calls to the GetKeywordsToCrawl method.
        /// </summary>
        public DateTime NextAvailability { get; private set; }

        public CrawlService()
        { }

        public IEnumerable<CrawlDescription> GetKeywordsToCrawl(int count)
        {
            throw new NotImplementedException();
        }
    }
}
