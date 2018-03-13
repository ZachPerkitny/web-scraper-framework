using System;
using System.Threading;
using System.Threading.Tasks;
using ScraperFramework.Pocos;

namespace ScraperFramework
{
    class Scraper : IScraper
    {
        private readonly CancellationToken _cancellationToken;

        public Scraper(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
        }

        public void Start()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                //Task<CrawlDescription> getCrawlTask = _mediator.Send(
                //    new CrawlDescriptionRequest());

                //getCrawlTask.Wait();
            }
        }

        public void Pause()
        {
            throw new NotImplementedException();
        } 

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
