using System;
using System.Text;
using Newtonsoft.Json;

namespace ScraperFramework.Data
{
    internal static class DBreezeInitialization
    {
        /// <summary>
        /// Indicates whether DBreeze has been configured.
        /// </summary>
        private static bool _initialized = false;

        /// <summary>
        /// Sets up the Dbreeze ByteArraySerializer and DeSerializer
        /// </summary>
        public static void SetupUtils()
        {
            if (!_initialized)
            {
                // Setup Serializers using Newtonsoft
                // TODO (zvp): Maybe use something more compact ?
                DBreeze.Utils.CustomSerializator.ByteArraySerializator = (object obj) =>
                {
                    return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
                };

                DBreeze.Utils.CustomSerializator.ByteArrayDeSerializator = (byte[] buffer, Type type) =>
                {
                    string value = Encoding.UTF8.GetString(buffer);
                    return JsonConvert.DeserializeObject(value, type);
                };

                _initialized = true;
            }
        }
    }
}
