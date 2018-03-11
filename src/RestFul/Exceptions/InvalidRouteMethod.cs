using System;

namespace RestFul.Exceptions
{
    class InvalidRouteMethod : Exception
    {
        public InvalidRouteMethod(string message)
            : base(message) { }

        public InvalidRouteMethod(string format, params object[] args)
            : this(string.Format(format, args)) { }
    }
}
