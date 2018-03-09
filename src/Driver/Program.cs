using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;
using ScraperFramework;
using ScraperFramework.Configuration;

namespace Driver
{
    class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = SetupLogger();

            var scraperBuilder = new ScraperBuilder(config =>
            {

            });

            IController controller = scraperBuilder.Build();

            controller.Start();

            Console.ReadLine();
            controller.Stop();
            Console.ReadLine();
        }

        private static ILogger SetupLogger()
        {
            var logConfig = new LoggerConfiguration();

            logConfig
                .MinimumLevel.Debug()
                .WriteTo.Console();

            return logConfig.CreateLogger();
        }
    }
}
