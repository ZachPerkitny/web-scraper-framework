using System.Reflection;
using RestFul.DI;
using RestFul.Enum;

namespace RestFul.Routing
{
    class RouteFactory : IRouteFactory
    {
        private readonly IContainer _container;

        public RouteFactory(IContainer container)
        {
            _container = container;
        }

        public IRoute Create(MethodInfo methodInfo, HttpMethod httpMethod, string path)
        {
            return new Route(methodInfo, httpMethod, path, _container);
        }
    }
}
