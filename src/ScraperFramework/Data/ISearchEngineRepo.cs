using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    public interface ISearchEngineRepo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngine"></param>
        void Insert(SearchEngine searchEngine);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngines"></param>
        void InsertMany(IEnumerable<SearchEngine> searchEngines);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <returns></returns>
        SearchEngine Select(int searchEngineId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<SearchEngine> SelectAll();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ulong Count();
    }
}