using System;
using System.Collections.Generic;
using DBreeze;
using DBreeze.DataTypes;
using DBreeze.Objects;
using DBreeze.Utils;
using DBreeze.Transactions;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data.Concrete
{
    class CrawlLogRepo : ICrawlLogRepo
    {
        private const string _table = "CrawlLog";
        private readonly DBreezeEngine _engine;

        public CrawlLogRepo(DBreezeEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            DBreezeInitialization.SetupUtils();
        }

        public void Insert(CrawlLog log)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                bool newEntity = log.ID == 0;
                if (newEntity)
                {
                    log.ID = transaction.ObjectGetNewIdentity<int>(_table);
                }

                transaction.ObjectInsert(_table, new DBreezeObject<CrawlLog>
                {
                    NewEntity = newEntity,
                    Entity = log,
                    Indexes = new List<DBreezeIndex>
                    {
                        new DBreezeIndex(1, log.ID)
                        {
                            PrimaryIndex = true
                        },
                        // for quick lookup for crawl counts for search target and keyword pairs
                        new DBreezeIndex(2, log.SearchTargetID, log.KeywordID)
                        {
                            AddPrimaryToTheEnd = true // select forward from (st, k, min_signed(32)) -> (st, k, max_signed(32))
                        },
                        new DBreezeIndex(3, log.SearchTargetID)
                        {
                            AddPrimaryToTheEnd = true
                        }
                    }
                });

                transaction.Commit();
            }
        }

        public CrawlLog Select(int id)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<CrawlLog> obj = transaction
                    .Select<byte[], byte[]>(_table, 1.ToIndex(id))
                    .ObjectGet<CrawlLog>();

                if (obj != null)
                {
                    return obj.Entity;
                }

                return null;
            }
        }

        public IEnumerable<CrawlLog> SelectMany(int searchTargetId, int keywordId)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                var entities = new List<CrawlLog>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction.SelectForwardFromTo<byte[], byte[]>(
                        _table, 2.ToIndex(searchTargetId, keywordId, int.MinValue), true,
                        2.ToIndex(searchTargetId, keywordId, int.MaxValue), true);

                foreach (var row in rows)
                {
                    DBreezeObject<CrawlLog> obj = row.ObjectGet<CrawlLog>();
                    if (obj != null)
                    {
                        CrawlLog entity = obj.Entity;
                        entities.Add(entity);
                    }
                }

                return entities;
            }
        }

        public IEnumerable<CrawlLog> SelectMany(int searchTargetId)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                var entities = new List<CrawlLog>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction.SelectBackwardFromTo<byte[], byte[]>(
                    _table, 3.ToIndex(searchTargetId, int.MinValue), true,
                    3.ToIndex(searchTargetId, int.MaxValue), true);

                foreach (var row in rows)
                {
                    DBreezeObject<CrawlLog> obj = row.ObjectGet<CrawlLog>();
                    if (obj != null)
                    {
                        CrawlLog entity = obj.Entity;
                        entities.Add(entity);
                    }
                }

                return entities;
            }
        }

        public IEnumerable<CrawlLog> SelectAll()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                var entities = new List<CrawlLog>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction.SelectForward<byte[], byte[]>(_table);

                foreach (var row in rows)
                {
                    DBreezeObject<CrawlLog> obj = row.ObjectGet<CrawlLog>();
                    if (obj != null)
                    {
                        CrawlLog entity = obj.Entity;
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
    }
}
