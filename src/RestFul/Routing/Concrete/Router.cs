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

        public void Route(IHttpContext httpContext)
        {
            IRoute route = GetRouteForContext(httpContext);
            if (route == null)
            {
                throw new APIException("Not Found", Enum.HttpStatusCode.NotFound);
            }

            route.Invoke(httpContext);
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
                Register(method);
            }
        }

        public void Register(MethodInfo method)
        {
            foreach (RestRouteAttribute routeAttr in method.GetRouteAttributes())
            {
                _logger.Debug("Registering Route {0} {1}", routeAttr.HttpMethod, routeAttr.Path);
                Route route = new Route(method, routeAttr.HttpMethod, routeAttr.Path, _container);
                if (!Routes.Add(route))
                {
                    throw new DuplicateRouteException("Duplicate Route {0}", route);
                }
            }
        }

        public IRoute GetRouteForContext(IHttpContext httpContext)
        {
            return Routes.FirstOrDefault(route => route.Matches(httpContext));
        }
    }
}
