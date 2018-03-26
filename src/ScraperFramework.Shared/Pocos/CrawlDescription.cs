namespace ScraperFramework.Shared.Pocos
{
    public class CrawlDescription
    {
        public string Keyword { get; set; }

        public int KeywordID { get; set; }

        public short SearchEngineID { get; set; }

        public short RegionID { get; set; }

        public short CityID { get; set; }

        public int SearchEngineGroup { get; set; }

        public int ProxyID { get; set; }

        public string IP { get; set; }

        public int Port { get; set; }

        public string UserAgent { get; set; }

        public string SearchString { get; set; }

        public bool IsNatural { get; set; }
    }
}
