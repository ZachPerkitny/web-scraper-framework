using RestFul.Enum;
using RestFul.Loggers;

namespace RestFul
{
    public interface IRestFulSettings
    {
        /// <summary>
        /// 
        /// </summary>
        int ConcurrentRequests { get; }

        /// <summary>
        /// 
        /// </summary>
        string Host { get; }

        /// <summary>
        /// 
        /// </summary>
        IRestfulLogger Logger { get; }

        /// <summary>
        /// 
        /// </summary>
        int Port { get; }

        /// <summary>
        /// 
        /// </summary>
        bool UseHTTPs { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        IRestFulSettings WithConcurrentRequests(int count);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        IRestFulSettings WithHost(string host);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IRestFulSettings WithConsoleLogger(LogLevel logLevel = LogLevel.Debug);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        IRestFulSettings WithLogger(IRestfulLogger logger);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        IRestFulSettings WithPort(int port);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IRestFulSettings WithHttps();
    }
}
