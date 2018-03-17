using System;
using System.Linq;
using System.Reflection;
using FlatFileDB.Attributes;

namespace FlatFileDB.Extensions
{
    static class FixedRecordAttributeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsFixedRecord(this Type type)
        {
            if (!type.IsClass)
            {
                return false;
            }

            return type.GetCustomAttributes()
                .Any(attr => attr is DelimetedRecordAttribute);
        }
    }
}
