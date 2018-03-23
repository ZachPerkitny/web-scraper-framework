using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FlatFileDB.Attributes;
using FlatFileDB.Columns;

namespace FlatFileDB.Tables
{
    internal class Table<T> : ITable<T>
    {
        public static Table<T> Create()
        {
            Type tableType = typeof(T);

            // ensure the poco has a table attribute defined
            if (tableType.IsTable())
            {
                // whether or not to include the header 
                // in the flat file
                bool useHeader = tableType.UsesHeader();

                // generate columns specific to the table
                // type
                if (tableType.IsDelimitedTable())
                {
                    List<IColumn> columns = new List<IColumn>();

                    FieldInfo[] fields = tableType.GetFields(
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    for (int i = 0; i < fields.Length; i++)
                    {
                        columns.Add(DelimitedColumn.Create(
                            fields[i], i, i == (fields.Length - 1)));
                    }

                    return new Table<T>(useHeader, columns);
                }
                // otherwise it's a fixed table
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new Exception();
            }
        }

        private Table(bool includeHeader, IEnumerable<IColumn> columns)
        {
            IncludeHeader = includeHeader;
            Columns = columns;
        }

        public IEnumerable<IColumn> Columns { get; private set; }

        public int ColumnCount
        {
            get { return Columns.Count(); }
        }

        public IEnumerable<string> ColumnNames
        {
            get { return Columns.Select(col => col.Name); }
        }

        public bool IncludeHeader { get; set; }

        public bool IsDelimitedTable
        {
            get { return typeof(T).IsDelimitedTable(); }
        }

        public bool IsFixedTable
        {
            get { return typeof(T).IsFixedTable(); }
        }

        public string BuildHeader()
        {
            StringBuilder sb = new StringBuilder();
            string delimiter = (IsDelimitedTable) ? 
                typeof(T).GetDelimiter() : "\t";

            List<string> columnNames = Columns
                .Select(col => col.Name).ToList();
            for (int i = 0; i < columnNames.Count; i++)
            {
                if (i == columnNames.Count - 1)
                {
                    sb.Append(columnNames[i]);
                }
                else
                {
                    sb.AppendFormat("{0}{1}", columnNames[i], delimiter);
                }
            }

            return sb.ToString();
        }

        public string BuildRow(T entity)
        {
            StringBuilder sb = new StringBuilder();

            foreach (IColumn column in Columns)
            {
                sb.Append(column.Serialize(entity));
            }

            return sb.ToString();
        }

        public T ParseRow(string row)
        {
            throw new NotImplementedException();
        }
    }
}
