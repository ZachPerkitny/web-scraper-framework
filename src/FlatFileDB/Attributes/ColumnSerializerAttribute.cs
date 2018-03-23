using System;
using System.Linq;
using System.Reflection;
using FlatFileDB.Serializer;

namespace FlatFileDB.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ColumnSerializerAttribute : Attribute
    {
        public ISerializer Serializer { get; set; }
    }

    internal static class ColumnSerializerAttributeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static bool IsSerializedColumn(this FieldInfo fieldInfo)
        {
            return fieldInfo.GetCustomAttributes()
                .Any(attr => attr is ColumnSerializerAttribute);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static ISerializer GetSerializer(this FieldInfo fieldInfo)
        {
            if (!fieldInfo.IsNamedColumn())
            {
                return null;
            }

            return ((ColumnSerializerAttribute)fieldInfo.GetCustomAttributes()
                .First(attr => attr is ColumnSerializerAttribute)).Serializer;
        }
    }
}
