using System;
using System.Collections.Generic;
using ScraperFramework.Data;
using ScraperFramework.Pocos;
using ScraperFramework.Utils;

namespace ScraperFramework
{
    class KeywordManager : IKeywordManager
    {
        private readonly IKeywordScrapeDetailRepo _keywordScrapeDetailRepo;
        private readonly IKeywordRepo _keywordRepo;

        private readonly PriorityQueue<KeywordNode> _keywords = new PriorityQueue<KeywordNode>();
        private readonly Dictionary<Tuple<short, short, int>, KeywordNode> _tempHolding = new Dictionary<Tuple<short, short, int>, KeywordNode>();
        private bool _filledPriorityQueue = false;

        private readonly object _locker = new object();

        public KeywordManager(IKeywordScrapeDetailRepo keywordScrapeDetailRepo, IKeywordRepo keywordRepo)
        {
            _keywordScrapeDetailRepo = keywordScrapeDetailRepo ?? throw new ArgumentNullException(nameof(keywordScrapeDetailRepo));
            _keywordRepo = keywordRepo ?? throw new ArgumentNullException(nameof(keywordRepo));
        }

        public IEnumerable<Keyword> GetKeywords(int count, bool mayBeRequeued)
        {
            lock (_locker)
            {
                if (!_filledPriorityQueue)
                {
                    FillPriorityQueue();
                }

                List<Keyword> keywords = new List<Keyword>();
                int i = 0;
                while (i < count && !_keywords.IsEmpty)
                {
                    KeywordNode node = _keywords.Dequeue();
                    keywords.Add(node.Keyword);
                    if (mayBeRequeued)
                    {
                        Tuple<short, short, int> key = new Tuple<short, short, int>(
                            node.Keyword.SearchEngineID, node.Keyword.RegionID, node.Keyword.KeywordID);
                        _tempHolding[key] = node;
                    }
                    i += 1;
                }

                return keywords;
            }
        }

        public void RequeueKeyword(short searchEngineId, short regionId, int keywordId)
        {
            lock (_locker)
            {
                Tuple<short, short, int> key = new Tuple<short, short, int>(
                    searchEngineId, regionId, keywordId);
                if (_tempHolding.ContainsKey(key))
                {
                    // retrieve node from temp holding
                    // and enqueue it
                    KeywordNode node = _tempHolding[key];
                    _keywords.Enqueue(node);
                    // remove the keyword from temp holding
                    _tempHolding.Remove(key);
                }
                else
                {
                    // throw ?
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FillPriorityQueue()
        {
            IEnumerable<Data.Entities.KeywordScrapeDetail> keywordsToCrawl = _keywordScrapeDetailRepo.SelectToCrawl();
            foreach (Data.Entities.KeywordScrapeDetail keywordScrapeDetail in keywordsToCrawl)
            {
                Data.Entities.Keyword keyword = _keywordRepo.Select(keywordScrapeDetail.KeywordID);
                if (keyword != null)
                {
                    _keywords.Enqueue(new KeywordNode
                    {
                        Keyword = new Keyword
                        {
                            KeywordValue = keyword.Value,
                            KeywordID = keyword.ID,
                            SearchEngineID = keywordScrapeDetail.SearchEngineID,
                            RegionID = keywordScrapeDetail.RegionID,
                            CityID = keywordScrapeDetail.CityID
                        },
                        Priority = keywordScrapeDetail.Priority
                    });
                }
            }

            _filledPriorityQueue = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private class KeywordNode : IComparable<KeywordNode>
        {
            public int Priority { get; set; }

            public Keyword Keyword { get; set; }

            public int CompareTo(KeywordNode other)
            {
                if (Priority < other.Priority)
                {
                    return -1;
                }
                else if (Priority > other.Priority)
                {
                    return 1;

                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
