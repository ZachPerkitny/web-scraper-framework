using System;
using System.Collections.Generic;
using System.Linq;
using FlatFileDB.Attributes;
using FlatFileDB.Columns;

namespace FlatFileDB.Tables
{
    internal class Table<T> : ITable<T>
    {
        public static Table<T> Create()
        {
            Type tableType = typeof(T);

            if (tableType.IsTable())
            {
                bool useHeader = tableType.UsesHeader();

                if (tableType.IsDelimitedTable())
                {
                    List<IDelimitedColumn> columns = new List<IDelimitedColumn>();

                    return new Table<T>(useHeader, columns);
                }
                else if (tableType.IsFixedTable())
                {
                    List<IFixedColumn> columns = new List<IFixedColumn>();

                    return new Table<T>(useHeader, columns);
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

        public byte[] BuildHeader()
        {
            throw new NotImplementedException();
        }

        public byte[] BuildRow(T entity)
        {
            throw new NotImplementedException();
        }

        public T ParseRow(byte[] row)
        {
            throw new NotImplementedException();
        }
    }
}
