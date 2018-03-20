using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    public interface ILocalUULERepo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="localUULE"></param>
        void Insert(LocalUULE localUULE);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localUULEs"></param>
        void InsertMany(IEnumerable<LocalUULE> localUULEs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        LocalUULE Select(int cityId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<LocalUULE> SelectAll();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ulong Count();
    }
}