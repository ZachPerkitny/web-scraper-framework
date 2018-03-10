using System;

namespace ScraperFramework.Data.Entities
{
    public class KeywordSearchTarget
    {
        public int ID { get; set; }

        public SearchTarget SearchTarget { get; set; }

        public Keyword Keyword { get; set; }
    }
}
