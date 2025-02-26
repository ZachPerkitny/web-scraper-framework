﻿using System;
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
    class SearchEngineRepo : ISearchEngineRepo
    {
        private const string _table = "SearchEngines";
        private readonly DBreezeEngine _engine;

        public SearchEngineRepo(DBreezeEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }

        public void Insert(SearchEngine searchEngine)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                Insert(transaction, searchEngine);

                transaction.Commit();
            }
        }

        public void InsertMany(IEnumerable<SearchEngine> searchEngines)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                foreach (SearchEngine searchEngine in searchEngines)
                {
                    Insert(transaction, searchEngine);
                }

                transaction.Commit();
            }
        }

        public SearchEngine Select(short searchEngineId)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<SearchEngine> obj = transaction
                    .Select<byte[], byte[]>(_table, 1.ToIndex(searchEngineId))
                    .ObjectGet<SearchEngine>();

                if (obj != null)
                {
                    SearchEngine entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public IEnumerable<SearchEngine> SelectMany(int searchEngineGroup)
        {
            //using (Transaction transaction = _engine.GetTransaction())
            //{
            //    List<SearchEngine> entities = new List<SearchEngine>();
            //    IEnumerable<Row<byte[], byte[]>> rows = transaction.SelectForwardFromTo<byte[], byte[]>(
            //        _table, 2.ToIndex(searchEngineGroup, int.MinValue), true,
            //        2.ToIndex(searchEngineGroup, int.MaxValue), true);

            //    foreach (Row<byte[], byte[]> row in rows)
            //    {
            //        DBreezeObject<SearchEngine> obj = row.ObjectGet<SearchEngine>();
            //        if (obj != null)
            //        {
            //            SearchEngine entity = obj.Entity;
            //            entities.Add(entity);
            //        }
            //    }

            //    return entities;
            //}
            throw new NotImplementedException();
        }

        public IEnumerable<SearchEngine> SelectAll()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                List<SearchEngine> entities = new List<SearchEngine>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction
                    .SelectForwardFromTo<byte[], byte[]>(_table, 
                    1.ToIndex(short.MinValue), true,
                    1.ToIndex(short.MaxValue), true);

                foreach (Row<byte[], byte[]> row in rows)
                {
                    DBreezeObject<SearchEngine> obj = row.ObjectGet<SearchEngine>();

                    if (obj != null)
                    {
                        SearchEngine entity = obj.Entity;
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

        public SearchEngine Max()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<SearchEngine> obj = transaction.Max<byte[], byte[]>(_table)
                    .ObjectGet<SearchEngine>();

                if (obj != null)
                {
                    SearchEngine entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public SearchEngine Min()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<SearchEngine> obj = transaction.Min<byte[], byte[]>(_table)
                    .ObjectGet<SearchEngine>();

                if (obj != null)
                {
                    SearchEngine entity = obj.Entity;
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
                    _table, 2.ToIndex(long.MaxValue), true);

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
        /// an search engine entity
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="searchEngine"></param>
        private void Insert(Transaction transaction, SearchEngine searchEngine)
        {
            bool newEntity = searchEngine.ID == 0;
            if (newEntity)
            {
                searchEngine.ID = transaction.ObjectGetNewIdentity<short>(_table);
            }

            DBreezeObjectInsertResult<SearchEngine> res = 
            transaction.ObjectInsert(_table, new DBreezeObject<SearchEngine>
            {
                NewEntity = newEntity,
                Entity = searchEngine,
                Indexes = new List<DBreezeIndex>
                    {
                        new DBreezeIndex(1, searchEngine.ID)
                        {
                            PrimaryIndex = true
                        },
                        new DBreezeIndex(2, searchEngine.RowRevision)
                        {
                            AddPrimaryToTheEnd = false
                        }
                        //new DBreezeIndex(3, searchEngine.SearchEngineGroupID)
                        //{
                        //    AddPrimaryToTheEnd = true
                        //}
                    }
            });
        }
    }
}
