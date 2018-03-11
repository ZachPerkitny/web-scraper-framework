using System.Text;
using Newtonsoft.Json;

namespace RestFul.Serializer
{
    class JsonSerializer : ISerializer
    {
        public byte[] Serialize(object obj)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
        }
    }
}
