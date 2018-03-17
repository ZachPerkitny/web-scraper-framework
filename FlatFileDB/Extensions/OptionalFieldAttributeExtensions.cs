using System.Reflection;
using FlatFileDB.Attributes;

namespace FlatFileDB.Extensions
{
    static class OptionalFieldAttributeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static bool IsOptionalField(this FieldInfo fieldInfo)
        {
            return fieldInfo.GetCustomAttributes()
                .Any(attr => attr is OptionalFieldAttribute);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool IsOptionalField(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes()
                .Any(attr => attr is OptionalFieldAttribute);
        }
    }
}
