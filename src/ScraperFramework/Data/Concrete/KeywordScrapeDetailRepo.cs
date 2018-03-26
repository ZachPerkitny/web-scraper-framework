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

        public KeywordScrapeDetail Select(int searchEngineId, int regionId, int cityId, int keywordId)
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

        public IEnumerable<KeywordScrapeDetail> SelectMany(int searchEngineId, int regionId, int cityId, int count)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                List<KeywordScrapeDetail> entities = new List<KeywordScrapeDetail>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction
                    .SelectForwardFromTo<byte[], byte[]>(_table,
                    1.ToIndex(searchEngineId, regionId, cityId, int.MinValue), true,
                    1.ToIndex(searchEngineId, regionId, cityId, int.MaxValue), true)
                    .Take(count);

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

        public void UpdateLastCrawl(int searchEngineId, int regionId, int cityId, int keywordId, DateTime lastCrawl)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<KeywordScrapeDetail> obj = transaction
                    .Select<byte[], byte[]>(_table, 1.ToIndex(
                        searchEngineId, regionId, cityId, keywordId))
                    .ObjectGet<KeywordScrapeDetail>();

                if (obj == null)
                {
                    return;
                }

                obj.Entity.LastCrawl = lastCrawl;
                Insert(transaction, obj.Entity);

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
        private void Insert(Transaction transaction, KeywordScrapeDetail keywordScrapeDetail)
        {
            transaction.ObjectInsert(_table, new DBreezeObject<KeywordScrapeDetail>
            {
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
                    }
                    // TODO(zvp): Add Priority Index
                }
            });
        }
    }
}
