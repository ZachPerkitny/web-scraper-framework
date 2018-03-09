using System;

namespace ScraperFramework.Attributes
{
    class CommandAttribute : Attribute
    {
        public string HttpMethod { get; private set; }

        public string Template { get; private set; }

        public CommandAttribute(string httpMethod, string template)
        {
            HttpMethod = httpMethod;
            Template = template;
        }
    }
}
