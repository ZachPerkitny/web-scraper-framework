namespace FlatFileDB.Columns
{
    internal interface IFixedColumn : IColumn
    {
        /// <summary>
        /// 
        /// </summary>
        int Length { get; }
    }
}
