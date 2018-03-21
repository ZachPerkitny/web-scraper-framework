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

        public async Task<IEnumerable<SearchEngine>> SelectSearchEngines()
        {
            string sql = @"SELECT SearchEngineID as ID
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
            string sql = @"SELECT SearchEngineID as ID
                                ,IsMobile
                                ,RowRevision
                           FROM [dbo].[SearchEngine]
                           WHERE @RowVersion > 
                                (SELECT MAX(RowRevision) 
                                FROM [dbo].[SearchEngine])";

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
    }
}
