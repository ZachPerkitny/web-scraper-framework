using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScraperFramework.Services;
using WebScraper.Pocos;

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
            _crawlService = crawlService ?? throw new ArgumentNullException(nameof(crawlService));
        }

        public async Task<CrawlDescription> Dequeue()
        {
            if (_queue.Count == 0)
            {
                await RequestMoreCrawlDescriptions();
            }

            lock (_locker)
            {
                return _queue.Dequeue();
            } 
        }

        private async Task RequestMoreCrawlDescriptions()
        {
            IEnumerable<CrawlDescription> crawlDescriptions = _crawlService.GetKeywordsToCrawl(KEYWORD_COUNT);
            if (!crawlDescriptions.Any())
            {
                await Task.Delay((_crawlService.NextAvailability - DateTime.Now).Milliseconds);
            }

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
