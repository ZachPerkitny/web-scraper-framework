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
    class LocalUULERepo : ILocalUULERepo
    {
        private const string _table = "LocalUULEs";
        private readonly DBreezeEngine _engine;

        public LocalUULERepo(DBreezeEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            DBreezeInitialization.SetupUtils();
        }

        public void Insert(LocalUULE localUULE)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                Insert(transaction, localUULE);

                transaction.Commit();
            }
        }

        public void InsertMany(IEnumerable<LocalUULE> localUULEs)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                foreach (LocalUULE localUULE in localUULEs)
                {
                    Insert(transaction, localUULE);
                }

                transaction.Commit();
            }
        }

        public LocalUULE Select(int cityID)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<LocalUULE> obj = transaction
                    .Select<byte[], byte[]>(_table, 2.ToIndex(cityID))
                    .ObjectGet<LocalUULE>();

                if (obj != null)
                {
                    LocalUULE entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public IEnumerable<LocalUULE> SelectAll()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                List<LocalUULE> entities = new List<LocalUULE>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction
                    .SelectForwardFromTo<byte[], byte[]>(
                    _table, 1.ToIndex(int.MinValue), true,
                    1.ToIndex(int.MaxValue), true);

                foreach (Row<byte[], byte[]> row in rows)
                {
                    DBreezeObject<LocalUULE> obj = row.ObjectGet<LocalUULE>();

                    if (obj != null)
                    {
                        LocalUULE entity = obj.Entity;
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
        /// a local uule entity.
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="localUULE"></param>
        private void Insert(Transaction transaction, LocalUULE localUULE)
        {
            bool newEntity = localUULE.ID == 0;
            if (newEntity)
            {
                localUULE.ID = transaction.ObjectGetNewIdentity<int>(_table);
            }

            transaction.ObjectInsert(_table, new DBreezeObject<LocalUULE>
            {
                NewEntity = newEntity,
                Entity = localUULE,
                Indexes = new List<DBreezeIndex>
                {
                    new DBreezeIndex(1, localUULE.ID)
                    {
                        PrimaryIndex = true
                    },
                    new DBreezeIndex(2, localUULE.CityID)
                    {
                        AddPrimaryToTheEnd = false
                    }
                }
            });
        }
    }
}
