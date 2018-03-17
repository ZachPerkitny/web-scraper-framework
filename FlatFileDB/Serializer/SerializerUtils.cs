using System;

namespace FlatFileDB.Serializer
{
    static class SerializerUtils
    {
        public static ISerializer GetSerializerForType(Type type)
        {
            if (type == typeof(bool))
            {
                return new BoolSerializer();
            }
            else if (type == typeof(int))
            {
                return new IntSerializer();
            }
        }
    }
}
