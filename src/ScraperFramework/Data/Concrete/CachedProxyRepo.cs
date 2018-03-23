using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data.Concrete
{
    class CachedProxyRepo : ProxyRepoDecorator
    {
        private readonly Dictionary<int, Proxy> _cache;
        private bool _filledCache;
        private readonly object _locker = new object();

        public CachedProxyRepo(IProxyRepo proxyRepo)
            : base(proxyRepo)
        {
            _cache = new Dictionary<int, Proxy>();
        }

        public override Proxy Select(int proxyId)
        {
            Proxy proxy = null;

            lock (_locker)
            {
                if (_cache.ContainsKey(proxyId))
                {
                    proxy = _cache[proxyId];
                }
                else if (!_filledCache)
                {
                    proxy = base.Select(proxyId);
                    _cache[proxyId] = proxy;
                }
            }

            return proxy;
        }

        public override IEnumerable<Proxy> SelectAll()
        {
            lock (_locker)
            {
                if (_filledCache)
                {
                    return _cache.Values;
                }
                else
                {
                    IEnumerable<Proxy> proxies = base.SelectAll();

                    foreach (Proxy proxy in proxies)
                    {
                        _cache[proxy.ID] = proxy;
                    }

                    _filledCache = true;

                    return proxies;
                }
            }
        }

        public override void Insert(Proxy proxy)
        {
            base.Insert(proxy);

            lock (_locker)
            {
                _cache[proxy.ID] = proxy;
            }
        }

        public override void InsertMany(IEnumerable<Proxy> proxies)
        {
            base.InsertMany(proxies);

            lock (_locker)
            {
                foreach (Proxy proxy in proxies)
                {
                    _cache[proxy.ID] = proxy;
                }
            }
        }
    }
}
