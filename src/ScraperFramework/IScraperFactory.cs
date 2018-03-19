using System.Threading;
using ScraperFramework.Utils;

namespace ScraperFramework
{
    public interface IScraperFactory
    {
        /// <summary>
        /// Creates a new Scraper (this needs to be refactored)
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IScraper Create(AsyncManualResetEvent manualResetEvent, CancellationToken cancellationToken = default(CancellationToken));
    }
}
