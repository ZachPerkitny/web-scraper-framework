using System;
using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data.Concrete
{
    class CachedProxyMultiplierRepo : ProxyMultiplierRepoDecorator
    {
        private readonly Dictionary<Tuple<int, int, int>, ProxyMultiplier> _cache;
        private bool _filledCache;
        private object _locker = new object();

        public CachedProxyMultiplierRepo(IProxyMultiplierRepo proxyMultiplierRepo)
            : base(proxyMultiplierRepo)
        {
            _cache = new Dictionary<Tuple<int, int, int>, ProxyMultiplier>();
        }

        public override ProxyMultiplier Select(int searchEngineId, int regionId, int proxyId)
        {
            Tuple<int, int, int> key = new Tuple<int, int, int>(
                searchEngineId, regionId, proxyId);
            ProxyMultiplier proxyMultiplier = null;

            lock (_locker)
            {
                if (_cache.ContainsKey(key))
                {
                    proxyMultiplier = _cache[key];
                }
                else if (!_filledCache)
                {
                    proxyMultiplier = base.Select(searchEngineId, regionId, proxyId);
                    _cache[key] = proxyMultiplier;
                }
            }

            return proxyMultiplier;
        }

        public override IEnumerable<ProxyMultiplier> SelectAll()
        {
            lock (_locker)
            {
                if (_filledCache)
                {
                    return _cache.Values;
                }
                else
                {
                    IEnumerable<ProxyMultiplier> proxyMultipliers = base.SelectAll();

                    foreach (ProxyMultiplier proxyMultiplier in proxyMultipliers)
                    {
                        Tuple<int, int, int> key = new Tuple<int, int, int>(
                            proxyMultiplier.SearchEngineID, proxyMultiplier.RegionID, proxyMultiplier.ProxyID);
                        _cache[key] = proxyMultiplier;
                    }

                    _filledCache = true;

                    return proxyMultipliers;
                }
            }
        }

        public override void Insert(ProxyMultiplier proxyMultiplier)
        {
            base.Insert(proxyMultiplier);

            lock (_locker)
            {
                Tuple<int, int, int> key = new Tuple<int, int, int>(
                    proxyMultiplier.SearchEngineID, proxyMultiplier.RegionID, proxyMultiplier.ProxyID);
                _cache[key] = proxyMultiplier;
            }
        }

        public override void InsertMany(IEnumerable<ProxyMultiplier> proxyMultipliers)
        {
            base.InsertMany(proxyMultipliers);

            lock (_locker)
            {
                foreach (ProxyMultiplier proxyMultiplier in proxyMultipliers)
                {
                    Tuple<int, int, int> key = new Tuple<int, int, int>(
                        proxyMultiplier.SearchEngineID, proxyMultiplier.RegionID, proxyMultiplier.ProxyID);
                    _cache[key] = proxyMultiplier;
                }
            }
        }
    }
}
