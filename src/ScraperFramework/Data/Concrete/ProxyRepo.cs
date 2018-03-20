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
    class ProxyRepo : IProxyRepo
    {
        private const string _table = "Proxy_V2";
        private readonly DBreezeEngine _engine;

        public ProxyRepo(DBreezeEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            DBreezeInitialization.SetupUtils();
        }

        public void Insert(Proxy proxy)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                Insert(transaction, proxy);

                transaction.Commit();
            }
        }

        public void InsertMany(IEnumerable<Proxy> proxies)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                foreach (Proxy proxy in proxies)
                {
                    Insert(transaction, proxy);
                }

                transaction.Commit();
            }
        }

        public Proxy Select(int proxyId)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<Proxy> obj = transaction
                    .Select<byte[], byte[]>(_table, 1.ToIndex(proxyId))
                    .ObjectGet<Proxy>();

                if (obj != null)
                {
                    return obj.Entity;
                }

                return null;
            }
        }

        public IEnumerable<Proxy> SelectAll()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                var entities = new List<Proxy>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction.SelectForward<byte[], byte[]>(_table);

                foreach (var row in rows)
                {
                    DBreezeObject<Proxy> obj = row.ObjectGet<Proxy>();
                    if (obj != null)
                    {
                        Proxy entity = obj.Entity;
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
        /// an proxy entity
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="proxy"></param>
        private void Insert(Transaction transaction, Proxy proxy)
        {
            bool newEntity = proxy.ID == 0;
            if (newEntity)
            {
                proxy.ID = transaction.ObjectGetNewIdentity<int>(_table);
            }

            transaction.ObjectInsert(_table, new DBreezeObject<Proxy>
            {
                NewEntity = newEntity,
                Entity = proxy,
                Indexes = new List<DBreezeIndex>
                    {
                        new DBreezeIndex(1, proxy.ID)
                        {
                            PrimaryIndex = true
                        }
                    }
            });
        }
    }
}
