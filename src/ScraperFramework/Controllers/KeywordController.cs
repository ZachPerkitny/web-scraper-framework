﻿using System;
using System.Collections.Generic;
using RestFul.Attributes;
using RestFul.Enum;
using RestFul.Http;
using RestFul.Result;
using ScraperFramework.Data;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Controllers
{
    [RestController(BasePath = "/keywords")]
    public class KeywordController
    {
        private readonly IKeywordRepo _keywordRepo;

        public KeywordController(IKeywordRepo keywordRepo)
        {
            _keywordRepo = keywordRepo ?? throw new ArgumentNullException(nameof(keywordRepo));
        }

        [RestRoute(HttpMethod = HttpMethod.GET)]
        public IResult GetKeywords(HttpContext context)
        {
            IEnumerable<Keyword> keywords = _keywordRepo.SelectAll();

            return new SerializedResult(keywords);
        }

        [RestRoute(HttpMethod = HttpMethod.POST)]
        public IResult PostKeyword(HttpContext context)
        {
            if (!context.Request.HasEntityBody)
            {
                return new EmptyResult(HttpStatusCode.BadRequest);
            }

            IEnumerable<string> keywords = null;
            try
            {
                keywords = context.Serializer.Deserialize<IEnumerable<string>>(
                    context.Request.DataBody);
            }
            catch(Exception)
            {
                return new EmptyResult(HttpStatusCode.BadRequest);
            }

            _keywordRepo.InsertMany(keywords);

            return new EmptyResult(HttpStatusCode.NoContent);
        }
    }
}
