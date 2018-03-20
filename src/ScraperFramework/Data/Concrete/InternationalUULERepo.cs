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
    class InternationalUULERepo : IInternationalUULERepo
    {
        private const string _table = "InternationalUULEs";
        private readonly DBreezeEngine _engine;

        public InternationalUULERepo(DBreezeEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            DBreezeInitialization.SetupUtils();
        }

        public void Insert(InternationalUULE internationalUULE)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                Insert(transaction, internationalUULE);

                transaction.Commit();
            }
        }

        public void InsertMany(IEnumerable<InternationalUULE> internationalUULEs)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                foreach (InternationalUULE internationalUULE in internationalUULEs)
                {
                    Insert(transaction, internationalUULE);
                }

                transaction.Commit();
            }
        }

        public InternationalUULE Select(int cityID)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<InternationalUULE> obj = transaction
                    .Select<byte[], byte[]>(_table, 2.ToIndex(cityID))
                    .ObjectGet<InternationalUULE>();

                if (obj != null)
                {
                    InternationalUULE entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public IEnumerable<InternationalUULE> SelectAll()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                List<InternationalUULE> entities = new List<InternationalUULE>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction
                    .SelectForward<byte[], byte[]>(_table);

                foreach (Row<byte[], byte[]> row in rows)
                {
                    DBreezeObject<InternationalUULE> obj = row.ObjectGet<InternationalUULE>();

                    if (obj != null)
                    {
                        InternationalUULE entity = obj.Entity;
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
        /// a international uule entity.
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="internationalUULE"></param>
        private void Insert(Transaction transaction, InternationalUULE internationalUULE)
        {
            bool newEntity = internationalUULE.ID == 0;
            if (newEntity)
            {
                internationalUULE.ID = transaction.ObjectGetNewIdentity<int>(_table);
            }

            transaction.ObjectInsert(_table, new DBreezeObject<InternationalUULE>
            {
                NewEntity = newEntity,
                Entity = internationalUULE,
                Indexes = new List<DBreezeIndex>
                {
                    new DBreezeIndex(1, internationalUULE.ID)
                    {
                        PrimaryIndex = true
                    },
                    new DBreezeIndex(2, internationalUULE.RegionID)
                    {
                        AddPrimaryToTheEnd = false
                    }
                }
            });
        }
    }
}
