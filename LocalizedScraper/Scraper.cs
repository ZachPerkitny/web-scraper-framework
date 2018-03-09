using System;
using System.Threading;
using System.Threading.Tasks;
using ScraperFramework.Handlers;
using ScraperFramework.Pocos;
using MediatR;

namespace ScraperFramework
{
    class Scraper : IScraper
    {
        private readonly IMediator _mediator;
        private readonly CancellationToken _cancellationToken;

        public Scraper(IMediator mediator, CancellationToken cancellationToken)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _cancellationToken = cancellationToken;
        }

        public void Start()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                Task<CrawlDescription> getCrawlTask = _mediator.Send(
                    new CrawlDescriptionRequest());

                getCrawlTask.Wait();
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
