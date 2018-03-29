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

        public IDictionary<int, Keyword> SelectMany(IEnumerable<int> keywordIds)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                Dictionary<int, Keyword> entities = new Dictionary<int, Keyword>();
                foreach (int keywordId in keywordIds)
                {
                    DBreezeObject<Keyword> obj = transaction
                        .Select<byte[], byte[]>(_table, 1.ToIndex(keywordId))
                        .ObjectGet<Keyword>();

                    if (obj != null)
                    {
                        entities.Add(keywordId, obj.Entity);
                    }
                }

                return entities;
            }
        }

        public IEnumerable<Keyword> SelectAll()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                IEnumerable<Row<byte[], byte[]>> rows = transaction
                    .SelectForwardFromTo<byte[], byte[]>(
                    _table, 1.ToIndex(int.MinValue), true,
                    1.ToIndex(int.MaxValue), true);

                List<Keyword> entities = new List<Keyword>();
                foreach (Row<byte[], byte[]> row in rows)
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
        /// a keyword
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="keyword"></param>
        private void Insert(Transaction transaction, Keyword keyword)
        {
            bool newEntity = keyword.ID == 0;
            if (newEntity)
            {
                // gets monotonically grown identity
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
