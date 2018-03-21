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
        /// <returns></returns>
        Task<IEnumerable<SearchEngine>> SelectSearchEngines();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowVersion"></param>
        /// <returns></returns>
        Task<IEnumerable<SearchEngine>> SelectSearchEngines(byte[] rowVersion);
    }
}
