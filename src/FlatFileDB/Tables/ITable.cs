using System.Collections.Generic;
using FlatFileDB.Columns;

namespace FlatFileDB.Tables
{
    internal interface ITable<T>
    {
        /// <summary>
        /// Gets the table's columns
        /// </summary>
        IEnumerable<IColumn> Columns { get; }

        /// <summary>
        /// Returns the number of columns in 
        /// this table
        /// </summary>
        int ColumnCount { get; }

        /// <summary>
        /// Returns a list of the column
        /// names
        /// </summary>
        IEnumerable<string> ColumnNames { get; }

        /// <summary>
        /// Gets the value used to indicate whether
        /// the table includes a header
        /// </summary>
        bool IncludeHeader { get; set; }

        /// <summary>
        /// Indicates whether the table is delimited
        /// </summary>
        bool IsDelimitedTable { get; }

        /// <summary>
        /// Indicates whether the table is fixed
        /// </summary>
        bool IsFixedTable { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        byte[] BuildHeader();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        byte[] BuildRow(T entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        T ParseRow(byte[] row);
    }
}
