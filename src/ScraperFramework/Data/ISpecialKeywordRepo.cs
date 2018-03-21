using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    interface ISpecialKeywordRepo
    {
        /// <summary>
        /// Inserts a single special keyword entity.
        /// </summary>
        /// <param name="specialKeyword">Keyword to insert</param>
        void Insert(SpecialKeyword specialKeyword);

        /// <summary>
        /// Inserts multiple special keyword entities.
        /// </summary>
        /// <param name="specialKeywords">List of keywords to insert</param>
        void InsertMany(IEnumerable<SpecialKeyword> specialKeywords);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <param name="regionId"></param>
        /// <param name="keywordId"></param>
        /// <returns></returns>
        SpecialKeyword Select(int searchEngineId, int regionId, int keywordId);

        /// <summary>
        /// Selects all keywords
        /// </summary>
        /// <returns>All Keywords</returns>
        IEnumerable<SpecialKeyword> SelectAll();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ulong Count();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        SpecialKeyword Max();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        SpecialKeyword Min();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        byte[] GetLatestRevision();
    }
}
