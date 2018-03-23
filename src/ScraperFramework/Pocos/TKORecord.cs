using FlatFileDB.Attributes;

namespace ScraperFramework.Pocos
{
    [DelimitedTable("\uFDD9")]
    internal class TKORecord
    {
        public int KeywordId;

        public int LastUpdate;

        public int SearchEngineId;

        public int RegionId;

        public int CityId;

        public bool IsLocal;

        public int ProxyId;

        public int UserAgentId;
    }
}
