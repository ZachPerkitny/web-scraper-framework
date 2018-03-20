using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    public interface IInternationalUULERepo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="internationalUULE"></param>
        void Insert(InternationalUULE internationalUULE);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="internationalUULEs"></param>
        void InsertMany(IEnumerable<InternationalUULE> internationalUULEs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        InternationalUULE Select(int regionId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<InternationalUULE> SelectAll();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ulong Count();
    }
}