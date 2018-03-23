using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ScraperFramework.Pipeline;
using ScraperFramework.Pocos;
using ScraperFramework.Shared.Pocos;

namespace ScraperFramework
{
    class ScraperQueue : IScraperQueue
    {
        private const int KEYWORD_COUNT = 5000;

        private readonly PipeLine<PipelinedCrawlDescription> _pipeline;
        private readonly Queue<CrawlDescription> _queue = new Queue<CrawlDescription>();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public ScraperQueue(PipeLine<PipelinedCrawlDescription> pipeline)
        {
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        }

        public async Task<CrawlDescription> Dequeue()
        {
            CrawlDescription crawlDescription;

            try
            {
                await _semaphore.WaitAsync();

                if (_queue.Count == 0)
                {
                    await RequestMoreCrawlDescriptions();
                }

                // TODO: Add Throttle
                crawlDescription = _queue.Dequeue();
            }
            finally
            {
                _semaphore.Release();
            }

            return crawlDescription;
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
