namespace ScraperFramework.Data.Entities
{
    public class SearchString
    {
        public int ID { get; set; }

        public int SearchEngineID { get; set; }

        public int RegionID { get; set; }

        public string SearchEngine { get; set; }

        public string SearchEngineURL { get; set; }

        public string NaturalResultParamString { get; set; }

        public int DelayMultiplier { get; set; }
    }
}
