using System;
using ScraperFramework.Pocos;

namespace ScraperFramework
{
    public interface IScraperQueue
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        CrawlDescription Dequeue();
    }
}
