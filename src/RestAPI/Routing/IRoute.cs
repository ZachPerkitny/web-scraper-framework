using RestAPI.Enum;
using RestAPI.Http;

namespace RestAPI.Routing
{
    public interface IRoute
    {
        /// <summary>
        /// Route HTTP Method
        /// </summary>
        HttpMethod HttpMethod { get; }

        /// <summary>
        /// Route Path
        /// </summary>
        string Path { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Matches(IHttpContext httpContext);
    }
}
