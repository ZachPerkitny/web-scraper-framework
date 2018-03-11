using System;
using RestFul.Enum;
using RestFul.Loggers;
using RestFul.Serializer;

namespace RestFul.Configuration
{
    public class RestFulSettings : IRestFulSettings
    {
        public string Host { get; private set; }

        public IRestFulLogger Logger { get; private set; }

        public int Port { get; private set; }

        public ISerializer Serializer { get; private set; }

        public bool UseHTTPs { get; private set; }  

        public RestFulSettings()
        {
            Host = "localhost";
            Logger = new NullLogger();
            Port = 8000;
            Serializer = new JsonSerializer();
            UseHTTPs = false;
        }

        public IRestFulSettings WithConsoleLogger(LogLevel logLevel)
        {
            Logger = new ConsoleLogger(logLevel);
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

        public IRestFulSettings WithLogger(IRestFulLogger logger)
        {
            Logger = logger;
            return this;
        }

        public IRestFulSettings WithPort(int port)
        {
            Port = port;
            return this;
        }

        public IRestFulSettings WithSerializer(ISerializer serializer)
        {
            Serializer = serializer;
            return this;
        }
    }
}
