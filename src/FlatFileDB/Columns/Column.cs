using System.Reflection;
using FlatFileDB.Attributes;
using FlatFileDB.Serializer;

namespace FlatFileDB.Columns
{
    internal abstract class Column : IColumn
    {
        protected Column(FieldInfo fieldInfo)
        {
            Field = fieldInfo;
            Name = fieldInfo.GetColumnName() ?? fieldInfo.Name;
        }

        public FieldInfo Field { get; private set; }

        public string Name { get; private set; }

        public ISerializer Serializer { get; private set; }

        public abstract object Deserialize(string col);

        public abstract string Serialize(object obj);
    }
}
