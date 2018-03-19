namespace FlatFileDB.Serializer
{
    interface ISerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        object Deserialize(byte[] buffer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        byte[] Serialize(object obj);
    }
}
