using System;
using RestAPI.Http;

namespace RestAPI.Routing
{
    public interface IRouter
    {
        void AddRoutes();

        void ExecuteRoute(IHttpContext httpContext);
    }
}
