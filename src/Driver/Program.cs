﻿using System;
using System.Configuration;
using Serilog;
using ScraperFramework;
using ScraperFramework.Configuration;

namespace Driver
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = SetupLogger();

            var scraperBuilder = new ScraperBuilder(config =>
            {
                config.DBreezeDataFolderName = ConfigurationManager.AppSettings["DBreezeDataFolderName"];
                config.Scrapers = int.Parse(ConfigurationManager.AppSettings["Scrapers"]);
                config.SyncInterval = 60000;
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

