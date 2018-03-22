using ProtoBuf;

namespace ScraperFramework.Data.Entities
{
    [ProtoContract]
    public class ProxyMultiplier
    {
        [ProtoMember(1)]
        public int ProxyID { get; set; }

        [ProtoMember(2)]
        public int SearchEngineID { get; set; }

        [ProtoMember(3)]
        public int RegionID { get; set; }

        [ProtoMember(4)]
        public int Multiplier { get; set; }

        [ProtoMember(5)]
        public byte[] RowRevision { get; set; }
    }
}
