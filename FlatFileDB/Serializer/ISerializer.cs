namespace FlatFileDB.Serializer
{
    public interface ISerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object DeserializeField(string value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string SerializeField(object value);
    }
}
