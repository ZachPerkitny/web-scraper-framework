using System;
using System.Threading;

namespace ScraperFramework
{
    public interface IHttpServer : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixes"></param>
        /// <param name="cancelToken"></param>
        void Listen(string[] prefixes, CancellationToken cancelToken);
    }
}
