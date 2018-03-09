using System;
using ScraperFramework.Enums;

namespace ScraperFramework.Data.Entities
{
    public class CrawlLog
    {
        public long ID { get; set; }

        public int KeywordID { get; set; }

        public int SearchTargetID { get; set; }

        public DateTime CrawlTime { get; } = DateTime.UtcNow;

        public CrawlResult CrawlResultID { get; set; }
    }
}
