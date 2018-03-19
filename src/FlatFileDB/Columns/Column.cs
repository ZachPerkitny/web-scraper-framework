using System;
using System.Reflection;
using FlatFileDB.Attributes;
using FlatFileDB.Serializer;

namespace FlatFileDB.Columns
{
    internal class Column : IColumn
    {
        public static IColumn Create(FieldInfo fieldInfo)
        {
            IColumn column = new Column(fieldInfo)
            {
                Name = fieldInfo.GetColumnName() ?? fieldInfo.Name
            };

            return column;
        }

        private Column(FieldInfo fieldInfo)
        {
            Field = fieldInfo;
        }

        public FieldInfo Field { get; private set; }

        public string Name { get; set; }

        public ISerializer Serializer { get; set; }

        public object Deserialize(byte[] col)
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
