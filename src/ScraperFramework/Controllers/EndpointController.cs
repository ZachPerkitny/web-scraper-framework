using System;
using System.Collections.Generic;
using RestFul.Attributes;
using RestFul.Enum;
using RestFul.Http;
using RestFul.Result;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Controllers
{
    [RestController(BasePath = "/endpoints")]
    public class EndpointController
    {
        private readonly IEndpointRepo _endpointRepo;

        public EndpointController(IEndpointRepo endpointRepo)
        {
            _endpointRepo = endpointRepo ?? throw new ArgumentNullException(nameof(endpointRepo));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.GET)]
        public IResult GetEndpoints(HttpContext context)
        {
            IEnumerable<Endpoint> endpoints = _endpointRepo.SelectAll();

            return new SerializedResult(endpoints);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.GET, Path = @"\d+")]
        public IResult GetEndpoint(HttpContext context)
        {
            int endpointID = int.Parse(context.Request.StrParams[1]);
            Endpoint endpoint = _endpointRepo.Select(endpointID);

            return new SerializedResult(endpoint);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.POST)]
        public IResult PostEndpoint(HttpContext context)
        {
            if (!context.Request.HasEntityBody)
            {
                return new EmptyResult(HttpStatusCode.BadRequest);
            }

            IEnumerable<Endpoint> endpoints = null;
            try
            {
                endpoints = context.Serializer.Deserialize<IEnumerable<Endpoint>>(
                    context.Request.DataBody);
            }
            catch (Exception)
            {
                return new EmptyResult(HttpStatusCode.BadRequest);
            }

            _endpointRepo.InsertMany(endpoints);

            return new EmptyResult(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.GET, Path = "/count")]
        public IResult GetEndpointCount(HttpContext context)
        {
            return new SerializedResult(new
            {
                EndpointCount = _endpointRepo.Count()
            });
        }
    }
}
