using RestFul.DI;
using RestFul.Http;
using RestFul.Loggers;
using RestFul.Routing;
using RestFul.Routing.Concrete;
using RestFul.Serializer;

namespace RestFul.Configuration
{
    static class RestFulRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public static void RegisterComponents(IContainer container)
        {
            container
                .Register((_) => container)
                .Register<IRestFulLogger, NullLogger>()
                .Register<ISerializer, JsonSerializer>()
                .Register<IHttpListener, HttpListener>()
                .Register<IRouter, Router>()
                .Register<IRestFulServer, RestFulServer>();
        }
    }
}
