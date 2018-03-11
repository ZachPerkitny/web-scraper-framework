using System;
using RestFul.Enum;
using RestFul.Loggers;

namespace RestFul
{
    public class RestFulSettings : IRestFulSettings
    {
        public int ConcurrentRequests { get; private set; }

        public string Host { get; private set; }

        public int Port { get; private set; }

        public bool UseHTTPs { get; private set; }

        public IRestfulLogger Logger { get; private set; }

        public RestFulSettings()
        {
            ConcurrentRequests = Environment.ProcessorCount * 1;
            Host = "localhost";
            Logger = new NullLogger();
            Port = 8000;
            UseHTTPs = false;
        }

        public IRestFulSettings WithConcurrentRequests(int count)
        {
            ConcurrentRequests = Environment.ProcessorCount * count;
            return this;
        }

        public IRestFulSettings WithHost(string host)
        {
            Host = host;
            return this;
        }

        public IRestFulSettings WithHttps()
        {
            UseHTTPs = true;
            return this;
        }

        public IRestFulSettings WithConsoleLogger(LogLevel logLevel)
        {
            Logger = new ConsoleLogger(logLevel);
            return this;
        }

        public IRestFulSettings WithLogger(IRestfulLogger logger)
        {
            Logger = logger;
            return this;
        }

        public IRestFulSettings WithPort(int port)
        {
            Port = port;
            return this;
        }
    }
}
