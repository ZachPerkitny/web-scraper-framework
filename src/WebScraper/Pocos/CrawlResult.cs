using System.Collections.Generic;

namespace WebScraper.Pocos
{
    public class AdResult
    {
        public string Title { get; set; }

        public string Url { get; set; }
    }

    public class CrawlResult
    {
        public int KeywordID { get; set; }

        public int SearchTargetID { get; set; }

        public int EndpointID { get; set; }

        public IEnumerable<AdResult> Ads { get; set; }
    }
}
