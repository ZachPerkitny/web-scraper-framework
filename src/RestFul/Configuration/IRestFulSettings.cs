using RestFul.Enum;
using RestFul.Loggers;
using RestFul.Serializer;

namespace RestFul.Configuration
{
    public interface IRestFulSettings
    {
        /// <summary>
        /// 
        /// </summary>
        string Host { get; }

        /// <summary>
        /// 
        /// </summary>
        IRestFulLogger Logger { get; }

        /// <summary>
        /// 
        /// </summary>
        int Port { get; }

        /// <summary>
        /// 
        /// </summary>
        ISerializer Serializer { get; }

        /// <summary>
        /// 
        /// </summary>
        bool UseHTTPs { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IRestFulSettings WithConsoleLogger(LogLevel logLevel = LogLevel.Debug);

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
        IRestFulSettings WithHttps();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        IRestFulSettings WithLogger(IRestFulLogger logger);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        IRestFulSettings WithPort(int port);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializer"></param>
        /// <returns></returns>
        IRestFulSettings WithSerializer(ISerializer serializer);
    }
}
