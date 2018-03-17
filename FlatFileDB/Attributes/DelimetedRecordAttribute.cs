using System;

namespace FlatFileDB.Attributes
{
    /// <summary>
    /// Defines a delimeter for the class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DelimetedRecordAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Delimeter { get; private set; }

        public DelimetedRecordAttribute(string delimeter)
        {
            if (!string.IsNullOrEmpty(delimeter))
            {
                Delimeter = delimeter;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
