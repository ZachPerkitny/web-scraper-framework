using System;

namespace FlatFileDB.Serializer
{
    public class IntSerializer : ISerializer
    {
        public object DeserializeField(string value)
        {
            try
            {
                return int.Parse(value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string SerializeField(object value)
        {
            return value.ToString();
        }
    }
}
