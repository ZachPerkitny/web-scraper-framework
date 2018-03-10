using System;
using System.Net;
using System.Threading.Tasks;

namespace ScraperFramework
{
    interface IHttpRequestHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        Task<object> Execute(HttpListenerRequest listenerRequest);
    }
}
