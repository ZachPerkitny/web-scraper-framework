using System;
using System.Collections.Generic;
using ScraperFramework.Pocos;
using ScraperFramework.Shared.Enum;

namespace ScraperFramework
{
    public interface IProxyManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<Proxy> GetAvailableProxies();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        DateTime GetNextAvailability();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxyId"></param>
        /// <param name="crawlResultID"></param>
        void MarkAsUsed(int proxyId, CrawlResultID crawlResultID);
    }
}
