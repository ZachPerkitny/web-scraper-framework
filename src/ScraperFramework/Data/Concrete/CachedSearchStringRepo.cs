using System;
using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data.Concrete
{
    public class CachedSearchStringRepo : SearchStringRepoDecorator
    {
        private Dictionary<Tuple<int, int>, SearchString> _cache;

        public CachedSearchStringRepo(ISearchStringRepo searchStringRepo) 
            : base(searchStringRepo)
        {
            _cache = new Dictionary<Tuple<int, int>, SearchString>();
        }

        public override SearchString Select(int searchEngineId, int regionId)
        {
            Tuple<int, int> key = new Tuple<int, int>(searchEngineId, regionId);
            SearchString searchString = null;

            if (_cache.ContainsKey(key))
            {
                searchString = _cache[key];
            }
            else
            {
                searchString = base.Select(searchEngineId, regionId);
                _cache.Add(key, searchString);
            }

            return searchString;
        }
    }
}
