namespace FlatFileDB.Serializer
{
    public interface ISerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        object Deserialize(string data);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        string Serialize(object entity);
    }
}
