using System;
using System.Collections.Generic;
using ScraperFramework.Pocos;

namespace ScraperFramework
{
    public interface IKeywordManager : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        IEnumerable<Keyword> GetKeywords(int count);
    }
}
