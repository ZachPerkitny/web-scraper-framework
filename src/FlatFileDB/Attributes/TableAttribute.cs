using System;
using System.Linq;
using System.Reflection;

namespace FlatFileDB.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public abstract class TableAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public bool UseHeader { get; set; }
    }

    internal static class TableAttributeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsTable(this Type type)
        {
            if (!type.IsClass)
            {
                return false;
            }

            return type.GetCustomAttributes()
                .Any(attr => attr is TableAttribute);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool UsesHeader(this Type type)
        {
            if (!type.IsTable())
            {
                return false;
            }

            return ((TableAttribute)type.GetCustomAttributes()
                .First(attr => attr is TableAttribute)).UseHeader;
        }
    }
}
