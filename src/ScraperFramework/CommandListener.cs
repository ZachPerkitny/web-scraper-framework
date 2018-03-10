using System;
using System.Threading.Tasks;
using MediatR;
using ScraperFramework.Attributes;
using ScraperFramework.Data.Entities;
using ScraperFramework.Exceptions;
using ScraperFramework.Handlers;

namespace ScraperFramework
{
    class CommandListener : HttpRequestHandler
    {
        private readonly IMediator _mediator;

        public CommandListener(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpRequest("GET", "keyword")]
        private Task<Keyword> GetKeyword(int id)
        {
            if (id < 0)
            {
                throw new BadRequest("Invalid ID.");
            }

            return _mediator.Send(new GetKeywordRequest
            {
                ID = id
            });
        }

        [HttpRequest("POST", "keyword")]
        private Task PostKeyword()
        {
            return _mediator.Send(new PostKeywordRequest
            {

            });
        }
    }
}
