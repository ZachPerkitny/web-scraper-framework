using System;
using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data.Concrete
{
    public abstract class SearchStringRepoDecorator : ISearchStringRepo
    {
        private readonly ISearchStringRepo _searchStringRepo;

        public SearchStringRepoDecorator(ISearchStringRepo searchStringRepo)
        {
            _searchStringRepo = searchStringRepo ?? throw new ArgumentNullException(nameof(searchStringRepo));
        }

        public virtual void Insert(SearchString searchString)
        {
            _searchStringRepo.Insert(searchString);
        }

        public virtual void InsertMany(IEnumerable<SearchString> searchStrings)
        {
            _searchStringRepo.InsertMany(searchStrings);
        }

        public virtual SearchString Select(int searchStringId)
        {
            return _searchStringRepo.Select(searchStringId);
        }

        public virtual SearchString Select(int searchEngineId, int regionId)
        {
            return _searchStringRepo.Select(searchEngineId, regionId);
        }

        public virtual IEnumerable<SearchString> SelectAll()
        {
            return _searchStringRepo.SelectAll();
        }

        public virtual ulong Count()
        {
            return _searchStringRepo.Count();
        }
    }
}
