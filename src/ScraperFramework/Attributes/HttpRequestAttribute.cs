using System;

namespace ScraperFramework.Attributes
{
    class HttpRequestAttribute : Attribute
    {
        private static readonly string[] _supportedMethods = new[]
        {
            "GET",
            "DELETE",
            "POST",
            "PUT",
            "PATCH"
        };

        public string HttpMethod { get; private set; }

        public string Template { get; private set; }

        public HttpRequestAttribute(string httpMethod, string template)
        {
            if (httpMethod == null)
            {
                throw new ArgumentNullException(nameof(httpMethod));
            }

            if (Array.IndexOf(_supportedMethods, httpMethod.ToUpper()) == -1)
            {
                throw new ArgumentException($"Unsupported Method: {httpMethod}");
            }

            HttpMethod = httpMethod;
            Template = template ?? throw new ArgumentNullException(nameof(template));
        }
    }
}
