﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
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

        private bool _disposed = false;

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task RequestMoreCrawlDescriptions()
        {
            PipelinedCrawlDescription pipelinedCrawlDescription = _pipeline.Drain();
            while (!pipelinedCrawlDescription.CrawlDescriptions.Any())
            {
                int delay = (int)(pipelinedCrawlDescription.NextAvailability - DateTime.Now).TotalMilliseconds;

                Log.Information("No Crawl Descriptions Available. Draining again in {0}ms.", (delay < 0) ? 0 : delay);
                if (delay > 0)
                {
                    await Task.Delay(delay);
                }

                pipelinedCrawlDescription = _pipeline.Drain();
            }

            foreach (CrawlDescription crawl in pipelinedCrawlDescription.CrawlDescriptions)
            {
                _queue.Enqueue(crawl);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _semaphore.Dispose();
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
