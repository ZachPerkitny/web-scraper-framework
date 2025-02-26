﻿namespace ScraperFramework.Configuration
{
    public class ScraperConfig
    {
        public int ScraperNo { get; set; }

        public int Scrapers { get; set; }

        public string DBreezeDataFolderName { get; set; }

        public string ScraperConnectionString { get; set; }

        public double SyncInterval { get; set; }
    }
}
