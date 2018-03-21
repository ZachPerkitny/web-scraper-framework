using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Sync
{
    internal class SearchEngineSyncTask : ISyncTask
    {
        private readonly IDataStore _dataStore;
        private readonly ISearchEngineRepo _searchEngineRepo;

        public SearchEngineSyncTask(IDataStore dataStore, ISearchEngineRepo searchEngineRepo)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            _searchEngineRepo = searchEngineRepo ?? throw new ArgumentNullException(nameof(searchEngineRepo));
        }

        public int Order { get; set; }

        public async Task Execute()
        {
            byte[] latestRevision = _searchEngineRepo.GetLatestRevision();
            IEnumerable<SearchEngine> searchEngines = (latestRevision == null) ? 
                await _dataStore.SelectSearchEngines() : 
                await _dataStore.SelectSearchEngines(latestRevision);

            if (searchEngines.Any())
            {
                _searchEngineRepo.InsertMany(searchEngines);
            }
        }
    }
}
