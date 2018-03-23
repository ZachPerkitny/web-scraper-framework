using System;

namespace FlatFileDB.Serializer
{
    public class IntSerializer : ISerializer
    {
        public object Deserialize(string data)
        {
            try
            {
                return int.Parse(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Serialize(object entity)
        {
            return entity.ToString();
        }
    }
}
