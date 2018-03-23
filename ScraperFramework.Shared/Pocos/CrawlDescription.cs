namespace ScraperFramework.Shared.Pocos
{
    public class CrawlDescription
    {
        public string Keyword { get; set; }

        public int KeywordID { get; set; }

        public int SearchEngineID { get; set; }

        public int RegionID { get; set; }

        public int CityID { get; set; }

        public int SearchEngineGroup { get; set; }

        public int ProxyID { get; set; }

        public string IP { get; set; }

        public int Port { get; set; }

        public string UserAgent { get; set; }

        public string SearchString { get; set; }

        public bool IsNatural { get; set; }
    }
}
