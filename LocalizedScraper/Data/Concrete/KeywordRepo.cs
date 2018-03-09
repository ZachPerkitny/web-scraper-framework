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
    class KeywordRepo : IKeywordRepo
    {
        private const string _table = "Keyword";
        private readonly DBreezeEngine _engine;

        public KeywordRepo(DBreezeEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }

        public void Insert(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                throw new ArgumentException(nameof(keyword));
            }

            using (Transaction transaction = _engine.GetTransaction())
            {
                Keyword entity = new Keyword
                {
                    // Automatically get monotonically grown entity ID
                    ID = transaction.ObjectGetNewIdentity<int>(_table),
                    Value = keyword
                };

                // Insert New Keyword Entity
                transaction.ObjectInsert(_table, new DBreezeObject<Keyword>
                {
                    NewEntity = true,
                    Entity = entity,
                    Indexes = new List<DBreezeIndex>
                    {
                        new DBreezeIndex(1, entity.ID)
                        {
                            PrimaryIndex = true
                        }
                    }
                });

                transaction.Commit();
            }
        }

        public void InsertMany(IEnumerable<string> keywords)
        {
            throw new NotImplementedException();
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
                IEnumerable<Row<byte[], byte[]>> rows = transaction.SelectForward<byte[], byte[]>(_table);

                foreach (var row in rows)
                {
                    DBreezeObject<Keyword> obj = row.ObjectGet<Keyword>();
                    Keyword entity = obj.Entity;
                    entities.Add(entity);
                }

                return entities;
            }
        }

        public void Delete(int keywordID)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                transaction
                    .RemoveKey<byte[]>(_table, 1.ToIndex(keywordID));

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
    }
}
