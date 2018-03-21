using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    public interface IKeywordRepo
    {
        /// <summary>
        /// Inserts a single keyword entity.
        /// </summary>
        /// <param name="keyword">Keyword to insert</param>
        void Insert(Keyword keyword);

        /// <summary>
        /// Inserts multiple keyword entities.
        /// </summary>
        /// <param name="keywords">List of keywords to insert</param>
        void InsertMany(IEnumerable<Keyword> keywords);

        /// <summary>
        /// Returns the keyword with the specified id, or null, if the
        /// keyword does not exist.
        /// </summary>
        /// <param name="keywordID"></param>
        /// <returns>Keyword object or null</returns>
        Keyword Select(int keywordID);

        /// <summary>
        /// Selects all keywords
        /// </summary>
        /// <returns>All Keywords</returns>
        IEnumerable<Keyword> SelectAll();

        /// <summary>
        /// Deletes the keyword with the specified id.
        /// </summary>
        /// <param name="keywordID"></param>
        void Delete(int keywordID);

        /// <summary>
        /// Deletes all keywords.
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ulong Count();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Keyword Max();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Keyword Min();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        byte[] GetLatestRevision();
    }
}
