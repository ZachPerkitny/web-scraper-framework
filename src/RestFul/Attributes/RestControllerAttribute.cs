using System;

namespace RestFul.Attributes
{
    /// <summary>
    /// Class attribute for defining a Rest Controller
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RestControllerAttribute : Attribute
    {
        /// <summary>
        /// Value prepended to the routes in the controller. Defaults
        /// to empty string.
        /// </summary>
        public string BasePath { get; set; }

        public RestControllerAttribute()
        {
            BasePath = string.Empty;
        }
    }
}
