using ProtoBuf;

namespace ScraperFramework.Data.Entities
{
    [ProtoContract]
    public class SearchEngine
    {
        [ProtoMember(1)]
        public short ID { get; set; }

        [ProtoMember(2)]
        public bool IsMobile { get; set; }

        [ProtoMember(3)]
        public byte[] RowRevision { get; set; }
    }
}
