using System;
using System.Threading;

namespace ScraperFramework
{
    public interface ICommandListener
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixes"></param>
        /// <param name="cancelToken"></param>
        void Listen(string[] prefixes, CancellationToken cancelToken);
    }
}
