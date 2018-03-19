using System;

namespace FlatFileDB.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public abstract class TableAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public bool UseHeader { get; set; }
    }
}
