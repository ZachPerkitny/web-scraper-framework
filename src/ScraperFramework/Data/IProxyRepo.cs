using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    public interface IProxyRepo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        void Insert(Proxy proxy);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxies"></param>
        void InsertMany(IEnumerable<Proxy> proxies);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxyId"></param>
        /// <returns></returns>
        Proxy Select(int proxyId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<Proxy> SelectAll();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ulong Count();
    }
}
