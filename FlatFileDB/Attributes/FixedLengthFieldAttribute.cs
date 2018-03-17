using System;

namespace FlatFileDB.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class FixedLengthFieldAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public uint Length { get; private set; }

        public FixedLengthFieldAttribute(uint length)
        {
            Length = length;
        }
    }
}
