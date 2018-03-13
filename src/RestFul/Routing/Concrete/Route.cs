using System;
using System.Reflection;
using RestFul.DI;
using RestFul.Enum;
using RestFul.Extensions;
using RestFul.Http;
using RestFul.Result;

namespace RestFul.Routing.Concrete
{
    class Route : IRoute
    {
        public HttpMethod HttpMethod { get; private set; }

        public string Path { get; private set; }

        public Func<IHttpContext, IResult> Method { get; private set; }

        private readonly IContainer _container;

        public Route(MethodInfo method, HttpMethod httpMethod, string path, IContainer container)
        {
            Method = CreateRouteFunc(method);
            Path = path;
            HttpMethod = httpMethod;
            _container = container;
        }

        public IResult Invoke(IHttpContext httpContext)
        {
            return Method.Invoke(httpContext);
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

        private Func<IHttpContext, IResult> CreateRouteFunc(MethodInfo method)
        {
            method.IsValidRoute(true);
            
            if (method.IsStatic || method.ReflectedType == null)
            {
                return (context) => (IResult)method.Invoke(null, new object[] { context });
            }

            return (context) =>
            {
                object instance = _container.Resolve(method.ReflectedType);
                return (IResult)method.Invoke(instance, new object[] { context });
            };
        }
    }
}
