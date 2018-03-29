using System.Collections.Generic;
using ScraperFramework.Pocos;

namespace ScraperFramework
{
    public interface IKeywordManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="mayBeRequeued"></param>
        /// <returns></returns>
        IEnumerable<Keyword> GetKeywords(int count, bool mayBeRequeued = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <param name="regionId"></param>
        /// <param name="keywordId"></param>
        /// <returns></returns>
        void RequeueKeyword(short searchEngineId, short regionId, int keywordId);
    }
}
