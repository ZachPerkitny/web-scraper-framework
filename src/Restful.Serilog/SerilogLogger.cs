using RestFul.Loggers;
using Serilog;

namespace Restful.Serilog
{
    public class SerilogLogger : IRestFulLogger
    {
        private readonly ILogger _logger;

        public SerilogLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Debug(string format, params object[] args)
        {
            _logger.Debug(format, args);
        }

        public void Information(string format, params object[] args)
        {
            _logger.Information(format, args);
        }

        public void Warning(string format, params object[] args)
        {
            _logger.Warning(format, args);
        }

        public void Error(string format, params object[] args)
        {
            _logger.Error(format, args);
        }

    }
}
