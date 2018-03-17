using System.Text.RegularExpressions;
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
        /// Path Regular Expression
        /// </summary>
        Regex PathPattern { get; }

        /// <summary>
        /// 
        /// </summary>
        IResult Execute(HttpContext httpContext);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsMatch(HttpContext httpContext);
    }
}
