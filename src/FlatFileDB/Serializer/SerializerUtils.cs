using System;

namespace FlatFileDB.Serializer
{
    static class SerializerUtils
    {
        public static ISerializer GetSerializerForType(Type type)
        {
            if (type == typeof(int))
            {
                return new IntSerializer();
            }
            else
            {
                return null;
            }
        }
    }
}