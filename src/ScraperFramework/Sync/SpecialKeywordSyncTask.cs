using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Sync
{
    class SpecialKeywordSyncTask : ISyncTask
    {
        private readonly IDataStore _dataStore;
        private readonly ISpecialKeywordRepo _specialKeywordRepo;

        public SpecialKeywordSyncTask(IDataStore dataStore, ISpecialKeywordRepo specialKeywordRepo)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            _specialKeywordRepo = specialKeywordRepo ?? throw new ArgumentNullException(nameof(specialKeywordRepo));
        }

        public int Order { get; set; }

        public async Task Execute()
        {
            byte[] latestRevision = _specialKeywordRepo.GetLatestRevision();
            IEnumerable<SpecialKeyword> specialKeywords = (latestRevision == null) ?
                await _dataStore.SelectSpecialKeywords() :
                await _dataStore.SelectSpecialKeywords(latestRevision);

            Log.Information("{0} Special Keyword(s) Updated or Added since last sync.", specialKeywords.Count());
            if (specialKeywords.Any())
            {
                _specialKeywordRepo.InsertMany(specialKeywords);
            }
        }
    }
}
