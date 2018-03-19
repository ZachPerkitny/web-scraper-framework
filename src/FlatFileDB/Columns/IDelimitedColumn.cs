namespace FlatFileDB.Columns
{
    internal interface IDelimitedColumn : IColumn
    {
        /// <summary>
        /// 
        /// </summary>
        string Delimiter { get; }
    }
}
