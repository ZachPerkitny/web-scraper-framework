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
        private const int SUCCESS_FALLBACK_DELAY = 1;
        private const int FAILURE_DELAY = 2;
        private const int CAPTCHA_DELAY = 6;
        private const int BLOCK_DELAY = 20;

        private const int DELAY_JITTER = 500;

        private const int START_LOWER_BOUND = 0;
        private const int START_UPPER_BOUND = 600;

        private readonly IProxyRepo _proxyRepo;
        private readonly IProxyMultiplierRepo _proxyMultiplierRepo;
        private readonly ISearchEngineRepo _searchEngineRepo;
        private readonly ISearchStringRepo _searchStringRepo;

        private readonly Random _random = new Random();

        private readonly object _locker = new object();

        // Tuple <SEID, RID>
        private readonly Dictionary<Tuple<short, short>, Dictionary<int, ProxyStatus>> _proxyStatuses;
        
        private bool _addedInitStatuses = false;

        public ProxyManager(IProxyRepo proxyRepo, IProxyMultiplierRepo proxyMultiplierRepo, 
            ISearchEngineRepo searchEngineRepo, ISearchStringRepo searchStringRepo)
        {
            _proxyRepo = proxyRepo ?? throw new ArgumentNullException(nameof(proxyRepo));
            _proxyMultiplierRepo = proxyMultiplierRepo ?? throw new ArgumentNullException(nameof(proxyMultiplierRepo));
            _searchEngineRepo = searchEngineRepo ?? throw new ArgumentNullException(nameof(searchEngineRepo));
            _searchStringRepo = searchStringRepo ?? throw new ArgumentNullException(nameof(searchStringRepo));

            _proxyStatuses = new Dictionary<Tuple<short, short>, Dictionary<int, ProxyStatus>>();
        }

        public IEnumerable<Proxy> GetAvailableProxies(short searchEngineId, short regionId, bool autoLock)
        {
            lock (_locker)
            {
                // TODO(zvp): Should we handle this differently ?
                if (!_addedInitStatuses)
                {
                    InitializeProxyStatuses();
                }

                // exit early if no proxes are available for the seid/region pair
                if (GetNextAvailabilityFor(searchEngineId, regionId) > DateTime.Now)
                {
                    // empty list, i guess ?
                    return new List<Proxy>();
                }

                List<Proxy> proxies = GetAvailableProxies(searchEngineId, regionId)
                    .Select(p => CreateProxyDetails(searchEngineId, regionId, p.Key))
                    .Concat(GetAvailableGlobalProxies(searchEngineId)
                        .Select(p => CreateProxyDetails(searchEngineId, 0, p.Key)))
                    .ToList();

                if (autoLock)
                {
                    // lock proxies until marked as used
                    foreach (Proxy proxy in proxies)
                    {
                        Tuple<short, short> key = new Tuple<short, short>(proxy.SearchEngineID, proxy.RegionID);
                        _proxyStatuses[key][proxy.ProxyID].IsLocked = true;
                    }
                }

                return proxies;
            }
        }

        public Proxy GetAvailableProxy(short searchEngineId, short regionId, bool autoLock = true)
        {
            lock (_locker)
            {
                // TODO(zvp): Should we handle this differently ?
                if (!_addedInitStatuses)
                {
                    InitializeProxyStatuses();
                }

                // exit early if no proxes are available for the seid/region pair
                if (GetNextAvailabilityFor(searchEngineId, regionId) > DateTime.Now)
                {
                    return null;
                }

                int proxyId = GetAvailableProxies(searchEngineId, regionId)
                    .FirstOrDefault().Key;
                if (proxyId > 0)
                {
                    Proxy proxy = CreateProxyDetails(searchEngineId, regionId, proxyId);
                    if (autoLock)
                    {
                        Tuple<short, short> key = new Tuple<short, short>(searchEngineId, regionId);
                        _proxyStatuses[key][proxyId].IsLocked = true;
                    }

                    return proxy;
                }

                //fall back to global
                proxyId = GetAvailableGlobalProxies(searchEngineId)
                    .FirstOrDefault().Key;
                if (proxyId > 0)
                {
                    Proxy proxy = CreateProxyDetails(searchEngineId, 0, proxyId);
                    if (autoLock)
                    {
                        Tuple<short, short> key = new Tuple<short, short>(searchEngineId, 0);
                        _proxyStatuses[key][proxyId].IsLocked = true;
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
                // TODO(zvp): Should we handle this differently ?
                if (!_addedInitStatuses)
                {
                    InitializeProxyStatuses();
                }

                return _proxyStatuses
                    .Select(p => GetNextAvailabilityFor(p.Key.Item1, p.Key.Item2))
                    .Min();
            }
        }

        public DateTime GetNextAvailability(IEnumerable<Tuple<short, short>> searchEngineRegionPairs)
        {
            lock (_locker)
            {
                // TODO(zvp): Should we handle this differently ?
                if (!_addedInitStatuses)
                {
                    InitializeProxyStatuses();
                }

                return searchEngineRegionPairs
                    .Select(p => GetNextAvailabilityFor(p.Item1, p.Item2))
                    .Min();
            }
        }

        public DateTime GetNextAvailability(short searchEngineId, short regionId)
        {
            lock (_locker)
            {
                // TODO(zvp): Should we handle this differently ?
                if (!_addedInitStatuses)
                {
                    InitializeProxyStatuses();
                }

                return GetNextAvailabilityFor(searchEngineId, regionId);
            }
        }

        public void Lock(short searchEngineId, short regionId, int proxyId)
        {
            lock (_locker)
            {
                // TODO(zvp): Should we handle this differently ?
                if (!_addedInitStatuses)
                {
                    InitializeProxyStatuses();
                }

                Tuple<short, short> key = new Tuple<short, short>(searchEngineId, regionId);
                if (_proxyStatuses.ContainsKey(key) && 
                    _proxyStatuses[key].ContainsKey(proxyId))
                {
                    _proxyStatuses[key][proxyId].IsLocked = true;
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
                // TODO(zvp): Should we handle this differently ?
                if (!_addedInitStatuses)
                {
                    InitializeProxyStatuses();
                }

                Tuple<short, short> key = new Tuple<short, short>(searchEngineId, regionId);
                if (_proxyStatuses.ContainsKey(key) &&
                    _proxyStatuses[key].ContainsKey(proxyId))
                {
                    ProxyStatus status = _proxyStatuses[key][proxyId];
                    status.IsLocked = false; // unlock proxy

                    // update next availability based on crawl result
                    switch (crawlResultID)
                    {
                        case CrawlResultID.Success:
                        {
                            Data.Entities.ProxyMultiplier multipler = _proxyMultiplierRepo.Select(
                                searchEngineId, regionId, proxyId);
                            if (multipler != null)
                            {
                                status.NextAvailability = DateTime.Now.AddSeconds(multipler.Multiplier);
                            }
                            else
                            {
                                Data.Entities.SearchString searchString = _searchStringRepo
                                    .Select(searchEngineId, regionId);
                                if (searchString != null)
                                {
                                    status.NextAvailability = DateTime.Now.AddSeconds(searchString.DelayMultiplier);
                                }
                                else
                                {
                                    status.NextAvailability = DateTime.Now.AddMinutes(SUCCESS_FALLBACK_DELAY);
                                }
                            }
                            
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
        /// <returns></returns>
        private DateTime GetNextAvailabilityFor(short searchEngineId, short regionId)
        {
            Tuple<short, short> key = new Tuple<short, short>(searchEngineId, regionId);
            if (_proxyStatuses.ContainsKey(key))
            {
                return _proxyStatuses[key]
                    .Where(p => !p.Value.IsLocked)
                    .Min(p => p.Value.NextAvailability);
            }

            // fallback to global
            key = new Tuple<short, short>(searchEngineId, 0);
            if (_proxyStatuses.ContainsKey(key))
            {
                return _proxyStatuses[key]
                    .Where(p => !p.Value.IsLocked)
                    .Min(p => p.Value.NextAvailability);
            }

            return default(DateTime);
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxyStatus"></param>
        /// <returns></returns>
        private bool IsProxyAvailable(ProxyStatus proxyStatus)
            => !proxyStatus.IsLocked && 
            proxyStatus.NextAvailability <= DateTime.Now.AddMilliseconds(_random.Next(-DELAY_JITTER, DELAY_JITTER));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        private bool IsProxyForSearchEngineRegionPair(Tuple<short, short> key, short searchEngineId, short regionId)
            => key.Item1 == searchEngineId && (key.Item2 == regionId || key.Item2 == 0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <param name="regionId"></param>
        /// <param name="proxyId"></param>
        /// <returns></returns>
        private Proxy CreateProxyDetails(short searchEngineId, short regionId, int proxyId)
        {
            Data.Entities.Proxy proxy = _proxyRepo.Select(proxyId); // proxy id
            return new Proxy
            {
                ProxyID = proxy.ID,
                IP = proxy.IP,
                Port = proxy.Port,
                SearchEngineID = searchEngineId,
                RegionID = regionId
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        private IEnumerable<KeyValuePair<int, ProxyStatus>> GetAvailableProxies(short searchEngineId, short regionId)
        {
            if (_proxyStatuses.ContainsKey(new Tuple<short, short>(searchEngineId, regionId)))
            {
                return _proxyStatuses[new Tuple<short, short>(searchEngineId, regionId)]
                    .Where(p => IsProxyAvailable(p.Value));
            }

            return new List<KeyValuePair<int, ProxyStatus>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <returns></returns>
        private IEnumerable<KeyValuePair<int, ProxyStatus>> GetAvailableGlobalProxies(short searchEngineId)
        {
            if (_proxyStatuses.ContainsKey(new Tuple<short, short>(searchEngineId, 0)))
            {
                return _proxyStatuses[new Tuple<short, short>(searchEngineId, 0)]
                    .Where(p => IsProxyAvailable(p.Value));
            }

            return new List<KeyValuePair<int, ProxyStatus>>();
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
                    Tuple<short, short> key = new Tuple<short, short>(searchEngine.ID, proxy.RegionID);
                    if (!_proxyStatuses.ContainsKey(key))
                    {
                        _proxyStatuses[key] = new Dictionary<int, ProxyStatus>();
                    }

                    _proxyStatuses[key].Add(proxy.ID, new ProxyStatus
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

        /// <summary>
        /// Represents the state of a proxy
        /// </summary>
        private class ProxyStatus
        {
            public bool IsLocked { get; set; }

            public DateTime NextAvailability { get; set; }
        }
    }
}
