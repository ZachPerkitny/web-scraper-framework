using System;
using System.Linq;
using System.Reflection;

namespace FlatFileDB.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FixedTableAttribute : TableAttribute { }

    internal static class FixedTableAttributeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsFixedTable(this Type type)
        {
            if (!type.IsClass)
            {
                return false;
            }

            return type.GetCustomAttributes()
                .Any(attr => attr is FixedTableAttribute);
        }
    }
}
