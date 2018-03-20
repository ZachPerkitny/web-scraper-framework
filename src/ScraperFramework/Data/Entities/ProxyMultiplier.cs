using System;
namespace ScraperFramework.Data.Entities
{
    public class ProxyMultiplier
    {
        public int ID { get; set; }

        public int ProxyID { get; set; }

        public int SearchEngineID { get; set; }

        public int RegionID { get; set; }

        public int Multiplier { get; set; }
    }
}
