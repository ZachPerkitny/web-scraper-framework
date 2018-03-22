using System;
using System.Configuration;
using Serilog;
using ScraperFramework;
using ScraperFramework.Configuration;
using ScraperFramework.Utils;

namespace Driver
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = SetupLogger();

            //var x = new DoubleOrderedMap<int, int>();

            //for (int i = 0; i < 8; i++)
            //{
            //    x.Add(i, i);
            //}

            //foreach (var y in x)
            //{
            //    Console.WriteLine("{0} {1}", y.Key, y.Value);
            //}

            //Log.Information(x.Count.ToString());

            var scraperBuilder = new ScraperBuilder(config =>
            {
                config.DBreezeDataFolderName = ConfigurationManager.AppSettings["DBreezeDataFolderName"];
                config.Scrapers = int.Parse(ConfigurationManager.AppSettings["Scrapers"]);
                config.SyncInterval = int.Parse(ConfigurationManager.AppSettings["SyncInterval"]);
                config.ScraperConnectionString = ConfigurationManager.AppSettings["ScraperConnectionString"];
            });

            ICoordinator controller = scraperBuilder.Build();
            controller.Start();

            Console.ReadLine();
            controller.Dispose();
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

