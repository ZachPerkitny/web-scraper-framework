using System.Threading;

namespace ScraperFramework
{
    public interface IScraperFactory
    {
        /// <summary>
        /// Creates a new Scraper
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IScraper Create(CancellationToken cancellationToken);
    }
}
