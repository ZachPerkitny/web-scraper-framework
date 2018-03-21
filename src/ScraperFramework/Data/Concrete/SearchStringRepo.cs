using System;
using System.Collections.Generic;
using System.Linq;
using DBreeze;
using DBreeze.DataTypes;
using DBreeze.Objects;
using DBreeze.Utils;
using DBreeze.Transactions;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data.Concrete
{
    class SearchStringRepo : ISearchStringRepo
    {
        private const string _table = "SearchStrings";
        private readonly DBreezeEngine _engine;

        public SearchStringRepo(DBreezeEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            DBreezeInitialization.SetupUtils();
        }

        public void Insert(SearchString searchString)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                Insert(transaction, searchString);

                transaction.Commit();
            }
        }

        public void InsertMany(IEnumerable<SearchString> searchStrings)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                foreach (SearchString searchString in searchStrings)
                {
                    Insert(transaction, searchString);
                }

                transaction.Commit();
            }
        }

        public SearchString Select(int searchStringId)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<SearchString> obj = transaction
                    .Select<byte[], byte[]>(_table, 1.ToIndex(searchStringId))
                    .ObjectGet<SearchString>();

                if (obj != null)
                {
                    SearchString entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public SearchString Select(int searchEngineId, int regionId)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<SearchString> obj = transaction
                    .Select<byte[], byte[]>(_table, 2.ToIndex(searchEngineId, regionId))
                    .ObjectGet<SearchString>();

                if (obj != null)
                {
                    SearchString entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public IEnumerable<SearchString> SelectAll()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                // Documentation https://goo.gl/MbZAsB
                List<SearchString> entities = new List<SearchString>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction
                    .SelectForwardFromTo<byte[], byte[]>(_table,
                    1.ToIndex(int.MinValue), true,
                    1.ToIndex(int.MaxValue), true);

                foreach (Row<byte[], byte[]> row in rows)
                {
                    DBreezeObject<SearchString> obj = row.ObjectGet<SearchString>();

                    if (obj != null)
                    {
                        SearchString entity = obj.Entity;
                        entities.Add(entity);
                    }
                }

                return entities;
            }
        }

        public ulong Count()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                return transaction.Count(_table);
            }
        }

        public SearchString Max()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<SearchString> obj = transaction.Max<byte[], byte[]>(_table)
                    .ObjectGet<SearchString>();

                if (obj != null)
                {
                    SearchString entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public SearchString Min()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<SearchString> obj = transaction.Min<byte[], byte[]>(_table)
                    .ObjectGet<SearchString>();

                if (obj != null)
                {
                    SearchString entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public byte[] GetLatestRevision()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                // this is done to take advantage of dbreeze's
                // lazy loading, value is never actually loaded
                // from disk.
                IEnumerable<Row<byte[], byte[]>> rows = transaction
                    .SelectBackwardStartFrom<byte[], byte[]>(
                    _table, 3.ToIndex(long.MaxValue), true);

                if (rows.Any())
                {
                    // skip first byte (dbreezeindex index)
                    return rows.First()
                        .Key.Skip(1).ToArray();
                }

                return null;
            }
        }

        /// <summary>
        /// Does an object insert and creates the necessary indexes for
        /// an search string entity
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="searchString"></param>
        private void Insert(Transaction transaction, SearchString searchString)
        {
            bool newEntity = searchString.ID == 0;
            if (newEntity)
            {
                searchString.ID = transaction.ObjectGetNewIdentity<int>(_table);
            }

            transaction.ObjectInsert(_table, new DBreezeObject<SearchString>
            {
                NewEntity = newEntity,
                Entity = searchString,
                Indexes = new List<DBreezeIndex>
                    {
                        new DBreezeIndex(1, searchString.ID)
                        {
                            PrimaryIndex = true
                        },
                        new DBreezeIndex(2, searchString.SearchEngineID, searchString.RegionID)
                        {
                            AddPrimaryToTheEnd = false
                        },
                        new DBreezeIndex(3, searchString.RowRevision)
                        {
                            AddPrimaryToTheEnd = false
                        }
                    }
            });
        }
    }
}
