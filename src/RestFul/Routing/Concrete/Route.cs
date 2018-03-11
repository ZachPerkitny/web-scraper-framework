using System.Reflection;
using RestFul.Enum;
using RestFul.Http;

namespace RestFul.Routing.Concrete
{
    class Route : IRoute
    {
        public HttpMethod HttpMethod { get; private set; }

        public string Path { get; private set; }

        public MethodInfo Method { get; private set; }

        public Route(MethodInfo method, HttpMethod httpMethod, string path)
        {
            Method = method;
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

        public override string ToString()
        {
            return $"{HttpMethod.ToString()} {Path}";
        }

        public override bool Equals(object obj)
        {
            Route route = obj as Route;
            if (route == null)
            {
                return false;
            }
            else
            {
                return HttpMethod.Equals(route.HttpMethod) &&
                    Path.Equals(route.Path);
            }
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
