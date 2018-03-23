using System.Threading.Tasks;
using ScraperFramework.Shared.Pocos;

namespace ScraperFramework
{
    public interface IScraperQueue
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<CrawlDescription> Dequeue();
    }
}
