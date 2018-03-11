using RestFul.Enum;
using RestFul.Http;

namespace RestFul.Routing
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
