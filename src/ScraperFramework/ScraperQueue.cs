using System.Collections.Generic;
using System.Threading.Tasks;
using ScraperFramework.Pocos;

namespace ScraperFramework
{
    class ScraperQueue : IScraperQueue
    {
        private readonly Queue<CrawlDescription> _queue = new Queue<CrawlDescription>();
        private readonly object _locker = new object();

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
            //Task<IEnumerable<CrawlDescription>> getKeywordsTask = _mediator.Send(new RefillRequest
            //{
            //    NumOfKeywords = 5000 // whatever
            //});

            //getKeywordsTask.Wait();

            //if (getKeywordsTask.Status == TaskStatus.Faulted)
            //{
            //    // log it
            //    return;
            //}

            //IEnumerable<CrawlDescription> crawlDescriptions = getKeywordsTask.Result;
            //foreach (var crawl in crawlDescriptions)
            //{
            //    _queue.Enqueue(crawl);
            //}
        }
    }
}
