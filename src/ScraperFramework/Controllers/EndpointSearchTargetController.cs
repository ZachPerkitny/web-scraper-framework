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
    [RestController(BasePath = "/endpoint-search-targets")]
    public class EndpointSearchTargetController
    {
        private readonly IEndpointSearchTargetRepo _endpointSearchTargetRepo;

        public EndpointSearchTargetController(IEndpointSearchTargetRepo endpointSearchTargetRepo)
        {
            _endpointSearchTargetRepo = endpointSearchTargetRepo ?? throw new ArgumentNullException(nameof(endpointSearchTargetRepo));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.GET)]
        public IResult GetEndpointSearchTargets(HttpContext context)
        {
            IEnumerable<EndpointSearchTarget> endpointSearchTargets = null;

            if (context.Request.QueryString["search-target"] != null)
            {
                try
                {
                    int searchTargetId = int.Parse(context.Request.QueryString["search-target"]);
                    endpointSearchTargets = _endpointSearchTargetRepo.SelectMany(searchTargetId);
                }
                catch (Exception)
                {
                    return new EmptyResult(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                endpointSearchTargets = _endpointSearchTargetRepo.SelectAll();
            }

            return new SerializedResult(endpointSearchTargets);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.GET, Path = @"\d+")]
        public IResult GetEndpointSearchTarget(HttpContext context)
        {
            int endpointSearchTargetId = int.Parse(context.Request.StrParams[1]);
            EndpointSearchTarget endpointSearchTarget = _endpointSearchTargetRepo.Select(endpointSearchTargetId);

            return new SerializedResult(endpointSearchTarget);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.POST)]
        public IResult PostEndpointSearchTarget(HttpContext context)
        {
            if (!context.Request.HasEntityBody)
            {
                return new EmptyResult(HttpStatusCode.BadRequest);
            }

            IEnumerable<EndpointSearchTarget> endpointSearchTargets = null;
            try
            {
                endpointSearchTargets = context.Serializer
                    .Deserialize<IEnumerable<EndpointSearchTarget>>(context.Request.DataBody);
            }
            catch (Exception)
            {
                return new EmptyResult(HttpStatusCode.BadRequest);
            }

            _endpointSearchTargetRepo.InsertMany(endpointSearchTargets);

            return new EmptyResult(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.GET, Path = "/count")]
        public IResult GetEndpointSearchTargetCount(HttpContext context)
        {
            return new SerializedResult(new
            {
                EndpointSearchTargetCount = _endpointSearchTargetRepo.Count()
            });
        }
    }
}
