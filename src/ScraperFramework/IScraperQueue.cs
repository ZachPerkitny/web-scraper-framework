using System;
using System.Threading.Tasks;
using ScraperFramework.Shared.Pocos;

namespace ScraperFramework
{
    public interface IScraperQueue : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<CrawlDescription> Dequeue();
    }
}
