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
    class EndpointRepo : IEndpointRepo
    {
        private const string _table = "Endpoint";
        private readonly DBreezeEngine _engine;

        public EndpointRepo(DBreezeEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            DBreezeInitialization.SetupUtils();
        }

        public void Insert(Endpoint endpoint)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                Insert(transaction, endpoint);

                transaction.Commit();
            }
        }

        public void InsertMany(IEnumerable<Endpoint> endpoints)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                foreach (Endpoint endpoint in endpoints)
                {
                    Insert(transaction, endpoint);
                }

                transaction.Commit();
            }
        }

        public Endpoint Select(int endpointId)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<Endpoint> obj = transaction
                    .Select<byte[], byte[]>(_table, 1.ToIndex(endpointId))
                    .ObjectGet<Endpoint>();

                if (obj != null)
                {
                    return obj.Entity;
                }

                return null;
            }
        }

        public IEnumerable<Endpoint> SelectAll()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                var entities = new List<Endpoint>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction.SelectForward<byte[], byte[]>(_table);

                foreach (var row in rows)
                {
                    DBreezeObject<Endpoint> obj = row.ObjectGet<Endpoint>();
                    if (obj != null)
                    {
                        Endpoint entity = obj.Entity;
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
        /// an endpoint entity
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="endpoint"></param>
        private void Insert(Transaction transaction, Endpoint endpoint)
        {
            bool newEntity = endpoint.ID == 0;
            if (newEntity)
            {
                endpoint.ID = transaction.ObjectGetNewIdentity<int>(_table);
            }

            transaction.ObjectInsert(_table, new DBreezeObject<Endpoint>
            {
                NewEntity = newEntity,
                Entity = endpoint,
                Indexes = new List<DBreezeIndex>
                    {
                        new DBreezeIndex(1, endpoint.ID)
                        {
                            PrimaryIndex = true
                        }
                    }
            });
        }
    }
}
