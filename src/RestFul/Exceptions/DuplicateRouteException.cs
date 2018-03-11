using System;

namespace RestFul.Exceptions
{
    class DuplicateRouteException : Exception
    {
        public DuplicateRouteException(string message)
            : base(message) { }

        public DuplicateRouteException(string format, params object[] args)
            : this(string.Format(format, args)) { }
    }
}
