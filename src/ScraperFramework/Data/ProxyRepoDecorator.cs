using System;
using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    public abstract class ProxyRepoDecorator : IProxyRepo
    {
        private readonly IProxyRepo _proxyRepo;

        public ProxyRepoDecorator(IProxyRepo proxyRepo)
        {
            _proxyRepo = proxyRepo ?? throw new ArgumentNullException(nameof(proxyRepo));
        }

        public virtual void Insert(Proxy proxy)
        {
            _proxyRepo.Insert(proxy);
        }

        public virtual void InsertMany(IEnumerable<Proxy> proxies)
        {
            _proxyRepo.InsertMany(proxies);
        }

        public virtual Proxy Select(int proxyId)
        {
            return _proxyRepo.Select(proxyId);
        }

        public virtual IEnumerable<Proxy> SelectAll()
        {
            return _proxyRepo.SelectAll();
        }

        public virtual ulong Count()
        {
            return _proxyRepo.Count();
        }

        public virtual Proxy Max()
        {
            return _proxyRepo.Max();
        }

        public virtual Proxy Min()
        {
            return _proxyRepo.Min();
        }

        public virtual byte[] GetLatestRevision()
        {
            return _proxyRepo.GetLatestRevision();
        }
    }
}
