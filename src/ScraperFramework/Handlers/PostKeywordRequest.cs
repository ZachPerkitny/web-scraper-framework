using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ScraperFramework.Services;
using MediatR;

namespace ScraperFramework.Handlers
{
    class PostKeywordRequest : IRequest
    {
        public IEnumerable<string> Keywords { get; set; }
    }

    class PostKeywordHandler : IRequestHandler<PostKeywordRequest>
    {
        private readonly IKeywordSearchTargetService _keywordSearchTargetService;

        public PostKeywordHandler(IKeywordSearchTargetService keywordSearchTargetService)
        {
            _keywordSearchTargetService = keywordSearchTargetService;
        }

        public Task Handle(PostKeywordRequest message, CancellationToken cancellationToken)
        {
            return _keywordSearchTargetService.InsertKeywords(message.Keywords);
        }
    }
}
