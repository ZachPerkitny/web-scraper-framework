using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RestFul.Attributes;
using RestFul.DI;
using RestFul.Exceptions;
using RestFul.Extensions;
using RestFul.Http;
using RestFul.Loggers;
using RestFul.Result;

namespace RestFul.Routing.Concrete
{
    class Router : IRouter
    {
        public static readonly List<Assembly> Assemblies;

        public HashSet<IRoute> Routes { get; private set; }

        private readonly IRestFulLogger _logger;
        private readonly IContainer _container;

        static Router()
        {
            Assemblies = new List<Assembly>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Assemblies.Add(assembly);
            }
        }

        public Router(IRestFulLogger logger, IContainer container)
        {
            _logger = logger;
            _container = container;
            Routes = new HashSet<IRoute>();
        }

        public IResult Route(HttpContext httpContext)
        {
            IRoute route = GetRouteForContext(httpContext);
            if (route == null)
            {
                throw new APIException("Not Found", Enum.HttpStatusCode.NotFound);
            }

            return route.Invoke(httpContext);
        }

        public void ScanAssemblies()
        {
            foreach (Assembly assembly in Assemblies)
            {
                Register(assembly);
            }
        }

        public void Register(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes().Where(t => t.IsRestController()))
            {
                Register(type);
            }
        }

        public void Register(Type type)
        {
            foreach (MethodInfo method in type.GetMethods().Where(m => m.IsRestRoute()))
            {
                RestControllerAttribute attr = type.GetControllerAttribute();
                Register(method, attr.BasePath);
            }
        }

        public void Register(MethodInfo method, string basePath)
        {
            foreach (RestRouteAttribute routeAttr in method.GetRouteAttributes())
            {
                string path = CreatePath(basePath, routeAttr.Path);
                _logger.Debug("Registering Route {0} {1}", routeAttr.HttpMethod, path);
                Route route = new Route(method, routeAttr.HttpMethod, path, _container);
                if (!Routes.Add(route))
                {
                    throw new DuplicateRouteException("Duplicate Route {0}", route);
                }
            }
        }

        public IRoute GetRouteForContext(HttpContext httpContext)
        {
            return Routes.FirstOrDefault(route => route.Matches(httpContext));
        }

        private string CreatePath(string basePath, string path)
        {
            if (!string.IsNullOrEmpty(basePath) && !basePath.StartsWith("/"))
            {
                basePath = $"/{basePath}";
            }

            if (!string.IsNullOrEmpty(path) && !path.StartsWith("/"))
            {
                path = $"{path}";
            }

            return $"{basePath}{path}";
        }
    }
}
