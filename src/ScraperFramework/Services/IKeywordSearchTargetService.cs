using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Services
{
    public interface IKeywordSearchTargetService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywords"></param>
        Task InsertKeywords(IEnumerable<string> keywords);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Keyword> GetKeyword(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Keyword>> GetAllKeywords();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTargets"></param>
        Task AddSearchTargets(IEnumerable<SearchTarget> searchTargets);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywordSearchTargets"></param>
        Task AddKeywordSearchTargets(IEnumerable<KeywordSearchTarget> keywordSearchTargets);
    }
}
