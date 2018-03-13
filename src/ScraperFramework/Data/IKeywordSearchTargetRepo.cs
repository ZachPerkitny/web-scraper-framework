using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    public interface IKeywordSearchTargetRepo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywordSearchTarget"></param>
        void Insert(KeywordSearchTarget keywordSearchTarget);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        KeywordSearchTarget Select(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTargetId"></param>
        /// <param name="keywordId"></param>
        KeywordSearchTarget Select(int searchTargetId, int keywordId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTargetId"></param>
        /// <returns></returns>
        IEnumerable<KeywordSearchTarget> SelectMany(int searchTargetId);
    }
}
