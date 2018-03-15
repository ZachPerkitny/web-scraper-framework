using System;
using ScraperFramework.Enum;

namespace ScraperFramework.Data.Entities
{
    public class CrawlLog
    {
        public long ID { get; set; }

        public int KeywordID { get; set; }

        public int SearchTargetID { get; set; }

        public int EndpointID { get; set; }

        public DateTime CrawlTime { get; } = DateTime.UtcNow;

        public CrawlResultID CrawlResultID { get; set; }
    }
}
