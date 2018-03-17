using System.Linq;
using System.Reflection;
using FlatFileDB.Attributes;

namespace FlatFileDB.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    static class FixedLengthFieldAttributeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static bool HasFixedLength(this FieldInfo fieldInfo)
        {
            return fieldInfo.GetCustomAttributes()
                .Any(attr => attr is FixedLengthFieldAttribute);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static bool HasFixedLength(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes()
                .Any(attr => attr is FixedLengthFieldAttribute);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static uint GetFixedLength(this FieldInfo fieldInfo)
        {
            if (!fieldInfo.HasFixedLength())
            {
                return 0;
            }

            return ((FixedLengthFieldAttribute)fieldInfo.GetCustomAttributes(true)
                .First(attr => attr is FixedLengthFieldAttribute)).Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static uint GetFixedLength(this PropertyInfo propertyInfo)
        {
            if (!propertyInfo.HasFixedLength())
            {
                return 0;
            }

            return ((FixedLengthFieldAttribute)propertyInfo.GetCustomAttributes(true)
                .First(attr => attr is FixedLengthFieldAttribute)).Length;
        }
    }
}
