using System;
using ProtoBuf;

namespace ScraperFramework.Data.Entities
{
    [ProtoContract]
    public class KeywordScrapeDetail
    {
        [ProtoMember(1)]
        public int SearchEngineID { get; set; }

        [ProtoMember(2)]
        public int RegionID { get; set; }

        [ProtoMember(3)]
        public int CityID { get; set; } = 0;

        [ProtoMember(4)]
        public int Priority { get; set; }

        [ProtoMember(5)]
        public bool IsActive { get; set; }

        [ProtoMember(6)]
        public int KeywordID { get; set; }

        [ProtoMember(7)]
        public byte[] RowRevision { get; set; }

        [ProtoMember(8)]
        public DateTime LastCrawl { get; set; }
    }
}
