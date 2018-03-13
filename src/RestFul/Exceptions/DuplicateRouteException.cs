using System;

namespace RestFul.Exceptions
{
    /// <summary>
    /// Exception thrown when there is a duplicate route.
    /// </summary>
    class DuplicateRouteException : RestFulException
    {
        public DuplicateRouteException(string message)
            : base(message) { }

        public DuplicateRouteException(string format, params object[] args)
            : base(format, args) { }

        public DuplicateRouteException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
