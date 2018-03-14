using System;
using System.Collections.Generic;
using System.Text;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    public interface IEndpointSearchTargetRepo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointSearchTarget"></param>
        void Insert(EndpointSearchTarget endpointSearchTarget);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointSearchTargets"></param>
        void InsertMany(IEnumerable<EndpointSearchTarget> endpointSearchTargets);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        EndpointSearchTarget Select(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTargetId"></param>
        /// <param name="endpointId"></param>
        EndpointSearchTarget Select(int searchTargetId, int endpointId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTargetId"></param>
        /// <returns></returns>
        IEnumerable<EndpointSearchTarget> SelectMany(int searchTargetId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<EndpointSearchTarget> SelectAll();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ulong Count();
    }
}
