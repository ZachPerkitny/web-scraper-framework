﻿using System.Reflection;
using FlatFileDB.Serializer;

namespace FlatFileDB.Columns
{
    internal interface IColumn
    {
        /// <summary>
        /// 
        /// </summary>
        FieldInfo Field { get; }

        /// <summary>
        /// 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        ISerializer Serializer { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        object Deserialize(byte[] col);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        byte[] Serialize(object obj);
    }
}
