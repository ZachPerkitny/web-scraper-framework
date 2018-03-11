using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ScraperFramework.Data.Entities;
using ScraperFramework.Services;
using MediatR;

namespace ScraperFramework.Handlers
{
    class SearchTargetRequest : IRequest<IEnumerable<SearchTarget>> { }

    class SearchTargetHandler : IRequestHandler<SearchTargetRequest, IEnumerable<SearchTarget>>
    {
        private readonly ICrawlService _crawlService;

        public SearchTargetHandler(ICrawlService crawlService)
        {
            _crawlService = crawlService;
        }

        public Task<IEnumerable<SearchTarget>> Handle(SearchTargetRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_crawlService.GetSearchTargets());
        }
    }
}
