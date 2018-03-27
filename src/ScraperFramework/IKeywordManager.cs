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
        /// <returns></returns>
        IEnumerable<Keyword> GetKeywordsToCrawl(int count);
    }
}
