using System.Text;
using Newtonsoft.Json;

namespace RestFul.Serializer
{
    class JsonSerializer : ISerializer
    {
        public T Deserialize<T>(string value) where T : class
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public byte[] Serialize(object obj)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
        }
    }
}
