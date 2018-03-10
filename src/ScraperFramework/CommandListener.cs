using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using ScraperFramework.Attributes;
using ScraperFramework.Data.Entities;
using ScraperFramework.Exceptions;
using ScraperFramework.Handlers;

namespace ScraperFramework
{
    class CommandListener : IHttpRequestHandler
    {
        private readonly IMediator _mediator;

        public CommandListener(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<object> Execute(HttpListenerRequest listenerRequest)
        {
            string template = listenerRequest.Url.Segments[1].Replace("/", "");

            MethodInfo method = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(m => m.ReturnType.GetGenericTypeDefinition() == typeof(Task<>) && m.GetCustomAttributes().Any(attr =>
            {
                return attr is CommandAttribute &&
                    ((CommandAttribute)attr).HttpMethod == listenerRequest.HttpMethod &&
                    ((CommandAttribute)attr).Template == template;
            }))
            .FirstOrDefault();

            if (method != null)
            {
                string[] strParameters = listenerRequest.Url.Segments
                    .Skip(2).Select(p => p.Replace("/", "")).ToArray();

                if (strParameters.Length != method.GetParameters().Length)
                {
                    throw new BadRequest("Expected {0} Parameters, Got {1}.", 
                        method.GetParameters().Length, strParameters.Length);
                }

                object[] parameters = method.GetParameters()
                    .Select((p, i) => Convert.ChangeType(strParameters[i], p.ParameterType))
                    .ToArray();

                try
                {
                    Task task = (Task) method.Invoke(this, parameters);
                    await task;
                    return (object)((dynamic) task).Result;
                }
                catch (Exception)
                {
                    throw new InternalServerError();
                }
            }
            else
            {
                throw new NotFound();
            }
        }

        [Command("GET", "keyword")]
        private Task<Keyword> GetKeyword(int id)
        {
            return _mediator.Send(new GetKeywordRequest
            {
                ID = id
            });
        }

        [Command("POST", "keyword")]
        private Task PostKeyword()
        {

        }
    }
}
