using System;

namespace RestFul.Exceptions
{
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
