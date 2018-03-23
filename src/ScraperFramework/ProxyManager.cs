using System;
using System.Collections.Generic;
using ScraperFramework.Pocos;
using ScraperFramework.Shared.Enum;

namespace ScraperFramework
{
    class ProxyManager : IProxyManager
    {
        /// <summary>
        /// Describes the current state of
        /// a proxy
        /// </summary>
        private enum ProxyState
        {
            Available,
            CoolDown,
            Locked
        }

        public IEnumerable<Proxy> GetAvailableProxies()
        {
            throw new NotImplementedException();
        }

        public DateTime GetNextAvailability()
        {
            throw new NotImplementedException();
        }

        public void MarkAsUsed(int proxyId, CrawlResultID crawlResultID)
        {
            throw new NotImplementedException();
        }
    }
}
