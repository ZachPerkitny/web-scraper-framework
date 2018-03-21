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
    class KeywordRepo : IKeywordRepo
    {
        private const string _table = "Keyword";
        private readonly DBreezeEngine _engine;

        public KeywordRepo(DBreezeEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            DBreezeInitialization.SetupUtils();
        }

        public void Insert(Keyword keyword)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                Insert(transaction, keyword);

                transaction.Commit();
            }
        }

        public void InsertMany(IEnumerable<Keyword> keywords)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                foreach (Keyword keyword in keywords)
                {
                    Insert(transaction, keyword);
                }

                transaction.Commit();
            }
        }

        public Keyword Select(int keywordID)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<Keyword> obj = transaction
                    .Select<byte[], byte[]>(_table, 1.ToIndex(keywordID))
                    .ObjectGet<Keyword>();

                if (obj != null)
                {
                    return obj.Entity;
                }

                return null;
            }
        }

        public IEnumerable<Keyword> SelectAll()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                var entities = new List<Keyword>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction
                    .SelectForwardFromTo<byte[], byte[]>(_table,
                    1.ToIndex(int.MinValue), true,
                    1.ToIndex(int.MaxValue), true);

                foreach (var row in rows)
                {
                    DBreezeObject<Keyword> obj = row.ObjectGet<Keyword>();
                    if (obj != null)
                    {
                        Keyword entity = obj.Entity;
                        entities.Add(entity);
                    } 
                }

                return entities;
            }
        }

        public void Delete(int keywordID)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                transaction
                    .RemoveKey(_table, 1.ToIndex(keywordID));

                transaction.Commit();
            }
        }

        public void DeleteAll()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                // without file recreation
                transaction
                    .RemoveAllKeys(_table, false);

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

        public Keyword Max()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<Keyword> obj = transaction.Max<byte[], byte[]>(_table)
                    .ObjectGet<Keyword>();

                if (obj != null)
                {
                    Keyword entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public Keyword Min()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<Keyword> obj = transaction.Min<byte[], byte[]>(_table)
                    .ObjectGet<Keyword>();

                if (obj != null)
                {
                    Keyword entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public byte[] GetLatestRevision()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                // this should be done to take advantage of dbreeze's
                // lazy loading, value is never actually loaded
                // from disk. However, the key includes an extra byte,
                // TODO(zvp): Figure out why
                IEnumerable<Row<byte[], byte[]>> rows = transaction
                    .SelectBackwardStartFrom<byte[], byte[]>(
                    _table, BitConverter.GetBytes(ulong.MaxValue), true);

                if (rows.Any())
                {
                    DBreezeObject<Keyword> obj = rows.First()
                        .ObjectGet<Keyword>();

                    if (obj != null)
                    {
                        byte[] latestRevision = obj.Entity.RowRevision;
                        return latestRevision;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Does an object insert and creates the necessary indexes for
        /// a keyword
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="keyword"></param>
        private void Insert(Transaction transaction, Keyword keyword)
        {
            bool newEntity = keyword.ID == 0;
            if (newEntity)
            {
                keyword.ID = transaction.ObjectGetNewIdentity<int>(_table);
            }

            transaction.ObjectInsert(_table, new DBreezeObject<Keyword>
            {
                NewEntity = newEntity,
                Entity = keyword,
                Indexes = new List<DBreezeIndex>
                {
                    new DBreezeIndex(1, keyword.ID)
                    {
                        PrimaryIndex = true
                    },
                    new DBreezeIndex(2, keyword.RowRevision)
                    {
                        AddPrimaryToTheEnd = false
                    }
                }
            });
        }
    }
}
