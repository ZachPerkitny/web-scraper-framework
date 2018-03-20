using System;
using System.Collections.Generic;
using System.Linq;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data.Concrete
{
    public class CachedSearchStringRepo : SearchStringRepoDecorator
    {
        private readonly Dictionary<Tuple<int, int>, SearchString> _cache;
        private bool _filledCache;
        private object _locker = new object();

        public CachedSearchStringRepo(ISearchStringRepo searchStringRepo) 
            : base(searchStringRepo)
        {
            _cache = new Dictionary<Tuple<int, int>, SearchString>();
        }

        public override SearchString Select(int searchEngineId, int regionId)
        {
            Tuple<int, int> key = new Tuple<int, int>(searchEngineId, regionId);
            SearchString searchString = null;
            
            lock (_locker)
            {
                if (_cache.ContainsKey(key))
                {
                    searchString = _cache[key];
                }
                // (searchEngineId, regionId) pair does not exist
                // if the cache has already been filled, exit 
                // early to avoid IO.
                else if (!_filledCache)
                {
                    searchString = base.Select(searchEngineId, regionId);
                    if (searchString != null)
                    {
                        _cache.Add(key, searchString);
                    }
                }
            }

            return searchString;
        }

        public override IEnumerable<SearchString> SelectAll()
        {
            lock (_locker)
            {
                if (_filledCache)
                {
                    return _cache.Values.ToList();
                }
                else
                {
                    IEnumerable<SearchString> searchStrings = base.SelectAll();
                    
                    foreach (SearchString searchString in searchStrings)
                    {
                        Tuple<int, int> key = new Tuple<int, int>(
                            searchString.SearchEngineID, searchString.RegionID);

                        _cache.Add(key, searchString);
                    }

                    _filledCache = true;

                    return searchStrings;
                }
            }
        }

        public override void Insert(SearchString searchString)
        {
            base.Insert(searchString);

            lock (_locker)
            {
                Tuple<int, int> key = new Tuple<int, int>(searchString.SearchEngineID, searchString.RegionID);
                _cache.Add(key, searchString);
            }
        }

        public override void InsertMany(IEnumerable<SearchString> searchStrings)
        {
            base.InsertMany(searchStrings);

            lock (_locker)
            {
                foreach (SearchString searchString in searchStrings)
                {
                    Tuple<int, int> key = new Tuple<int, int>(searchString.SearchEngineID, searchString.RegionID);
                    _cache.Add(key, searchString);
                }
            }
        }
    }
}
