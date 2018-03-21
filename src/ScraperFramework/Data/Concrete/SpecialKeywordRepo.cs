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
    class SpecialKeywordRepo : ISpecialKeywordRepo
    {
        private const string _table = "SpecialKeywords";
        private readonly DBreezeEngine _engine;

        public SpecialKeywordRepo(DBreezeEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }

        public void Insert(SpecialKeyword specialKeyword)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                Insert(transaction, specialKeyword);

                transaction.Commit();
            }
        }

        public void InsertMany(IEnumerable<SpecialKeyword> specialKeywords)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                foreach (SpecialKeyword specialKeyword in specialKeywords)
                {
                    Insert(transaction, specialKeyword);
                }

                transaction.Commit();
            }
        }

        public SpecialKeyword Select(int searchEngineId, int regionId, int keywordId)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<SpecialKeyword> obj = transaction
                    .Select<byte[], byte[]>(
                        _table, 1.ToIndex(searchEngineId, regionId, keywordId))
                    .ObjectGet<SpecialKeyword>();

                if (obj != null)
                {
                    SpecialKeyword entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public IEnumerable<SpecialKeyword> SelectAll()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                List<SpecialKeyword> entities = new List<SpecialKeyword>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction
                    .SelectForwardFromTo<byte[], byte[]>(
                    _table, 1.ToIndex(int.MinValue, int.MinValue, int.MinValue), true,
                    1.ToIndex(int.MaxValue, int.MaxValue, int.MaxValue), true);

                foreach (Row<byte[], byte[]> row in rows)
                {
                    DBreezeObject<SpecialKeyword> obj = row.ObjectGet<SpecialKeyword>();

                    if (obj != null)
                    {
                        SpecialKeyword entity = obj.Entity;
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

        public SpecialKeyword Max()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<SpecialKeyword> obj = transaction.Max<byte[], byte[]>(_table)
                    .ObjectGet<SpecialKeyword>();

                if (obj != null)
                {
                    SpecialKeyword entity = obj.Entity;
                    return entity;
                }

                return null;
            }
        }

        public SpecialKeyword Min()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<SpecialKeyword> obj = transaction.Min<byte[], byte[]>(_table)
                    .ObjectGet<SpecialKeyword>();

                if (obj != null)
                {
                    SpecialKeyword entity = obj.Entity;
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
        /// <param name="specialKeyword"></param>
        private void Insert(Transaction transaction, SpecialKeyword specialKeyword)
        {
            transaction.ObjectInsert(_table, new DBreezeObject<SpecialKeyword>
            {
                Entity = specialKeyword,
                Indexes = new List<DBreezeIndex>
                {
                    new DBreezeIndex(1, specialKeyword.SearchEngineID, specialKeyword.RegionID, specialKeyword.KeywordID)
                    {
                        PrimaryIndex = true
                    },
                    new DBreezeIndex(2, specialKeyword.RowRevision)
                    {
                        AddPrimaryToTheEnd = false
                    }
                }
            });
        }
    }
}
