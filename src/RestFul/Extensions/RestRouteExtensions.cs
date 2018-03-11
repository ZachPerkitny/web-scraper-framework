using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RestFul.Attributes;

namespace RestFul.Extensions
{
    static class RestRouteExtensions
    {
        /// <summary>
        /// Indicates whether a method is a rest route
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static bool IsRestRoute(this MethodInfo methodInfo)
        {
            if (methodInfo.GetCustomAttributes().Any(attr => attr is RestRouteAttribute))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns all route attributes for a method
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static IEnumerable<RestRouteAttribute> GetRouteAttributes(this MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes()
                .Where(attr => attr is RestRouteAttribute)
                .Cast<RestRouteAttribute>();
        }
    }
}
