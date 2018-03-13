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
    class SearchTargetRepo : ISearchTargetRepo
    {
        private const string _table = "SearchTarget";
        private readonly DBreezeEngine _engine;

        public SearchTargetRepo(DBreezeEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            DBreezeInitialization.SetupUtils();
        }

        public void Insert(SearchTarget searchTarget)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                bool newEntity = searchTarget.ID == 0;
                if (newEntity)
                {
                    searchTarget.ID = transaction.ObjectGetNewIdentity<int>(_table);
                }

                transaction.ObjectInsert(_table, new DBreezeObject<SearchTarget>
                {
                    NewEntity = newEntity,
                    Entity = searchTarget,
                    Indexes = new List<DBreezeIndex>
                    {
                        new DBreezeIndex(1, searchTarget.ID)
                        {
                            PrimaryIndex = true
                        },
                        new DBreezeIndex(2, searchTarget.CountryID, searchTarget.CityID, searchTarget.IsMobile)
                        {
                            AddPrimaryToTheEnd = false
                        }
                    }
                });

                transaction.Commit();
            }
        }

        public SearchTarget Select(int id)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<SearchTarget> obj = transaction
                    .Select<byte[], byte[]>(_table, 1.ToIndex(id))
                    .ObjectGet<SearchTarget>();

                if (obj != null)
                {
                    return obj.Entity;
                }

                return null;
            }
        }

        public SearchTarget Select(int countryId, int cityId, bool isMobile)
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                DBreezeObject<SearchTarget> obj = transaction
                    .Select<byte[], byte[]>(_table, 2.ToIndex(countryId, cityId, isMobile))
                    .ObjectGet<SearchTarget>();

                if (obj != null)
                {
                    return obj.Entity;
                }

                return null;
            }
        }

        public IEnumerable<SearchTarget> SelectAll()
        {
            using (Transaction transaction = _engine.GetTransaction())
            {
                var entities = new List<SearchTarget>();
                IEnumerable<Row<byte[], byte[]>> rows = transaction.SelectForward<byte[], byte[]>(_table);

                foreach (var row in rows)
                {
                    DBreezeObject<SearchTarget> obj = row.ObjectGet<SearchTarget>();

                    if (obj != null)
                    {
                        SearchTarget entity = obj.Entity;
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
    }
}
