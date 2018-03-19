using System;
using System.Linq;
using System.Reflection;

namespace FlatFileDB.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DelimitedTableAttribute : TableAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Delimiter { get; private set; }

        public DelimitedTableAttribute(string delimiter)
        {
            if (string.IsNullOrEmpty(delimiter))
            {
                // TODO (zvp): Add FlatFile Exceptions
                throw new Exception();
            }

            Delimiter = delimiter;
        }
    }

    internal static class DelimitedTableAttributeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDelimitedTable(this Type type)
        {
            if (!type.IsClass)
            {
                return false;
            }

            return type.GetCustomAttributes()
                .Any(attr => attr is DelimitedTableAttribute);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetDelimiter(this Type type)
        {
            if (!type.IsDelimitedTable())
            {
                return null;
            }

            return ((DelimitedTableAttribute)type.GetCustomAttributes()
                .First(attr => attr is DelimitedTableAttribute)).Delimiter;
        }
    }
}
