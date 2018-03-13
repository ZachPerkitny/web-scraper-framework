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
    class KeywordSearchTargetRepo : IKeywordSearchTargetRepo
    {
        private const string _table = "KeywordSearchTarget";
        private readonly DBreezeEngine _engine;

        public KeywordSearchTargetRepo(DBreezeEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            DBreezeInitialization.SetupUtils();
        }

        public void Insert(KeywordSearchTarget keywordSearchTarget)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                bool newEntity = keywordSearchTarget.ID == 0;
                if (newEntity)
                {
                    keywordSearchTarget.ID = transaction.ObjectGetNewIdentity<int>(_table);
                }

                transaction.ObjectInsert(_table, new DBreezeObject<KeywordSearchTarget>
                {
                    NewEntity = newEntity,
                    Entity = keywordSearchTarget,
                    Indexes = new List<DBreezeIndex>
                    {
                        new DBreezeIndex(1, keywordSearchTarget.ID)
                        {
                            PrimaryIndex = true
                        },
                        new DBreezeIndex(2, keywordSearchTarget.SearchTargetID, keywordSearchTarget.KeywordID)
                        {
                            AddPrimaryToTheEnd = false
                        },
                        new DBreezeIndex(3, keywordSearchTarget.SearchTargetID)
                        {
                            AddPrimaryToTheEnd = true
                        }
                    }
                });
            }
        }

        public KeywordSearchTarget Select(int id)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<KeywordSearchTarget> obj = transaction
                    .Select<byte[], byte[]>(_table, 1.ToIndex(id))
                    .ObjectGet<KeywordSearchTarget>();

                if (obj != null)
                {
                    return obj.Entity;
                }

                return null;
            }
        }

        public KeywordSearchTarget Select(int searchTargetId, int keywordId)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<KeywordSearchTarget> obj = transaction
                    .Select<byte[], byte[]>(_table, 2.ToIndex(searchTargetId, keywordId))
                    .ObjectGet<KeywordSearchTarget>();

                if (obj != null)
                {
                    return obj.Entity;
                }

                return null;
            }
        }

        public IEnumerable<KeywordSearchTarget> SelectMany(int searchTargetId)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                var entities = new List<KeywordSearchTarget>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction.SelectForwardFromTo<byte[], byte[]>(
                    _table, 3.ToIndex(searchTargetId, int.MinValue), true,
                    3.ToIndex(searchTargetId, int.MaxValue), true);

                foreach (var row in rows)
                {
                    DBreezeObject<KeywordSearchTarget> obj = row.ObjectGet<KeywordSearchTarget>();
                    if (obj != null)
                    {
                        KeywordSearchTarget entity = obj.Entity;
                        entities.Add(entity);
                    }
                }

                return entities;
            }
        }
    }
}
