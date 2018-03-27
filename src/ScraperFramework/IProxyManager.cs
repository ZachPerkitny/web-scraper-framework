using System;
using System.Collections.Generic;
using ScraperFramework.Pocos;
using ScraperFramework.Shared.Enum;

namespace ScraperFramework
{
    public interface IProxyManager
    {
        /// <summary>
        /// Gets the proxies that have cooldowned and are not locked.
        /// If autoLock is set to false, a manual call to Lock is 
        /// required to prevent the same proxy from getting picked
        /// up.
        /// </summary>
        /// <param name="autoLock"></param>
        /// <returns></returns>
        IEnumerable<Proxy> GetAvailableProxies(bool autoLock = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <param name="regionId"></param>
        /// <param name="autoLock"></param>
        /// <returns></returns>
        Proxy GetAvailableProxy(short searchEngineId, short regionId, bool autoLock = true);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        DateTime GetNextAvailability();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        DateTime GetNextAvailability(short searchEngineId, short regionId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <param name="regionId"></param>
        /// <param name="proxyId"></param>
        void Lock(short searchEngineId, short regionId, int proxyId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <param name="regionId"></param>
        /// <param name="proxyId"></param>
        /// <param name="crawlResultID"></param>
        void UnLock(short searchEngineId, short regionId, int proxyId, CrawlResultID crawlResultID);
    }
}
