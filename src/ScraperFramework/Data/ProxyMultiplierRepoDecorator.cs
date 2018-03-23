using System;
using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    public abstract class ProxyMultiplierRepoDecorator : IProxyMultiplierRepo
    {
        private readonly IProxyMultiplierRepo _proxyMultiplierRepo;

        public ProxyMultiplierRepoDecorator(IProxyMultiplierRepo proxyMultiplierRepo)
        {
            _proxyMultiplierRepo = proxyMultiplierRepo ?? throw new ArgumentNullException(nameof(proxyMultiplierRepo));
        }

        public virtual void Insert(ProxyMultiplier proxyMultiplier)
        {
            _proxyMultiplierRepo.Insert(proxyMultiplier);
        }

        public virtual void InsertMany(IEnumerable<ProxyMultiplier> proxyMultipliers)
        {
            _proxyMultiplierRepo.InsertMany(proxyMultipliers);
        }

        public virtual ProxyMultiplier Select(int searchEngineId, int regionId, int proxyId)
        {
            return _proxyMultiplierRepo.Select(searchEngineId, regionId, proxyId);
        }

        public virtual IEnumerable<ProxyMultiplier> SelectAll()
        {
            return _proxyMultiplierRepo.SelectAll();
        }

        public virtual ulong Count()
        {
            return _proxyMultiplierRepo.Count();
        }

        public virtual ProxyMultiplier Max()
        {
            return _proxyMultiplierRepo.Max();
        }

        public virtual ProxyMultiplier Min()
        {
            return _proxyMultiplierRepo.Min();
        }

        public virtual byte[] GetLatestRevision()
        {
            return _proxyMultiplierRepo.GetLatestRevision();
        }
    }
}
