using ProtoBuf;

namespace ScraperFramework.Data.Entities
{
    [ProtoContract]
    public class SearchString
    {
        [ProtoMember(1)]
        public int ID { get; set; }

        [ProtoMember(2)]
        public int SearchEngineID { get; set; }

        [ProtoMember(3)]
        public int RegionID { get; set; }

        [ProtoMember(4)]
        public string SearchEngine { get; set; }

        [ProtoMember(5)]
        public string SearchEngineURL { get; set; }

        [ProtoMember(6)]
        public string NaturalResultParamString { get; set; }

        [ProtoMember(7)]
        public int DelayMultiplier { get; set; }

        [ProtoMember(8)]
        public byte[] RowRevision { get; set; }
    }
}
