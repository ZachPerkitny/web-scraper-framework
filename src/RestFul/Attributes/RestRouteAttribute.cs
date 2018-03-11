using System;
using RestFul.Enum;

namespace RestFul.Attributes
{
    /// <summary>
    /// Method attribute for defining a route
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RestRouteAttribute : Attribute
    {
        /// <summary>
        /// The HTTP Method this method will be called on. Defaults to GET.
        /// </summary>
        public HttpMethod HttpMethod { get; set; }

        /// <summary>
        /// Path to Match. Defaults to Empty String.
        /// </summary>
        public string Path { get; set; }

        public RestRouteAttribute()
        {
            HttpMethod = HttpMethod.GET;
            Path = string.Empty;
        }
    }
}
