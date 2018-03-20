namespace ScraperFramework.Data.Entities
{
    public class Proxy
    {
        public int ID { get; set; }

        public string IP { get; set; }

        public string Port { get; set; }

        public int RegionID { get; set; }

        public int SearchEngineGroup { get; set; }

        public int Status { get; set; }

        public int ProxyBlockID { get; set; }
    }
}
