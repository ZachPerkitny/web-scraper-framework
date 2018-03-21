using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RestFul.Attributes;
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
        public HashSet<IRoute> Routes { get; private set; }

        private readonly IRestFulLogger _logger;
        private readonly IRouteFactory _routeFactory;
        private bool _initialized;

        public Router(IRestFulLogger logger, IRouteFactory routeFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _routeFactory = routeFactory ?? throw new ArgumentNullException(nameof(routeFactory));
            Routes = new HashSet<IRoute>();
        }

        public IResult Route(HttpContext httpContext)
        {
            IRoute route = Routes.FirstOrDefault(r => r.IsMatch(httpContext));
            if (route == null)
            {
                return new EmptyResult(HttpStatusCode.NotFound);
            }

            return route.Execute(httpContext);
        }

        public void Initialize()
        {
            if (!_initialized)
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Add(assembly);
                }

                _initialized = true;
            }
        }

        public void Add(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes().Where(t => t.IsRestController()))
            {
                Add(type);
            }
        }

        public void Add(Type type)
        {
            foreach (MethodInfo method in type.GetMethods().Where(m => m.IsRestRoute()))
            {
                RestControllerAttribute attr = type.GetControllerAttribute();
                Add(method, attr.BasePath);
            }
        }

        public void Add(MethodInfo method, string basePath)
        {
            foreach (RestRouteAttribute routeAttr in method.GetRouteAttributes())
            {
                string path = CreatePath(basePath, routeAttr.Path);
                _logger.Information("Registering Route {0} {1}", routeAttr.HttpMethod, path);
                IRoute route = _routeFactory.Create(method, routeAttr.HttpMethod, path);
                if (!Routes.Add(route))
                {
                    throw new DuplicateRouteException("Duplicate Route {0}", route);
                }
            }
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
