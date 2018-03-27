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
    class KeywordScrapeDetailRepo : IKeywordScrapeDetailRepo
    {
        private const string _table = "KeywordScrapeDetail";
        private readonly DBreezeEngine _engine;

        public KeywordScrapeDetailRepo(DBreezeEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }

        public void Insert(KeywordScrapeDetail keywordScrapeDetail)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                Insert(transaction, keywordScrapeDetail);

                transaction.Commit();
            }
        }

        public void InsertMany(IEnumerable<KeywordScrapeDetail> keywordScrapeDetails)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                foreach (KeywordScrapeDetail keywordScrapeDetail in keywordScrapeDetails)
                {
                    Insert(transaction, keywordScrapeDetail);
                }

                transaction.Commit();
            }
        }

        public KeywordScrapeDetail Select(short searchEngineId, short regionId, short cityId, int keywordId)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<KeywordScrapeDetail> obj = transaction
                    .Select<byte[], byte[]>(_table, 1.ToIndex(
                        searchEngineId, regionId, cityId, keywordId))
                    .ObjectGet<KeywordScrapeDetail>();

                if (obj != null)
                {
                    KeywordScrapeDetail entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public IEnumerable<KeywordScrapeDetail> SelectNext(int count)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                transaction.SynchronizeTables(_table);

                IEnumerable<Row<byte[], byte[]>> rows = transaction
                    .SelectForwardFromTo<byte[], byte[]>(
                    _table, 3.ToIndex(long.MinValue, short.MinValue, short.MinValue, short.MinValue, short.MinValue, int.MinValue), true,
                    3.ToIndex(long.MaxValue, short.MaxValue, short.MaxValue, short.MaxValue, short.MaxValue, int.MaxValue), true)
                    .Take(count);

                List<KeywordScrapeDetail> entities = new List<KeywordScrapeDetail>();
                foreach (Row<byte[], byte[]> row in rows)
                {
                    DBreezeObject<KeywordScrapeDetail> obj = row.ObjectGet<KeywordScrapeDetail>();

                    if (obj != null)
                    {
                        entities.Add(obj.Entity);
                    }
                }

                if (entities.Count() > 1006)
                {
                    Environment.Exit(1);
                }

                return entities;
            }
        }

        public IEnumerable<KeywordScrapeDetail> SelectMany(short searchEngineId, short regionId, short cityId)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                List<KeywordScrapeDetail> entities = new List<KeywordScrapeDetail>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction
                    .SelectForwardFromTo<byte[], byte[]>(_table,
                    1.ToIndex(searchEngineId, regionId, cityId, int.MinValue), true,
                    1.ToIndex(searchEngineId, regionId, cityId, int.MaxValue), true);

                foreach (Row<byte[], byte[]> row in rows)
                {
                    DBreezeObject<KeywordScrapeDetail> obj = row.ObjectGet<KeywordScrapeDetail>();

                    if (obj != null)
                    {
                        KeywordScrapeDetail entity = obj.Entity;
                        entities.Add(entity);
                    }
                }

                return entities;
            }
        }

        public void UpdateLastCrawl(short searchEngineId, short regionId, short cityId, int keywordId, DateTime lastCrawl)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                transaction.SynchronizeTables(_table);

                DBreezeObject<KeywordScrapeDetail> obj = transaction
                    .Select<byte[], byte[]>(_table, 1.ToIndex(
                        searchEngineId, regionId, cityId, keywordId))
                    .ObjectGet<KeywordScrapeDetail>();

                if (obj == null)
                {
                    return;
                }

                obj.Entity.LastCrawl = lastCrawl;
                Insert(transaction, obj.Entity, false);

                transaction.Commit();
            }
        }

        public ulong Count()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                return transaction.Count(_table);
            }
        }

        public KeywordScrapeDetail Max()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<KeywordScrapeDetail> obj = transaction.Max<byte[], byte[]>(_table)
                    .ObjectGet<KeywordScrapeDetail>();

                if (obj != null)
                {
                    KeywordScrapeDetail entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public KeywordScrapeDetail Min()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<KeywordScrapeDetail> obj = transaction.Min<byte[], byte[]>(_table)
                    .ObjectGet<KeywordScrapeDetail>();

                if (obj != null)
                {
                    KeywordScrapeDetail entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public byte[] GetLatestRevision()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                IEnumerable<Row<byte[], byte[]>> rows = transaction
                    .SelectBackwardStartFrom<byte[], byte[]>(
                    _table, 2.ToIndex(long.MaxValue), true);

                if (rows.Any())
                {
                    return rows.First()
                        .Key.Skip(1).ToArray();
                }

                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="keywordScrapeDetail"></param>
        /// <param name="newEntity"></param>
        private void Insert(Transaction transaction, KeywordScrapeDetail keywordScrapeDetail, bool newEntity = true)
        {
            transaction.ObjectInsert(_table, new DBreezeObject<KeywordScrapeDetail>
            {
                NewEntity = newEntity,
                Entity = keywordScrapeDetail,
                Indexes = new List<DBreezeIndex>
                {
                    new DBreezeIndex(1, keywordScrapeDetail.SearchEngineID, keywordScrapeDetail.RegionID,
                        keywordScrapeDetail.CityID, keywordScrapeDetail.KeywordID)
                    {
                        PrimaryIndex = true
                    },
                    new DBreezeIndex(2, keywordScrapeDetail.RowRevision)
                    {
                        AddPrimaryToTheEnd = false
                    },
                    new DBreezeIndex(3, keywordScrapeDetail.LastCrawl, keywordScrapeDetail.Priority)
                    {
                        AddPrimaryToTheEnd = true
                    }
                    // TODO(zvp): Add Priority Index
                }
            });
        }
    }
}
