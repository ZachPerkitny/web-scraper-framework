using ProtoBuf;

namespace ScraperFramework.Data.Entities
{
    [ProtoContract]
    public class SpecialKeyword
    {
        [ProtoMember(1)]
        public int ID { get; set; }

        [ProtoMember(2)]
        public int KeywordID { get; set; }

        [ProtoMember(3)]
        public int SearchEngineID { get; set; }

        [ProtoMember(4)]
        public int RegionID { get; set; }
    }
}
