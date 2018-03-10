using System;
using System.Threading;
using System.Threading.Tasks;
using ScraperFramework.Data.Entities;
using ScraperFramework.Services;
using MediatR;

namespace ScraperFramework.Handlers
{
    class GetKeywordRequest : IRequest<Keyword>
    {
        public int ID { get; set; }
    }

    class GetKeywordHandler : IRequestHandler<GetKeywordRequest, Keyword>
    {
        private readonly IKeywordSearchTargetService _keywordSearchTargetService;

        public GetKeywordHandler(IKeywordSearchTargetService keywordSearchTargetService)
        {
            _keywordSearchTargetService = keywordSearchTargetService;
        }

        public Task<Keyword> Handle(GetKeywordRequest request, CancellationToken cancellationToken)
        {
            return _keywordSearchTargetService.GetKeyword(request.ID);
        }
    }
}
