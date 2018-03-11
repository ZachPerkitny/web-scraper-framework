using System;
using RestFul.Enum;

namespace RestFul.Loggers
{
    class ConsoleLogger : IRestFulLogger
    {
        public LogLevel MinimumLogLevel { get; set; }

        public ConsoleLogger()
        {
            MinimumLogLevel = LogLevel.Debug;
        }

        public ConsoleLogger(LogLevel logLevel)
        {
            MinimumLogLevel = logLevel;
        }

        public void Debug(string format, params object[] args)
        {
            if (MinimumLogLevel > LogLevel.Debug)
            {
                return;
            }

            Log(LogLevel.Debug, string.Format(format, args));
        }

        public void Information(string format, params object[] args)
        {
            if (MinimumLogLevel > LogLevel.Info)
            {
                return;
            }

            Log(LogLevel.Info, string.Format(format, args));
        }

        public void Warning(string format, params object[] args)
        {
            if (MinimumLogLevel > LogLevel.Warning)
            {
                return;
            }

            Log(LogLevel.Warning, string.Format(format, args));
        }

        public void Error(string format, params object[] args)
        {
            if (MinimumLogLevel > LogLevel.Error)
            {
                return;
            }

            Log(LogLevel.Error, string.Format(format, args));
        }


        private void Log(LogLevel logLevel, string message)
        {
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()} {logLevel.ToString()}] {message}");
        }
    }
}
