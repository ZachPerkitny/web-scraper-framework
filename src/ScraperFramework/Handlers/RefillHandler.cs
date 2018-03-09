using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ScraperFramework.Pocos;
using ScraperFramework.Services;
using MediatR;

namespace ScraperFramework.Handlers
{
    class RefillRequest : IRequest<IEnumerable<CrawlDescription>>
    {
        public int NumOfKeywords { get; set; }

        public int SearchTargetID { get; set; }
    }

    class RefillHandler : IRequestHandler<RefillRequest, IEnumerable<CrawlDescription>>
    {
        private readonly ICrawlService _crawlService;

        public RefillHandler(ICrawlService crawlService)
        {
            _crawlService = crawlService;
        }

        public Task<IEnumerable<CrawlDescription>> Handle(RefillRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
