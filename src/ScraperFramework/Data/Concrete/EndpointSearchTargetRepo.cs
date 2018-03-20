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
    public class EndpointSearchTargetRepo : IEndpointSearchTargetRepo
    {
        private const string _table = "EndpointSearchTarget";
        private readonly DBreezeEngine _engine;

        public EndpointSearchTargetRepo(DBreezeEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            DBreezeInitialization.SetupUtils();
        }

        public void Insert(EndpointSearchTarget endpointSearchTarget)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                Insert(transaction, endpointSearchTarget);

                transaction.Commit();
            }
        }

        public void InsertMany(IEnumerable<EndpointSearchTarget> endpointSearchTargets)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                foreach (var endpointSearchTarget in endpointSearchTargets)
                {
                    Insert(transaction, endpointSearchTarget);
                }

                transaction.Commit();
            }
        }

        public EndpointSearchTarget Select(int id)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<EndpointSearchTarget> obj = transaction
                    .Select<byte[], byte[]>(_table, 1.ToIndex(id))
                    .ObjectGet<EndpointSearchTarget>();

                if (obj != null)
                {
                    return obj.Entity;
                }

                return null;
            }
        }

        public EndpointSearchTarget Select(int searchTargetId, int endpointId)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<EndpointSearchTarget> obj = transaction
                    .Select<byte[], byte[]>(_table, 2.ToIndex(searchTargetId, endpointId))
                    .ObjectGet<EndpointSearchTarget>();

                if (obj != null)
                {
                    return obj.Entity;
                }

                return null;
            }
        }

        public IEnumerable<EndpointSearchTarget> SelectMany(int searchTargetId)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                var entities = new List<EndpointSearchTarget>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction.SelectForwardFromTo<byte[], byte[]>(
                    _table, 3.ToIndex(searchTargetId, int.MinValue, true), true,
                    3.ToIndex(searchTargetId, int.MaxValue), true);

                foreach (var row in rows)
                {
                    DBreezeObject<EndpointSearchTarget> obj = row.ObjectGet<EndpointSearchTarget>();
                    if (obj != null)
                    {
                        EndpointSearchTarget entity = obj.Entity;
                        entities.Add(entity);
                    }
                }

                return entities;
            }
        }

        public IEnumerable<EndpointSearchTarget> SelectAll()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                var entities = new List<EndpointSearchTarget>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction.SelectForward<byte[], byte[]>(_table);
                
                foreach (var row in rows)
                {
                    DBreezeObject<EndpointSearchTarget> obj = row.ObjectGet<EndpointSearchTarget>();
                    if (obj != null)
                    {
                        EndpointSearchTarget entity = obj.Entity;
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
        /// a endpoint search target
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="endpointSearchTarget"></param>
        private void Insert(Transaction transaction, EndpointSearchTarget endpointSearchTarget)
        {
            bool newEntity = endpointSearchTarget.ID == 0;
            if (newEntity)
            {
                endpointSearchTarget.ID = transaction.ObjectGetNewIdentity<int>(_table);
            }

            transaction.ObjectInsert(_table, new DBreezeObject<EndpointSearchTarget>
            {
                NewEntity = newEntity,
                Entity = endpointSearchTarget,
                Indexes = new List<DBreezeIndex>
                {
                    new DBreezeIndex(1, endpointSearchTarget.ID)
                    {
                        PrimaryIndex = true
                    },
                    new DBreezeIndex(2, endpointSearchTarget.SearchTargetID, endpointSearchTarget.EndpointID)
                    {
                        AddPrimaryToTheEnd = false
                    },
                    new DBreezeIndex(3, endpointSearchTarget.SearchTargetID)
                    {
                        AddPrimaryToTheEnd = true
                    }
                }
            });
        }
    }
}
