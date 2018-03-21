using ProtoBuf;

namespace ScraperFramework.Data.Entities
{
    [ProtoContract]
    public class InternationalUULE
    {
        [ProtoMember(1)]
        public int ID { get; set; }

        [ProtoMember(2)]
        public int RegionID { get; set; }

        [ProtoMember(3)]
        public string UULE { get; set; }
    }
}
