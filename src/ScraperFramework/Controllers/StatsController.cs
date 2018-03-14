using System;
using RestFul.Attributes;
using RestFul.Enum;
using RestFul.Http;
using RestFul.Result;
using ScraperFramework.Pocos;
using ScraperFramework.Services;

namespace ScraperFramework.Controllers
{
    [RestController(BasePath = "/stats")]
    public class StatsController
    {
        private readonly IStatsService _statsService;

        public StatsController(IStatsService statsService)
        {
            _statsService = statsService ?? throw new ArgumentNullException(nameof(statsService));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.GET)]
        public IResult GetCrawlStats(HttpContext context)
        {
            CrawlStats stats = _statsService.GetCrawlStats();
            return new SerializedResult(stats);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.GET, Path = @"search-target/\d+")]
        public IResult GetCrawlStatsForSearchTarget(HttpContext context)
        {
            int searchTargetId = int.Parse(context.Request.StrParams[2]);
            CrawlStats stats = _statsService.GetCrawlStatsForSearchTarget(searchTargetId);
            return new SerializedResult(stats);
        }
    }
}
