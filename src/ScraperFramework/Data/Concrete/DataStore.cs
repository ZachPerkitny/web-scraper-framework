using System;
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

        public async Task<IEnumerable<Keyword>> SelectKeywords()
        {
            string sql = @"SELECT KeywordID AS ID 
                                ,Keyword AS Value
                                ,RowRevision
                           FROM [dbo].[keyword]";

            using (IDbConnection connection = _connectionFactory.GetDbConnection())
            {
                IEnumerable<Keyword> keywords =
                    await connection.QueryAsync<Keyword>(
                        sql: sql);

                return keywords;
            }
        }

        public async Task<IEnumerable<Keyword>> SelectKeywords(byte[] rowVersion)
        {
            string sql = @"SELECT KeywordID AS ID 
                                ,Keyword AS Value
                                ,RowRevision
                           FROM [dbo].[keyword]
                           WHERE @RowVersion < RowRevision";

            using (IDbConnection connection = _connectionFactory.GetDbConnection())
            {
                IEnumerable<Keyword> keywords =
                    await connection.QueryAsync<Keyword>(
                        sql: sql,
                        param: new
                        {
                            RowVersion = rowVersion
                        });

                return keywords;
            }
        }

        public async Task<IEnumerable<SearchEngine>> SelectSearchEngines()
        {
            string sql = @"SELECT SearchEngineID AS ID
                                ,IsMobile
                                ,RowRevision
                           FROM [dbo].[SearchEngine]";

            using (IDbConnection connection = _connectionFactory.GetDbConnection())
            {
                IEnumerable<SearchEngine> searchEngines =
                    await connection.QueryAsync<SearchEngine>(
                        sql: sql);

                return searchEngines;
            }
        }

        public async Task<IEnumerable<SearchEngine>> SelectSearchEngines(byte[] rowVersion)
        {
            string sql = @"SELECT SearchEngineID AS ID
                                ,IsMobile
                                ,RowRevision
                           FROM [dbo].[SearchEngine]
                           WHERE @RowVersion < RowRevision";

            using (IDbConnection connection = _connectionFactory.GetDbConnection())
            {
                IEnumerable<SearchEngine> searchEngines = 
                    await connection.QueryAsync<SearchEngine>(
                        sql: sql,
                        param: new
                        {
                            RowVersion = rowVersion
                        });

                return searchEngines;
            }
        }

        public async Task<IEnumerable<SearchString>> SelectSearchStrings()
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

            using (IDbConnection connection = _connectionFactory.GetDbConnection())
            {
                IEnumerable<SearchString> searchStrings =
                    await connection.QueryAsync<SearchString>(
                        sql: sql);

                return searchStrings;
            }
        }

        public async Task<IEnumerable<SearchString>> SelectSearchStrings(byte[] rowVersion)
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

            using (IDbConnection connection = _connectionFactory.GetDbConnection())
            {
                IEnumerable<SearchString> searchStrings =
                    await connection.QueryAsync<SearchString>(
                        sql: sql,
                        param: new
                        {
                            RowVersion = rowVersion
                        });

                return searchStrings;
            }
        }
    }
}
