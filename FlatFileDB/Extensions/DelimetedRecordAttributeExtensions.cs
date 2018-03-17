using System;
using System.Linq;
using System.Reflection;
using FlatFileDB.Attributes;

namespace FlatFileDB.Extensions
{
    static class DelimetedRecordAttributeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDelimetedRecord(this Type type)
        {
            if (!type.IsClass)
            {
                return false;
            }

            return type.GetCustomAttributes()
                .Any(attr => attr is DelimetedRecordAttribute);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetDelimter(this Type type)
        {
            if (!type.IsDelimetedRecord())
            {
                return null;
            }

            return ((DelimetedRecordAttribute)type.GetCustomAttributes(true)
                .First(attr => attr is DelimetedRecordAttribute)).Delimeter;
        }
    }
}
