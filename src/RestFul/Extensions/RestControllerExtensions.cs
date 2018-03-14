using System;
using System.Linq;
using RestFul.Attributes;

namespace RestFul.Extensions
{
    static class RestControllerExtensions
    {
        /// <summary>
        /// Indicates whether or not a class is a valid RestController
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsRestController(this Type type)
        {
            if (!type.IsClass || type.IsAbstract)
            {
                return false;
            }

            return type.GetCustomAttributes(true)
                .Any(attr => attr is RestControllerAttribute);
        }

        /// <summary>
        /// Retrieves the RestControllerAttribute
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static RestControllerAttribute GetControllerAttribute(this Type type)
        {
            if (!type.IsRestController())
            {
                return null;
            }

            return (RestControllerAttribute)type.GetCustomAttributes(true)
                .First(attr => attr is RestControllerAttribute);
        }
    }
}
