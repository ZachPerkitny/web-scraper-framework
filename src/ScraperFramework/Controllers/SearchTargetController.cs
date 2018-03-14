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
    [RestController(BasePath = "/search-targets")]
    public class SearchTargetController
    {
        private readonly ISearchTargetRepo _searchTargetRepo;

        public SearchTargetController(ISearchTargetRepo searchTargetRepo)
        {
            _searchTargetRepo = searchTargetRepo ?? throw new ArgumentNullException(nameof(searchTargetRepo));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.GET)]
        public IResult GetSearchTargets(HttpContext context)
        {
            IEnumerable<SearchTarget> searchTargets = _searchTargetRepo.SelectAll();

            return new SerializedResult(searchTargets);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.GET, Path = @"\d+")]
        public IResult GetSearchTarget(HttpContext context)
        {
            int id = int.Parse(context.Request.StrParams[1]);
            SearchTarget searchTarget = _searchTargetRepo.Select(id);

            return new SerializedResult(searchTarget);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.POST)]
        public IResult PostSearchTarget(HttpContext context)
        {
            if (!context.Request.HasEntityBody)
            {
                return new EmptyResult(HttpStatusCode.BadRequest);
            }

            SearchTarget searchTarget = null;
            try
            {
                searchTarget = context.Serializer.Deserialize<SearchTarget>(
                    context.Request.DataBody);
            }
            catch (Exception)
            {
                return new EmptyResult(HttpStatusCode.BadRequest);
            }

            _searchTargetRepo.Insert(searchTarget);

            return new SerializedResult(searchTarget);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.GET, Path = "/count")]
        public IResult GetSearchTargetCount(HttpContext context)
        {
            return new SerializedResult(new
            {
                SearchTargetCount = _searchTargetRepo.Count()
            });
        }
    }
}
