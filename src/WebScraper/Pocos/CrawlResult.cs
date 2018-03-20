using System.Collections.Generic;
using WebScraper.Enum;

namespace WebScraper.Pocos
{
    public class AdResult
    {
        public string Title { get; set; }

        public string Url { get; set; }
    }

    public class CrawlResult
    {
        public CrawlResultID CrawlResultID { get; set; }

        public IEnumerable<AdResult> Ads { get; set; }
    }
}
