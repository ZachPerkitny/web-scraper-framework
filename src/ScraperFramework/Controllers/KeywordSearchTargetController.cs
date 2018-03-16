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
    [RestController(BasePath = "/keyword-search-targets")]
    public class KeywordSearchTargetController
    {
        private readonly IKeywordSearchTargetRepo _keywordSearchTargetRepo;

        public KeywordSearchTargetController(IKeywordSearchTargetRepo keywordSearchTargetRepo)
        {
            _keywordSearchTargetRepo = keywordSearchTargetRepo ?? throw new ArgumentNullException(nameof(keywordSearchTargetRepo));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.GET)]
        public IResult GetKeywordSearchTargets(HttpContext context)
        {
            IEnumerable<KeywordSearchTarget> keywordSearchTargets = null;

            if (context.Request.QueryString["search-target"] != null)
            {
                try
                {
                    int searchTargetId = int.Parse(context.Request.QueryString["search-target"]);
                    keywordSearchTargets = _keywordSearchTargetRepo.SelectMany(searchTargetId);
                }
                catch (Exception)
                {
                    return new EmptyResult(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                keywordSearchTargets = _keywordSearchTargetRepo.SelectAll();
            }

            return new SerializedResult(keywordSearchTargets);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.GET, Path = @"\d+")]
        public IResult GetKeywordSearchTarget(HttpContext context)
        {
            int keywordSearchTargetId = int.Parse(context.Request.StrParams[1]);
            KeywordSearchTarget keywordSearchTarget = _keywordSearchTargetRepo.Select(keywordSearchTargetId);

            return new SerializedResult(keywordSearchTarget);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.POST)]
        public IResult PostKeywordSearchTarget(HttpContext context)
        {
            if (!context.Request.HasEntityBody)
            {
                return new EmptyResult(HttpStatusCode.BadRequest);
            }

            IEnumerable<KeywordSearchTarget> keywordSearchTargets = null;
            try
            {
                keywordSearchTargets = context.Serializer
                    .Deserialize<IEnumerable<KeywordSearchTarget>>(context.Request.DataBody);
            }
            catch (Exception)
            {
                return new EmptyResult(HttpStatusCode.BadRequest);
            }

            _keywordSearchTargetRepo.InsertMany(keywordSearchTargets);

            return new SerializedResult(keywordSearchTargets);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.GET, Path = "/count")]
        public IResult GetKeywordSearchTargetCount(HttpContext context)
        {
            return new SerializedResult(new
            {
                KeywordSearchTargetCount = _keywordSearchTargetRepo.Count()
            });
        }
    }
}
