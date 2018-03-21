using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Sync
{
    class SearchStringSyncTask : ISyncTask
    {
        private readonly IDataStore _dataStore;
        private readonly ISearchStringRepo _searchStringRepo;

        public SearchStringSyncTask(IDataStore dataStore, ISearchStringRepo searchStringRepo)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            _searchStringRepo = searchStringRepo ?? throw new ArgumentNullException(nameof(searchStringRepo));
        }

        public int Order { get; set; }

        public async Task Execute()
        {
            byte[] latestRevision = _searchStringRepo.GetLatestRevision();
            IEnumerable<SearchString> searchStrings = (latestRevision == null) ?
                await _dataStore.SelectSearchStrings() :
                await _dataStore.SelectSearchStrings(latestRevision);

            Log.Information("{0} Search String(s) Updated or Added since last sync.", searchStrings.Count());
            if (searchStrings.Any())
            {
                _searchStringRepo.InsertMany(searchStrings);
            }
        }
    }
}
