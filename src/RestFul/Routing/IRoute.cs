using RestFul.Enum;
using RestFul.Http;
using RestFul.Result;

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
        IResult Invoke(HttpContext httpContext);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Matches(HttpContext httpContext);
    }
}
