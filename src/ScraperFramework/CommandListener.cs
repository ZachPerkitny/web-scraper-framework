using System;
using System.Threading.Tasks;
using MediatR;
using RestFul.Attributes;
using RestFul.Enum;
using ScraperFramework.Data.Entities;
using ScraperFramework.Handlers;

namespace ScraperFramework
{
    [RestController]
    class CommandListener
    {
        private readonly IMediator _mediator;

        public CommandListener(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [RestRoute(HttpMethod = HttpMethod.GET, Path = "/keywords")]
        public Task<Keyword> GetKeyword(int id)
        {
            return _mediator.Send(new GetKeywordRequest
            {
                ID = id
            });
        }

        [RestRoute(HttpMethod = HttpMethod.POST, Path = "/keywords")]
        public Task PostKeyword()
        {
            return _mediator.Send(new PostKeywordRequest
            {

            });
        }
    }
}
