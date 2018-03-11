using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RestFul.Attributes;
using RestFul.Extensions;
using RestFul.Http;
using RestFul.Loggers;

namespace RestFul.Routing.Concrete
{
    class Router : IRouter
    {
        public static readonly List<Assembly> Assemblies;

        public HashSet<IRoute> Routes { get; private set; }

        private readonly IRestfulLogger _logger;

        static Router()
        {
            Assemblies = new List<Assembly>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Assemblies.Add(assembly);
            }
        }

        public Router(IRestfulLogger logger)
        {
            _logger = logger;
            Routes = new HashSet<IRoute>();
        }

        public void Route(IHttpContext httpContext)
        {
            List<IRoute> toExecute = GetRoutesForContext(httpContext);

            if (toExecute.Count == 0)
            {

                return;
            }

            foreach (var route in toExecute)
            {

            }
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
                Route route = new Route(method, routeAttr.HttpMethod, routeAttr.Path);
                Routes.Add(route);
            }
        }

        public List<IRoute> GetRoutesForContext(IHttpContext httpContext)
        {
            return Routes.Where(route => route.Matches(httpContext)).ToList();
        }
    }
}
