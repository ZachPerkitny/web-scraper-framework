using System;

namespace ScraperFramework.Data.Entities
{
    public class SearchTarget
    {
        public int ID { get; set; }

        public int CountryID { get; set; }

        public int CityID { get; set; }

        public bool IsMobile { get; set; }
    }
}
