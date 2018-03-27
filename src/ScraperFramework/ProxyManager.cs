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

        private readonly Random _random = new Random();

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

        public IEnumerable<Proxy> GetAvailableProxies(bool autoLock)
        {
            lock (_locker)
            {
                if (!_addedInitStatuses)
                {
                    InitializeProxyStatuses();
                }

                DateTime now = DateTime.Now;
                List<Proxy> proxies = _proxyStatuses
                    .Where(p => IsProxyAvailable(p.Value))
                    .Select(p => GetProxyFromKey(p.Key))
                    .ToList();

                if (autoLock)
                {
                    // lock proxies until marked as used
                    foreach (Proxy proxy in proxies)
                    {
                        Tuple<short, short, int> key = new Tuple<short, short, int>(
                            proxy.SearchEngineID, proxy.RegionID, proxy.ProxyID);

                        _proxyStatuses[key].IsLocked = true;
                    }
                }

                return proxies;
            }
        }

        public Proxy GetAvailableProxy(short searchEngineId, short regionId, bool autoLock = true)
        {
            lock (_locker)
            {
                if (!_addedInitStatuses)
                {
                    InitializeProxyStatuses();
                }

                Proxy proxy = _proxyStatuses
                    .Where(p => p.Key.Item1 == searchEngineId &&
                        (p.Key.Item2 == regionId || p.Key.Item2 == 0) &&
                        IsProxyAvailable(p.Value))
                    .Select(p => GetProxyFromKey(p.Key))
                    .FirstOrDefault();

                if (proxy != null)
                {
                    if (autoLock)
                    {
                        Tuple<short, short, int> key = new Tuple<short, short, int>(
                            proxy.SearchEngineID, proxy.RegionID, proxy.ProxyID);

                        _proxyStatuses[key].IsLocked = true;
                    }

                    return proxy;
                }

                return null;
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

        public DateTime GetNextAvailability(short searchEngineId, short regionId)
        {
            lock (_locker)
            {
                if (!_addedInitStatuses)
                {
                    InitializeProxyStatuses();
                }

                IEnumerable<ProxyStatus> proxies = _proxyStatuses.Where(
                    p => p.Key.Item1 == searchEngineId &&
                    (p.Key.Item2 == regionId || p.Key.Item2 == 0))
                    .Select(p => p.Value);

                DateTime min = DateTime.MaxValue;
                foreach (ProxyStatus proxy in proxies)
                {
                    if (proxy.NextAvailability < min)
                    {
                        min = proxy.NextAvailability;
                    }
                }

                return min;
            }
        }

        public void Lock(short searchEngineId, short regionId, int proxyId)
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
                    _proxyStatuses[key].IsLocked = true;
                }
                else
                {
                    // throw ?
                }
            }
        }

        public void UnLock(short searchEngineId, short regionId, int proxyId, 
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
                else
                {
                    // throw ?
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxyStatus"></param>
        /// <returns></returns>
        private bool IsProxyAvailable(ProxyStatus proxyStatus)
            => !proxyStatus.IsLocked && proxyStatus.NextAvailability <= DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Proxy GetProxyFromKey(Tuple<short, short, int> key)
        {
            Data.Entities.Proxy proxy = _proxyRepo.Select(key.Item3); // proxy id
            return new Proxy
            {
                ProxyID = proxy.ID,
                IP = proxy.IP,
                Port = proxy.Port,
                SearchEngineID = key.Item1,
                RegionID = key.Item2
            };
        }

        /// <summary>
        /// All methods that make use of the _proxyStatus dictionary
        /// should call this method prior.
        /// </summary>
        private void InitializeProxyStatuses()
        {
            // create search engine, region, proxy tuples
            IEnumerable<Data.Entities.SearchEngine> searchEngines = _searchEngineRepo.SelectAll();
            List<Data.Entities.Proxy> proxies = _proxyRepo.SelectAll().ToList();

            // shuffle
            int n = proxies.Count();
            for (int i = n - 1; i >= 1; i--)
            {
                int j = _random.Next(0, i);
                Data.Entities.Proxy temp = proxies[i];
                proxies[i] = proxies[j];
                proxies[j] = temp;
            }

            foreach (var searchEngine in searchEngines)
            {
                foreach (var proxy in proxies)
                {
                    Tuple<short, short, int> key = new Tuple<short, short, int>(
                        searchEngine.ID, proxy.RegionID, proxy.ID);

                    _proxyStatuses.Add(key, new ProxyStatus
                    {
                        IsLocked = false,
                        // forces slow start, but proxies will now be naturally distributed
                        NextAvailability = new DateTime(
                            DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                            DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, 0).AddSeconds(
                            _random.Next(START_LOWER_BOUND, START_UPPER_BOUND))
                    });
                }
            }

            _addedInitStatuses = true;
        }
    }
}
