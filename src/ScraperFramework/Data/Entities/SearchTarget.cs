namespace ScraperFramework.Data.Entities
{
    public class SearchTarget
    {
        public int ID { get; set; }

        public int WebsiteID { get; set; }

        public int CountryID { get; set; }

        public int? CityID { get; set; }

        public bool IsMobile { get; set; }

        public int PlatformID { get; set; }

        public string SearchTargetURL { get; set; }
    }
}
