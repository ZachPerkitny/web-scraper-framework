using System;
using System.Collections.Generic;
using System.Reflection;
using RestFul.Http;
using RestFul.Result;

namespace RestFul.Routing
{
    public interface IRouter
    {
        /// <summary>
        /// 
        /// </summary>
        HashSet<IRoute> Routes { get; }

        /// <summary>
        /// Executes any method that matches the current HTTP
        /// context
        /// </summary>
        /// <param name="httpContext"></param>
        IResult Route(HttpContext httpContext);

        /// <summary>
        /// 
        /// </summary>
        void ScanAssemblies();

        /// <summary>
        /// 
        /// </summary>
        void Register(Assembly assembly);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        void Register(Type type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        void Register(MethodInfo method, string basePath);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        IRoute GetRouteForContext(HttpContext httpContext);
    }
}
