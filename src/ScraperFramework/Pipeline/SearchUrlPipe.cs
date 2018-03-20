using System;
using System.Collections.Generic;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;
using ScraperFramework.Pocos;
using WebScraper.Pocos;

namespace ScraperFramework.Pipeline
{
    internal class SearchUrlPipe : Pipe<PipelinedCrawlDescription>
    {
        private readonly ILocalUULERepo _localUULERepo;
        private readonly ISearchStringRepo _searchStringRepo;

        public SearchUrlPipe(ILocalUULERepo localUULERepo, ISearchStringRepo searchStringRepo)
        {
            _localUULERepo = localUULERepo ?? throw new ArgumentNullException(nameof(localUULERepo));
            _searchStringRepo = searchStringRepo ?? throw new ArgumentNullException(nameof(searchStringRepo));
        }

        public override PipelinedCrawlDescription Flow(PipelinedCrawlDescription pipelinedCrawlDescription)
        {
            IEnumerable<CrawlDescription> crawlDescriptions = pipelinedCrawlDescription.CrawlDescriptions;
            foreach (CrawlDescription crawlDescription in crawlDescriptions)
            {
                SearchString searchString = _searchStringRepo.Select(
                    crawlDescription.SearchEngineID, crawlDescription.RegionID);

                if (searchString != null)
                {
                    string naturalResultParamString = string.Empty;
                    if (crawlDescription.IsNatural)
                    {
                        naturalResultParamString = searchString.NaturalResultParamString;
                    }

                    string UULE = string.Empty;
                    if (crawlDescription.CityID > 0)
                    {
                        LocalUULE localUULE = _localUULERepo.Select(crawlDescription.CityID);
                        if (localUULE != null)
                        {
                            UULE = $"&uule={localUULE.UULE}";
                        }
                    }

                    crawlDescription.SearchString =
                        $"{searchString.SearchEngine}{searchString.SearchEngineURL}{naturalResultParamString}{UULE}";
                }
            }

            if (_connection != null)
            {
                return _connection.Flow(pipelinedCrawlDescription);
            }

            return pipelinedCrawlDescription;
        }
    }
}
