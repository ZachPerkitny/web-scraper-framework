namespace WebScraper.Pocos
{
    public class CrawlDescription
    {
        public string Keyword { get; set; }

        public int KeywordID { get; set; }

        public int SearchTargetID { get; set; }

        public string EndpointAddress { get; set; }

        public int EndpointID { get; set; }

        public string SearchTargetUrl { get; set; }
    }
}
