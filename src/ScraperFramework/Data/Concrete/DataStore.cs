﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data.Concrete
{
    class DataStore : IDataStore
    {
        private readonly ConnectionFactory _connectionFactory;

        public DataStore(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public Task<IEnumerable<Keyword>> SelectKeywords()
        {
            string sql = @"SELECT KeywordID AS ID 
                                ,Keyword AS Value
                                ,RowRevision
                           FROM [dbo].[keyword]";

            return SelectMany<Keyword>(sql);
        }

        public Task<IEnumerable<Keyword>> SelectKeywords(byte[] rowVersion)
        {
            string sql = @"SELECT KeywordID AS ID 
                                ,Keyword AS Value
                                ,RowRevision
                           FROM [dbo].[keyword]
                           WHERE @RowVersion < RowRevision";

            return SelectMany<Keyword>(sql, new
            {
                RowVersion = rowVersion
            });
        }

        public Task<IEnumerable<Proxy>> SelectProxies()
        {
            string sql = @"SELECT ProxyID AS ID
                                ,IP
                                ,Port
                                ,RegionId
                                ,Status
                                ,ProxyBlockID
                                ,RowRevision
                           FROM [dbo].[Proxy_v2]";

            return SelectMany<Proxy>(sql);
        }

        public Task<IEnumerable<KeywordScrapeDetail>> SelectKeywordScrapeDetails(int scraperNo)
        {
            string sql = @"SELECT SearchEngineID
                                ,RegionID
                                ,CityID
                                ,Priority
                                ,IsActive
                                ,KeywordID
                                ,RowRevision
                           FROM [dbo].[KeywordScrapeDetail]
                           WHERE ScraperNo = @ScraperNo";

            return SelectMany<KeywordScrapeDetail>(sql, new
            {
                ScraperNo = scraperNo
            });
        }

        public Task<IEnumerable<KeywordScrapeDetail>> SelectKeywordScrapeDetails(int scraperNo, byte[] rowRevision)
        {
            string sql = @"SELECT SearchEngineID
                                ,RegionID
                                ,CityID
                                ,Priority
                                ,IsActive
                                ,KeywordID
                                ,RowRevision
                           FROM [dbo].[KeywordScrapeDetail]
                           WHERE ScraperNo = @ScraperNo
                           AND @RowVersion < RowRevision";

            return SelectMany<KeywordScrapeDetail>(sql, new
            {
                RowVersion = rowRevision,
                ScraperNo = scraperNo
            });
        }

        public Task<IEnumerable<Proxy>> SelectProxies(byte[] rowVersion)
        {
            string sql = @"SELECT ProxyID AS ID
                                ,IP
                                ,Port
                                ,RegionId
                                ,Status
                                ,ProxyBlockID
                                ,RowRevision
                           FROM [dbo].[Proxy_v2]
                           WHERE @RowVersion < RowRevision";

            return SelectMany<Proxy>(sql, new
            {
                RowVersion = rowVersion
            });
        }

        public Task<IEnumerable<ProxyMultiplier>> SelectProxyMultipliers()
        {
            string sql = @"SELECT ProxyID
                                ,SEID AS SearchEngineID
                                ,RID AS RegionID
                                ,Multiplier
                                ,RowRevision
                           FROM [dbo].[Proxy_Multipliers]";

            return SelectMany<ProxyMultiplier>(sql);
        }

        public Task<IEnumerable<ProxyMultiplier>> SelectProxyMultipliers(byte[] rowVersion)
        {
            string sql = @"SELECT ProxyID
                                ,SEID AS SearchEngineID
                                ,RID AS RegionID
                                ,Multiplier
                                ,RowRevision
                           FROM [dbo].[Proxy_Multipliers]
                           WHERE @RowVersion < RowRevision";

            return SelectMany<ProxyMultiplier>(sql, new
            {
                RowVersion = rowVersion
            });
        }

        public Task<IEnumerable<SearchEngine>> SelectSearchEngines()
        {
            string sql = @"SELECT SearchEngineID AS ID
                                ,IsMobile
                                ,RowRevision
                           FROM [dbo].[SearchEngine]";

            return SelectMany<SearchEngine>(sql);
        }

        public Task<IEnumerable<SearchEngine>> SelectSearchEngines(byte[] rowVersion)
        {
            string sql = @"SELECT SearchEngineID AS ID
                                ,IsMobile
                                ,RowRevision
                           FROM [dbo].[SearchEngine]
                           WHERE @RowVersion < RowRevision";

            return SelectMany<SearchEngine>(sql, new
            {
                RowVersion = rowVersion
            });
        }

        public Task<IEnumerable<SearchString>> SelectSearchStrings()
        {
            string sql = @"SELECT SearchStringID AS ID
                                ,SearchEngineId
                                ,RegionID
                                ,SearchEngine
                                ,SearchEngineURL
                                ,naturalResultsParamString
                                ,DelayMultiplier
                                ,RowRevision
                           FROM [dbo].[SearchStrings]";

            return SelectMany<SearchString>(sql);
        }

        public Task<IEnumerable<SearchString>> SelectSearchStrings(byte[] rowVersion)
        {
            string sql = @"SELECT SearchStringID AS ID
                                ,SearchEngineId
                                ,RegionID
                                ,SearchEngine
                                ,SearchEngineURL
                                ,naturalResultsParamString
                                ,DelayMultiplier
                                ,RowRevision
                           FROM [dbo].[SearchStrings]
                           WHERE @RowVersion < RowRevision";

            return SelectMany<SearchString>(sql, new
            {
                RowVersion = rowVersion
            });
        }

        public Task<IEnumerable<SpecialKeyword>> SelectSpecialKeywords()
        {
            string sql = @"SELECT KeywordId
                                ,SearchEngineId
                                ,RegionId
                                ,RowRevision
                           FROM [dbo].[SpecialKeywords]";

            return SelectMany<SpecialKeyword>(sql);
        }

        public Task<IEnumerable<SpecialKeyword>> SelectSpecialKeywords(byte[] rowVersion)
        {
            string sql = @"SELECT KeywordId
                                ,SearchEngineId
                                ,RegionId
                                ,RowRevision
                           FROM [dbo].[SpecialKeywords]
                           WHERE @RowVersion < RowRevision";

            return SelectMany<SpecialKeyword>(sql, new
            {
                RowVersion = rowVersion
            });
        }

        private async Task<IEnumerable<T>> SelectMany<T>(string sql, object @params = null) 
            where T : class
        {
            using (IDbConnection connection = _connectionFactory.GetDbConnection())
            {
                IEnumerable<T> entities =
                    await connection.QueryAsync<T>(
                        sql: sql,
                        param: @params);

                return entities;
            }
        }
    }
}
