using System.Collections.Generic;
using System.Threading.Tasks;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    public interface IDataStore
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Keyword>> SelectKeywords();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowVersion"></param>
        /// <returns></returns>
        Task<IEnumerable<Keyword>> SelectKeywords(byte[] rowVersion);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scraperNo"></param>
        /// <returns></returns>
        Task<IEnumerable<KeywordScrapeDetail>> SelectKeywordScrapeDetails(int scraperNo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scraperNo"></param>
        /// <param name="rowRevision"></param>
        /// <returns></returns>
        Task<IEnumerable<KeywordScrapeDetail>> SelectKeywordScrapeDetails(int scraperNo, byte[] rowRevision);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Proxy>> SelectProxies();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowVersion"></param>
        /// <returns></returns>
        Task<IEnumerable<Proxy>> SelectProxies(byte[] rowVersion);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ProxyMultiplier>> SelectProxyMultipliers();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowVersion"></param>
        /// <returns></returns>
        Task<IEnumerable<ProxyMultiplier>> SelectProxyMultipliers(byte[] rowVersion);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SearchEngine>> SelectSearchEngines();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowVersion"></param>
        /// <returns></returns>
        Task<IEnumerable<SearchEngine>> SelectSearchEngines(byte[] rowVersion);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SearchString>> SelectSearchStrings();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowVersion"></param>
        /// <returns></returns>
        Task<IEnumerable<SearchString>> SelectSearchStrings(byte[] rowVersion);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SpecialKeyword>> SelectSpecialKeywords();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowVersion"></param>
        /// <returns></returns>
        Task<IEnumerable<SpecialKeyword>> SelectSpecialKeywords(byte[] rowVersion);
    }
}
