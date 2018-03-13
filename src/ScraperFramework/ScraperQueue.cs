using System.Collections.Generic;
using ScraperFramework.Pocos;
using ScraperFramework.Services;

namespace ScraperFramework
{
    class ScraperQueue : IScraperQueue
    {
        private const int KEYWORD_COUNT = 5000;

        private readonly ICrawlService _crawlService;
        private readonly Queue<CrawlDescription> _queue = new Queue<CrawlDescription>();
        private readonly object _locker = new object();

        public ScraperQueue(ICrawlService crawlService)
        {
            _crawlService = crawlService;
        }

        public CrawlDescription Dequeue()
        {
            lock (_locker)
            {
                if (_queue.Count == 0)
                {
                    RequestMoreCrawlDescriptions();
                }

                return _queue.Dequeue();
            } 
        }

        private void RequestMoreCrawlDescriptions()
        {
            IEnumerable<CrawlDescription> crawlDescriptions = _crawlService.GetKeywordsToCrawl(KEYWORD_COUNT);
            lock (_locker)
            {
                foreach (var crawl in crawlDescriptions)
                {
                    _queue.Enqueue(crawl);
                }
            }
        }
    }
}
