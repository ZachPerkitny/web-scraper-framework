using System;
using System.IO;
using ProtoBuf;

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
                // Setup Serializers using Protobuf-net
                DBreeze.Utils.CustomSerializator.ByteArraySerializator = (object obj) =>
                {
                    byte[] serialized = null;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        Serializer.NonGeneric.Serialize(ms, obj);
                        serialized = ms.ToArray();
                    }

                    return serialized;
                };

                DBreeze.Utils.CustomSerializator.ByteArrayDeSerializator = (byte[] data, Type type) =>
                {
                    object ret = null;

                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        ret = Serializer.NonGeneric.Deserialize(type, ms);
                    }

                    return ret;
                };

                _initialized = true;
            }
        }
    }
}
