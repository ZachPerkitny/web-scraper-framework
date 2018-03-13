using System;

namespace RestFul.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a method with <see cref="Attributes.RestRouteAttribute"/>
    /// is invalid.
    /// </summary>
    class InvalidRouteMethod : RestFulException
    {
        public InvalidRouteMethod(string message)
            : base(message) { }

        public InvalidRouteMethod(string format, params object[] args)
            : base(format, args) { }

        public InvalidRouteMethod(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
