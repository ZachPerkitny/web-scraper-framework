using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RestFul.Attributes;
using RestFul.Exceptions;
using RestFul.Http;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="throwIfInvalid"></param>
        /// <returns></returns>
        public static bool IsValidRoute(this MethodInfo methodInfo, bool throwIfInvalid = false)
        {
            if (methodInfo.ReturnType != typeof(Task) && methodInfo.ReturnType != typeof(Task<>))
            {
                if (throwIfInvalid)
                {
                    throw new InvalidRouteMethod("Expected Route Method {0} to return a Task.", methodInfo.Name);
                }

                return false;
            }

            if (methodInfo.GetParameters().Length != 1)
            {
                if (throwIfInvalid)
                {
                    throw new InvalidRouteMethod("Expected Route Method {0} to have a single parameter.", methodInfo.Name);
                }

                return false;
            }

            if (methodInfo.GetParameters()[0].ParameterType != typeof(IHttpContext))
            {
                if (throwIfInvalid)
                {
                    throw new InvalidRouteMethod("Expected Route Method {0}'s argument to be of type {1}",
                        methodInfo.Name, typeof(IHttpContext).Name);
                }

                return false;
            }

            return true;
        }
    }
}
