using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Sync
{
    class ProxyMultiplierSyncTask : ISyncTask
    {
        private readonly IDataStore _dataStore;
        private readonly IProxyMultiplierRepo _proxyMultiplierRepo;

        public ProxyMultiplierSyncTask(IDataStore dataStore, IProxyMultiplierRepo proxyMultiplierRepo)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            _proxyMultiplierRepo = proxyMultiplierRepo ?? throw new ArgumentNullException(nameof(proxyMultiplierRepo));
        }

        public int Order { get; set; }

        public async Task Execute()
        {
            byte[] latestRevision = _proxyMultiplierRepo.GetLatestRevision();
            IEnumerable<ProxyMultiplier> proxyMultipliers = (latestRevision == null) ?
                await _dataStore.SelectProxyMultipliers() :
                await _dataStore.SelectProxyMultipliers(latestRevision);

            Log.Information("{0} Proxy Multiplier(s) Updated or Added since last sync.", proxyMultipliers.Count());
            if (proxyMultipliers.Any())
            {
                _proxyMultiplierRepo.InsertMany(proxyMultipliers);
            }
        }
    }
}
