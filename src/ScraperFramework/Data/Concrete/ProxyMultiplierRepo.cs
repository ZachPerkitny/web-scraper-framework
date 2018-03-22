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
    class ProxyMultiplierRepo : IProxyMultiplierRepo
    {
        private const string _table = "ProxyMultipliers";
        private readonly DBreezeEngine _engine;

        public ProxyMultiplierRepo(DBreezeEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            DBreezeInitialization.SetupUtils();
        }

        public void Insert(ProxyMultiplier proxyMultiplier)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                Insert(transaction, proxyMultiplier);

                transaction.Commit();
            }
        }

        public void InsertMany(IEnumerable<ProxyMultiplier> proxyMultipliers)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                foreach (ProxyMultiplier proxyMultiplier in proxyMultipliers)
                {
                    Insert(transaction, proxyMultiplier);
                }

                transaction.Commit();
            }
        }

        public ProxyMultiplier Select(int searchEngineId, int regionId, int proxyId)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<ProxyMultiplier> obj = transaction
                    .Select<byte[], byte[]>(_table, 1.ToIndex(searchEngineId, regionId, proxyId))
                    .ObjectGet<ProxyMultiplier>();

                if (obj != null)
                {
                    ProxyMultiplier entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public IEnumerable<ProxyMultiplier> SelectAll()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                List<ProxyMultiplier> entities = new List<ProxyMultiplier>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction
                    .SelectForwardFromTo<byte[], byte[]>(
                    _table, 1.ToIndex(int.MinValue, int.MinValue, int.MinValue), true,
                    1.ToIndex(int.MaxValue, int.MaxValue, int.MaxValue), true);

                foreach (Row<byte[], byte[]> row in rows)
                {
                    DBreezeObject<ProxyMultiplier> obj = row.ObjectGet<ProxyMultiplier>();

                    if (obj != null)
                    {
                        ProxyMultiplier entity = obj.Entity;
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

        public ProxyMultiplier Max()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<ProxyMultiplier> obj = transaction.Max<byte[], byte[]>(_table)
                    .ObjectGet<ProxyMultiplier>();

                if (obj != null)
                {
                    ProxyMultiplier entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public ProxyMultiplier Min()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<ProxyMultiplier> obj = transaction.Min<byte[], byte[]>(_table)
                    .ObjectGet<ProxyMultiplier>();

                if (obj != null)
                {
                    ProxyMultiplier entity = obj.Entity;
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
        /// an proxy multiplier entity
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="proxyMultiplier"></param>
        private void Insert(Transaction transaction, ProxyMultiplier proxyMultiplier)
        {
            transaction.ObjectInsert(_table, new DBreezeObject<ProxyMultiplier>
            {
                Entity = proxyMultiplier,
                Indexes = new List<DBreezeIndex>
                    {
                        new DBreezeIndex(1, proxyMultiplier.SearchEngineID, proxyMultiplier.RegionID, 
                        proxyMultiplier.ProxyID)
                        {
                            PrimaryIndex = true
                        },
                        new DBreezeIndex(2, proxyMultiplier.RowRevision)
                        {
                            AddPrimaryToTheEnd = false
                        }
                    }
            });
        }
    }
}
