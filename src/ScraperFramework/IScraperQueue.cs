using System.Threading.Tasks;
using ScraperFramework.Pocos;

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
