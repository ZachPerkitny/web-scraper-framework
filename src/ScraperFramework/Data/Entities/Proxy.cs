using ProtoBuf;

namespace ScraperFramework.Data.Entities
{
    [ProtoContract]
    public class Proxy
    {
        [ProtoMember(1)]
        public int ID { get; set; }

        [ProtoMember(2)]
        public string IP { get; set; }

        [ProtoMember(3)]
        public string Port { get; set; }

        [ProtoMember(4)]
        public int RegionID { get; set; }

        [ProtoMember(5)]
        public int SearchEngineGroup { get; set; }

        [ProtoMember(6)]
        public int Status { get; set; }

        [ProtoMember(7)]
        public int ProxyBlockID { get; set; }
    }
}
