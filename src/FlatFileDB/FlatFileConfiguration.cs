using System;
using System.Text;

namespace FlatFileDB
{
    public class FlatFileConfiguration
    {
        /// <summary>
        /// Indicates whether or not the buffer should
        /// be flushed to disk if the internal memory 
        /// buffer has exceeded over a constant limit.
        /// </summary>
        public bool AutoFlush { get; set; }

        /// <summary>
        /// Indicates whether the flush timer should
        /// be enabled on initialization
        /// </summary>
        public bool AutoStartTimer { get; set; }

        /// <summary>
        /// Specifies the character encoding to
        /// use.
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// File to flush to.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Interval at which the contents of the
        /// memory buffer should be flushed to disk.
        /// </summary>
        public int FlushInterval { get; set; }

        /// <summary>
        /// Character used to seperate table rows
        /// </summary>
        public string RowSeparator { get; set; } = Environment.NewLine;
    }
}
