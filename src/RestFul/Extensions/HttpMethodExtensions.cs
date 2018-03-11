using System.Collections.Generic;
using RestFul.Enum;

namespace RestFul.Extensions
{
    static class HttpMethodExtensions
    {
        private readonly static IDictionary<string, int> _stringMethodMap;

        static HttpMethodExtensions()
        {
            _stringMethodMap = new Dictionary<string, int>();
            foreach (object value in System.Enum.GetValues(typeof(HttpMethod)))
            {
                _stringMethodMap[value.ToString()] = (int)value;
            }
        }

        /// <summary>
        /// Converts a string to an Http Method Enum, or returns 0 (GET).
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static HttpMethod ToHttpMethod(this string str)
        {
            string key = str.ToUpper();
            if (_stringMethodMap.ContainsKey(key))
            {
                return (HttpMethod)_stringMethodMap[key];
            }

            return 0;
        }
    }
}
