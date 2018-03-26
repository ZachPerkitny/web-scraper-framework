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
        private const int FAILURE_DELAY = 2;
        private const int CAPTCHA_DELAY = 6;
        private const int BLOCK_DELAY = 20;

        private const int START_LOWER_BOUND = 10;
        private const int START_UPPER_BOUND = 600;

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

        private readonly object _locker = new object();

        // Tuple <SEID, RID, ProxyID>
        private readonly Dictionary<Tuple<short, short, int>, ProxyStatus> _proxyStatuses;
        
        private bool _addedInitStatuses = false;

        public ProxyManager(IProxyRepo proxyRepo, IProxyMultiplierRepo proxyMultiplierRepo, 
            ISearchEngineRepo searchEngineRepo)
        {
            _proxyRepo = proxyRepo ?? throw new ArgumentNullException(nameof(proxyRepo));
            _proxyMultiplierRepo = proxyMultiplierRepo ?? throw new ArgumentNullException(nameof(proxyMultiplierRepo));
            _searchEngineRepo = searchEngineRepo ?? throw new ArgumentNullException(nameof(searchEngineRepo));

            _proxyStatuses = new Dictionary<Tuple<short, short, int>, ProxyStatus>();
        }

        public IEnumerable<Proxy> GetAvailableProxies()
        {
            lock (_locker)
            {
                if (!_addedInitStatuses)
                {
                    InitializeProxyStatuses();
                }

                DateTime now = DateTime.Now;
                List<Proxy> proxies = _proxyStatuses
                    .Where(p => 
                        !p.Value.IsLocked && 
                        p.Value.NextAvailability <= now)
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

                // lock proxies until marked as used
                foreach (Proxy proxy in proxies)
                {
                    Tuple<short, short, int> key = new Tuple<short, short, int>(
                        proxy.SearchEngineID, proxy.RegionID, proxy.ProxyID);

                    _proxyStatuses[key].IsLocked = true;
                }

                return proxies;
            }
        }

        public DateTime GetNextAvailability()
        {
            lock (_locker)
            {
                if (!_addedInitStatuses)
                {
                    InitializeProxyStatuses();
                }

                DateTime min = DateTime.MaxValue;
                foreach (ProxyStatus proxy in _proxyStatuses.Values)
                {
                    if (proxy.NextAvailability < min)
                    {
                        min = proxy.NextAvailability;
                    }
                }

                return min;
            }
        }

        public void MarkAsUsed(short searchEngineId, short regionId, int proxyId, 
            CrawlResultID crawlResultID)
        {
            lock (_locker)
            {
                if (!_addedInitStatuses)
                {
                    InitializeProxyStatuses();
                }

                Tuple<short, short, int> key = new Tuple<short, short, int>(
                    searchEngineId, regionId, proxyId);

                if (_proxyStatuses.ContainsKey(key))
                {
                    ProxyStatus status = _proxyStatuses[key];
                    status.IsLocked = false; // unlock proxy

                    // update next availability based on crawl result
                    switch (crawlResultID)
                    {
                        case CrawlResultID.Success:
                        {
                            double multipler = _proxyMultiplierRepo.Select(
                                searchEngineId, regionId, proxyId).Multiplier;
                            status.NextAvailability = DateTime.Now.AddSeconds(multipler);
                            break;
                        }
                        case CrawlResultID.Failure:
                            status.NextAvailability = DateTime.Now.AddMinutes(
                                FAILURE_DELAY);
                            break;
                        case CrawlResultID.Captcha:
                            status.NextAvailability = DateTime.Now.AddMinutes(
                                CAPTCHA_DELAY);
                            break;
                        case CrawlResultID.Block:
                            status.NextAvailability = DateTime.Now.AddMinutes(
                                BLOCK_DELAY);
                            break;
                    }
                }
                // TODO(zvp): Log it not existing ?
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeProxyStatuses()
        {
            // create search engine, region, proxy tuples
            IEnumerable<Data.Entities.SearchEngine> searchEngines = _searchEngineRepo.SelectAll();
            IEnumerable<Data.Entities.Proxy> proxies = _proxyRepo.SelectAll();
            foreach (var searchEngine in searchEngines)
            {
                foreach (var proxy in proxies)
                {
                    Tuple<short, short, int> key = new Tuple<short, short, int>(
                        searchEngine.ID, proxy.RegionID, proxy.ID);

                    _proxyStatuses.Add(key, new ProxyStatus
                    {
                        IsLocked = false,
                        // 0 ms
                        NextAvailability = new DateTime(
                            DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                            DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, 0).AddSeconds(
                            (new Random()).Next(START_LOWER_BOUND, START_UPPER_BOUND))
                    });
                }
            }

            _addedInitStatuses = true;
        }
    }
}
