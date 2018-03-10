using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Services.Concrete
{
    class KeywordSearchTargetService : IKeywordSearchTargetService
    {
        private readonly IKeywordRepo _keywordRepo;
        private readonly ISearchTargetRepo _searchTargetRepo;

        public KeywordSearchTargetService(IKeywordRepo keywordRepo, ISearchTargetRepo searchTargetRepo)
        {
            _keywordRepo = keywordRepo;
            _searchTargetRepo = searchTargetRepo;
        }

        public Task InsertKeywords(IEnumerable<string> keywords)
        {
            return Task.Run(() => _keywordRepo.InsertMany(keywords));
        }

        public Task<Keyword> GetKeyword(int id)
        {
            return Task.Run(() => _keywordRepo.Select(id));
        }

        public Task<IEnumerable<Keyword>> GetAllKeywords()
        {
            return Task.Run(() => _keywordRepo.SelectAll());
        }

        public Task AddSearchTargets(IEnumerable<SearchTarget> searchTargets)
        {
            throw new NotImplementedException();
        }

        public Task AddKeywordSearchTargets(IEnumerable<KeywordSearchTarget> keywordSearchTargets)
        {
            throw new NotImplementedException();
        }
    }
}
