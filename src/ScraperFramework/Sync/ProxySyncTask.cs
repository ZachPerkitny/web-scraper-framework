using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Sync
{
    internal class ProxySyncTask : ISyncTask
    {
        private readonly IDataStore _dataStore;
        private readonly IProxyRepo _proxyRepo;

        public ProxySyncTask(IDataStore dataStore, IProxyRepo proxyRepo)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            _proxyRepo = proxyRepo ?? throw new ArgumentNullException(nameof(proxyRepo));
        }

        public int Order { get; set; }

        public async Task Execute()
        {
            byte[] latestRevision = _proxyRepo.GetLatestRevision();
            IEnumerable<Proxy> proxies = (latestRevision == null) ?
                await _dataStore.SelectProxies() :
                await _dataStore.SelectProxies(latestRevision);

            Log.Information("{0} Proxy(s) Updated or Added since last sync.", proxies.Count());
            if (proxies.Any())
            {
                _proxyRepo.InsertMany(proxies);
            }
        }
    }
}
