using System;
using System.Linq;
using System.Reflection;

namespace FlatFileDB.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ColumnNameAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }

    internal static class ColumnNameAttributeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static bool IsNamedColumn(this FieldInfo fieldInfo)
        {
            return fieldInfo.GetCustomAttributes()
                .Any(attr => attr is ColumnNameAttribute);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static string GetColumnName(this FieldInfo fieldInfo)
        {
            if (!fieldInfo.IsNamedColumn())
            {
                return null;
            }

            return ((ColumnNameAttribute)fieldInfo.GetCustomAttributes()
                .First(attr => attr is ColumnNameAttribute)).Name;
        }
    }

}
