using ProtoBuf;

namespace ScraperFramework.Data.Entities
{
    [ProtoContract]
    public class Keyword
    {
        [ProtoMember(1)]
        public int ID { get; set; }

        [ProtoMember(2)]
        public string Value { get; set; }

        [ProtoMember(3)]
        public byte[] RowRevision { get; set; }
    }
}
