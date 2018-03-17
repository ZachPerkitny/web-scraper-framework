using System;
using System.Collections.Generic;

namespace FlatFileDB
{
    interface IFlatFile<T>
    {
        /// <summary>
        /// Event raised when the FlatFile is opened
        /// </summary>
        event EventHandler OnOpen;

        /// <summary>
        /// Event raised when the FlatFile is closed
        /// </summary>
        event EventHandler OnClose;

        /// <summary>
        /// Event raised when the FlatFile is persisted
        /// to disk
        /// </summary>
        event EventHandler OnDrain;

        /// <summary>
        /// 
        /// </summary>
        int DrainInterval { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int Size { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Read();

        /// <summary>
        /// 
        /// </summary>
        void Write(T record);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="records"></param>
        void Write(IEnumerable<T> records);

        /// <summary>
        /// 
        /// </summary>
        void Flush();

        /// <summary>
        /// 
        /// </summary>
        void FlushAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ftpUri"></param>
        void UploadWithFTP(string ftpUri);
    }
}
