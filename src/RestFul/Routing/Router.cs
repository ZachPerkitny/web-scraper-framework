using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RestFul.Attributes;
using RestFul.DI;
using RestFul.Enum;
using RestFul.Exceptions;
using RestFul.Extensions;
using RestFul.Http;
using RestFul.Loggers;
using RestFul.Result;

namespace RestFul.Routing
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
                return new EmptyResult(HttpStatusCode.NotFound);
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

        /// <summary>
        /// Creates a the Route Path by appending the basePath and 
        /// the path. If the path begins with a caret, it is moved to
        /// the front of the path. If the basePath does not begin with
        /// a forward slash, it is prepended, so it can be properly matched.
        /// If the path does not start with a forward slash, and the basePath
        /// does not end with a forward slash, a forward slash is prepended
        /// to the path so it can be properly matched.
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private string CreatePath(string basePath, string path)
        {
            // if both are null, just set to index route
            if (basePath == null && path == null)
            {
                return "/";
            }

            string caret = string.Empty;

            // Handle Caret in front of Route Attr Path, Move it to front
            // of final constructed path
            if (path != null && path.StartsWith("^"))
            {
                path.TrimStart(new char[] { '^' });
                caret = "^";
            }

            // Prepend slash to base path if the first character is not a slash
            if (basePath != null && !basePath.StartsWith("/"))
            {
                basePath = $"/{basePath}";
            }

            // Prepend slash to path if the first character is not a slash
            // and the basePath is null or does not end with a slash
            if ((path != null && !path.StartsWith("/")) && 
                (basePath == null || !basePath.EndsWith("/")))
            {
                path = $"/{path}";
            }

            return $"{caret}{basePath}{path}";
        }
    }
}
