using System;
using System.Collections.Generic;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;
using ScraperFramework.Pocos;
using ScraperFramework.Shared.Pocos;

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
            LinkedListNode<CrawlDescription> node = pipelinedCrawlDescription.CrawlDescriptions.First;
            while (node != null)
            {
                SearchString searchString = _searchStringRepo.Select(
                    node.Value.SearchEngineID, node.Value.RegionID);
                if (searchString != null)
                {
                    string naturalResultParamString = string.Empty;
                    if (node.Value.IsNatural)
                    {
                        naturalResultParamString = searchString.NaturalResultParamString;
                    }

                    string UULE = string.Empty;
                    if (node.Value.CityID > 0)
                    {
                        LocalUULE localUULE = _localUULERepo.Select(node.Value.CityID);
                        if (localUULE != null)
                        {
                            UULE = $"&uule={localUULE.UULE}";
                        }
                    }

                    node.Value.SearchString =
                        $"{searchString.SearchEngine}{searchString.SearchEngineURL}{naturalResultParamString}{UULE}";
                }
                else
                {
                    // mark the node for removal, no
                    // search string exists for the
                    // seid, reid tuple
                    pipelinedCrawlDescription.MarkedForRemoval.Add(node);
                }

                node = node.Next;
            }

            if (_connection != null)
            {
                _connection.Flow(pipelinedCrawlDescription);
            }

            return pipelinedCrawlDescription;
        }
    }
}
