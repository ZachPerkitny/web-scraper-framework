using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    public interface ISearchTargetRepo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTarget"></param>
        void Insert(SearchTarget searchTarget);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SearchTarget Select(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="cityId"></param>
        /// <param name="isMobile"></param>
        /// <returns></returns>
        SearchTarget Select(int countryId, int cityId, bool isMobile);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<SearchTarget> SelectAll();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ulong Count();
    }
}
