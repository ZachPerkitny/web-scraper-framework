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

        public ProxyMultiplier Select(int proxyMultiplierId)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<ProxyMultiplier> obj = transaction
                    .Select<byte[], byte[]>(_table, 1.ToIndex(proxyMultiplierId))
                    .ObjectGet<ProxyMultiplier>();

                if (obj != null)
                {
                    ProxyMultiplier entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public ProxyMultiplier Select(int searchEngineId, int regionId, int proxyId)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<ProxyMultiplier> obj = transaction
                    .Select<byte[], byte[]>(_table, 2.ToIndex(searchEngineId, regionId, proxyId))
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
                    _table, 1.ToIndex(int.MinValue), true,
                    1.ToIndex(int.MaxValue), true);

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

        /// <summary>
        /// Does an object insert and creates the necessary indexes for
        /// an proxy multiplier entity
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="proxyMultiplier"></param>
        private void Insert(Transaction transaction, ProxyMultiplier proxyMultiplier)
        {
            bool newEntity = proxyMultiplier.ID == 0;
            if (newEntity)
            {
                proxyMultiplier.ID = transaction.ObjectGetNewIdentity<int>(_table);
            }

            transaction.ObjectInsert(_table, new DBreezeObject<ProxyMultiplier>
            {
                NewEntity = newEntity,
                Entity = proxyMultiplier,
                Indexes = new List<DBreezeIndex>
                    {
                        new DBreezeIndex(1, proxyMultiplier.ID)
                        {
                            PrimaryIndex = true
                        },
                        new DBreezeIndex(2, proxyMultiplier.SearchEngineID, proxyMultiplier.RegionID, 
                        proxyMultiplier.ProxyID)
                        {
                            AddPrimaryToTheEnd = false
                        }
                    }
            });
        }
    }
}
