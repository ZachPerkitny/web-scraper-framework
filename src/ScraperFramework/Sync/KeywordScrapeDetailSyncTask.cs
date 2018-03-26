using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Sync
{
    class KeywordScrapeDetailSyncTask : ISyncTask
    {
        private readonly int _scraperNo;
        private readonly IDataStore _dataStore;
        private readonly IKeywordScrapeDetailRepo _keywordScrapeDetailRepo;

        public KeywordScrapeDetailSyncTask(int scraperNo, IDataStore dataStore,
            IKeywordScrapeDetailRepo keywordScrapeDetailRepo)
        {
            if (scraperNo <= 0)
            {
                throw new ArgumentException("Invalid Scraper No.");
            }

            _scraperNo = scraperNo;
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            _keywordScrapeDetailRepo = keywordScrapeDetailRepo ?? throw new ArgumentNullException(nameof(keywordScrapeDetailRepo));
        }

        public int Order { get; set; }

        public async Task Execute()
        {
            byte[] latestRevision = _keywordScrapeDetailRepo.GetLatestRevision();
            IEnumerable<KeywordScrapeDetail> keywordScrapeDetails = (latestRevision == null) ?
                await _dataStore.SelectKeywordScrapeDetails(_scraperNo) :
                await _dataStore.SelectKeywordScrapeDetails(_scraperNo, latestRevision);

            Log.Information("{0} Keyword Scrape Details(s) Updated or Added since last sync.", 
                keywordScrapeDetails.Count());
            if (keywordScrapeDetails.Any())
            {
                _keywordScrapeDetailRepo.InsertMany(keywordScrapeDetails);
            }
        }
    }
}
