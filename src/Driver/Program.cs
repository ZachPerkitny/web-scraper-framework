﻿using System;
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
                config.DBreezeDataFolderName = @"C:\Users\zaperkitny\Documents\dbreeze";
                config.Scrapers = 5;
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

