using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    public interface IEndpointRepo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        void Insert(Endpoint endpoint);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoints"></param>
        void InsertMany(IEnumerable<Endpoint> endpoints);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointId"></param>
        /// <returns></returns>
        Endpoint Select(int endpointId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<Endpoint> SelectAll();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ulong Count();
    }
}
