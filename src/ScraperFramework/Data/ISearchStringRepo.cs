using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    public interface ISearchStringRepo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchString"></param>
        void Insert(SearchString searchString);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchStrings"></param>
        void InsertMany(IEnumerable<SearchString> searchStrings);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchStringId"></param>
        /// <returns></returns>
        SearchString Select(int searchStringId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        SearchString Select(int searchEngineId, int regionId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<SearchString> SelectAll();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ulong Count();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        SearchString Max();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        SearchString Min();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        byte[] GetLatestRevision();
    }
}
