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
    [RestController(BasePath = "/crawl-logs")]
    public class CrawlLogController
    {
        private readonly ICrawlLogRepo _crawlLogRepo;

        public CrawlLogController(ICrawlLogRepo crawlLogRepo)
        {
            _crawlLogRepo = crawlLogRepo ?? throw new ArgumentNullException(nameof(crawlLogRepo));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.GET)]
        public IResult GetCrawlLogs(HttpContext context)
        {
            IEnumerable<CrawlLog> crawlLogs = _crawlLogRepo.SelectAll();

            return new SerializedResult(crawlLogs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [RestRoute(HttpMethod = HttpMethod.GET, Path = "/total-count")]
        public IResult GetKeywordCount(HttpContext context)
        {
            return new SerializedResult(new
            {
                CrawlLogCount = _crawlLogRepo.Count()
            });
        }
    }
}
