using System.Reflection;
using RestFul.Enum;

namespace RestFul.Routing
{
    public interface IRouteFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="httpMethod"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        IRoute Create(MethodInfo methodInfo, HttpMethod httpMethod, string path);
    }
}
