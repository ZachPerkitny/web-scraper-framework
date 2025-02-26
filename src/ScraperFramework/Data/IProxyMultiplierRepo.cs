﻿using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    public interface IProxyMultiplierRepo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxyMultiplier"></param>
        void Insert(ProxyMultiplier proxyMultiplier);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxyMultipliers"></param>
        void InsertMany(IEnumerable<ProxyMultiplier> proxyMultipliers);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <param name="regionId"></param>
        /// <param name="proxyId"></param>
        /// <returns></returns>
        ProxyMultiplier Select(int searchEngineId, int regionId, int proxyId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProxyMultiplier> SelectAll();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ulong Count();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ProxyMultiplier Max();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ProxyMultiplier Min();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        byte[] GetLatestRevision();
    }
}
