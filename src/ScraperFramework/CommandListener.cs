using System;
using System.Threading.Tasks;
using MediatR;
using RestFul.Attributes;
using RestFul.Enum;
using RestFul.Http;
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
        public Task GetKeyword(IHttpContext context)
        {
            return Task.FromResult(5);
        }

        [RestRoute(HttpMethod = HttpMethod.POST, Path = "/keywords")]
        public Task PostKeyword(IHttpContext context)
        {
            return Task.FromResult(5);
        }
    }
}
