using System;
using System.Reflection;
using FlatFileDB.Attributes;

namespace FlatFileDB.Columns
{
    internal class DelimitedColumn : Column, IDelimitedColumn
    {
        /// <summary>
        /// Creates and returns a DelimitedColumn object
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static IDelimitedColumn Create(FieldInfo fieldInfo, int index, bool isLast)
        {
            // for now just ignore invalid attributes
            if (fieldInfo == null)
            {
                throw new ArgumentNullException(nameof(fieldInfo));
            }

            if (!fieldInfo.ReflectedType.IsDelimitedTable())
            {
                throw new Exception();
            }

            string delimiter = fieldInfo.ReflectedType.GetDelimiter();
            return new DelimitedColumn(fieldInfo, index, isLast, delimiter);
        }

        private DelimitedColumn(FieldInfo fieldInfo, int index, bool isLast, string delimiter)
            : base(fieldInfo, index, isLast)
        {
            if (string.IsNullOrEmpty(delimiter))
            {
                throw new ArgumentException();
            }

            Delimiter = delimiter;
        }

        public string Delimiter { get; private set; }

        public override object Deserialize(string col)
        {
            throw new NotImplementedException();
        }

        public override string Serialize(object obj)
        {
            object value = Field.GetValue(obj);
            string serializedValue = Serializer.Serialize(value);
            
            if (IsLast)
            {
                return serializedValue;
            }
            else
            {
                return $"{serializedValue}{Delimiter}";
            }
        }
    }
}
