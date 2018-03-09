using System;
using System.Threading;
using System.Threading.Tasks;
using ScraperFramework.Pocos;
using MediatR;

namespace ScraperFramework.Handlers
{
    class CrawlDescriptionRequest : IRequest<CrawlDescription> { }

    class CrawlDescriptionHandler : IRequestHandler<CrawlDescriptionRequest, CrawlDescription>
    {
        private readonly IScraperQueue _keywordQueue;

        public CrawlDescriptionHandler(IScraperQueue keywordQueue)
        {
            _keywordQueue = keywordQueue;
        }

        public Task<CrawlDescription> Handle(CrawlDescriptionRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_keywordQueue.Dequeue());
        }
    }
}
