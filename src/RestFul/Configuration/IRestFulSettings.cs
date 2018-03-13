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
        int Port { get; }

        /// <summary>
        /// 
        /// </summary>
        bool UseHTTPs { get; }

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
        /// <param name="port"></param>
        /// <returns></returns>
        IRestFulSettings WithPort(int port);
    }
}
