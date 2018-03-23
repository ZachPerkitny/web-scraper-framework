using System;
using System.Collections.Generic;
using System.Linq;
using ScraperFramework.Data;
using ScraperFramework.Pocos;
using ScraperFramework.Shared.Enum;

namespace ScraperFramework
{
    class ProxyManager : IProxyManager
    {
        /// <summary>
        /// Represents the state of a proxy
        /// </summary>
        private class ProxyStatus
        {
            public bool IsLocked { get; set; }

            public DateTime NextAvailability { get; set; }
        }

        private readonly IProxyRepo _proxyRepo;
        private readonly IProxyMultiplierRepo _proxyMultiplierRepo;
        private readonly ISearchEngineRepo _searchEngineRepo;

        // Tuple <SEID, RID, ProxyID>
        private readonly Dictionary<Tuple<int, int, int>, ProxyStatus> _proxyStatuses;
        
        private bool _addedInitStatuses = false;

        public ProxyManager(IProxyRepo proxyRepo, IProxyMultiplierRepo proxyMultiplierRepo, 
            ISearchEngineRepo searchEngineRepo)
        {
            _proxyRepo = proxyRepo ?? throw new ArgumentNullException(nameof(proxyRepo));
            _proxyMultiplierRepo = proxyMultiplierRepo ?? throw new ArgumentNullException(nameof(proxyMultiplierRepo));
            _searchEngineRepo = searchEngineRepo ?? throw new ArgumentNullException(nameof(searchEngineRepo));

            _proxyStatuses = new Dictionary<Tuple<int, int, int>, ProxyStatus>();
        }

        public IEnumerable<Proxy> GetAvailableProxies()
        {
            if (!_addedInitStatuses)
            {
                InitializeProxyStatuses();
            }

            List<Proxy> proxies = _proxyStatuses
                .Where(p => !p.Value.IsLocked && p.Value.NextAvailability <= DateTime.Now)
                .Select(p =>
                {
                    Data.Entities.Proxy proxy = _proxyRepo.Select(p.Key.Item3); // proxy id

                    return new Proxy
                    {
                        ProxyID = proxy.ID,
                        IP = proxy.IP,
                        Port = proxy.Port,
                        SearchEngineID = p.Key.Item1,
                        RegionID = p.Key.Item2
                    };
                })
                .ToList();
            

            // lock proxies here in case
            // it throws an exception inside
            // the delegate passed to the select
            // extension, the proxy will be stuck
            // in limbo otherwise
            // TODO(zvp): Lock Timeout
            foreach (Proxy proxy in proxies)
            {
                Tuple<int, int, int> key = new Tuple<int, int, int>(
                    proxy.SearchEngineID, proxy.RegionID, proxy.ProxyID);

                _proxyStatuses[key].IsLocked = true;
            }

            return proxies;
        }

        public DateTime GetNextAvailability()
        {
            if (!_addedInitStatuses)
            {
                InitializeProxyStatuses();
            }

            return _proxyStatuses.Min(p => p.Value.NextAvailability);
        }

        public void MarkAsUsed(int searchEngineId, int regionId, int proxyId, 
            CrawlResultID crawlResultID)
        {
            Tuple<int, int, int> key = new Tuple<int, int, int>(
                searchEngineId, regionId, proxyId);

            if (_proxyStatuses.ContainsKey(key))
            {
                ProxyStatus status = _proxyStatuses[key];
                status.IsLocked = false; // unlock proxy

                // update next availability based on crawl result
                double multipler = _proxyMultiplierRepo.Select(
                    searchEngineId, regionId, proxyId).Multiplier;
                switch (crawlResultID)
                {
                    case CrawlResultID.Success:
                        status.NextAvailability = DateTime.Now.AddSeconds(multipler);
                        break;
                    case CrawlResultID.Failure:
                        // waht
                        status.NextAvailability = DateTime.Now.AddSeconds(multipler * 2);
                        break;
                    case CrawlResultID.Captcha:
                        // waht
                        status.NextAvailability = DateTime.Now.AddSeconds(multipler * 4);
                        break;
                }
            }
        }

        private void InitializeProxyStatuses()
        {
            // create search engine, region, proxy tuples
            IEnumerable<Data.Entities.SearchEngine> searchEngines = _searchEngineRepo.SelectAll();
            IEnumerable<Data.Entities.Proxy> proxies = _proxyRepo.SelectAll();
            foreach (var searchEngine in searchEngines)
            {
                foreach (var proxy in proxies)
                {
                    Tuple<int, int, int> key = new Tuple<int, int, int>(
                        searchEngine.ID, proxy.RegionID, proxy.ID);

                    _proxyStatuses.Add(key, new ProxyStatus
                    {
                        IsLocked = false,
                        NextAvailability = DateTime.Now
                    });
                }
            }

            _addedInitStatuses = true;
        }
    }
}
