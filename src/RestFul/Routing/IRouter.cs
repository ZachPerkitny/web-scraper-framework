using System;
using System.Collections.Generic;
using System.Reflection;
using RestFul.Http;

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
        void Route(IHttpContext httpContext);

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
        void Register(MethodInfo method);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        List<IRoute> GetRoutesForContext(IHttpContext httpContext);
    }
}
