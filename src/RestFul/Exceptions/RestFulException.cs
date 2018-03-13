using System;

namespace RestFul.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class RestFulException : Exception
    {
        public RestFulException(string message)
            : base(message) { }

        public RestFulException(string message, params object[] format)
            : base(string.Format(message, format)) { }

        public RestFulException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
