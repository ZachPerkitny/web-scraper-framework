using System.Threading.Tasks;
using WebScraper.Pocos;

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
