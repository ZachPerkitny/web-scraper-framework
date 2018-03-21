using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Sync
{
    internal class KeywordSyncTask : ISyncTask
    {
        private readonly IDataStore _dataStore;
        private readonly IKeywordRepo _keywordRepo;

        public KeywordSyncTask(IDataStore dataStore, IKeywordRepo keywordRepo)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            _keywordRepo = keywordRepo ?? throw new ArgumentNullException(nameof(keywordRepo));
        }

        public int Order { get; set; }

        public async Task Execute()
        {
            byte[] latestRevision = _keywordRepo.GetLatestRevision();
            IEnumerable<Keyword> keywords = (latestRevision == null) ?
                await _dataStore.SelectKeywords() :
                await _dataStore.SelectKeywords(latestRevision);

            if (keywords.Any())
            {
                _keywordRepo.InsertMany(keywords);
            }
        }
    }
}
