using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScraperFramework.Pipeline;
using ScraperFramework.Pocos;
using WebScraper.Pocos;

namespace ScraperFramework
{
    class ScraperQueue : IScraperQueue
    {
        private const int KEYWORD_COUNT = 5000;

        private readonly PipeLine<PipelinedCrawlDescription> _pipeline;
        private DateTime _nextAvailability;
        private readonly Queue<CrawlDescription> _queue = new Queue<CrawlDescription>();
        private readonly object _locker = new object();

        public ScraperQueue(PipeLine<PipelinedCrawlDescription> pipeline)
        {
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
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
            PipelinedCrawlDescription pipelinedCrawlDescription = _pipeline.Drain();
            if (!pipelinedCrawlDescription.CrawlDescriptions.Any())
            {
                await Task.Delay((pipelinedCrawlDescription.NextAvailability - DateTime.Now).Milliseconds);
                pipelinedCrawlDescription = _pipeline.Drain();
            }

            foreach (CrawlDescription crawl in pipelinedCrawlDescription.CrawlDescriptions)
            {
                _queue.Enqueue(crawl);
            }
        }
    }
}
