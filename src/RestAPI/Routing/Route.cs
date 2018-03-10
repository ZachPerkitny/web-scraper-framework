using System.Text.RegularExpressions;
using RestAPI.Enum;
using RestAPI.Http;

namespace RestAPI.Routing
{
    class Route : IRoute
    {
        public HttpMethod HttpMethod { get; private set; }

        public string Path { get; private set; }

        private Regex _pathRegex;

        public Route(HttpMethod httpMethod, string path)
        {
            Path = path;
            HttpMethod = httpMethod;
        }

        public bool Matches(IHttpContext httpContext)
        {
            if (httpContext.Request.HttpMethod == HttpMethod && httpContext.Request.Path == Path)
            {
                return true;
            }

            return false;
        }
    }
}
