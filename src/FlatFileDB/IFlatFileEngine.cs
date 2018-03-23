using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace FlatFileDB
{
    public interface IFlatFileEngine<T>
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
        event EventHandler OnFlush;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Read();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> ReadAsync();

        /// <summary>
        /// Writes the records to an internal
        /// memory buffer.
        /// </summary>
        /// <param name="records"></param>
        void Write(IEnumerable<T> records);

        /// <summary>
        /// Flushes the contents of the memory buffer
        /// to the file name specified in the configuration
        /// object.
        /// </summary>
        void Flush();

        /// <summary>
        /// Flushes the contents of the memory buffer
        /// to the specified file name.
        /// </summary>
        /// <param name="fileName"></param>
        void Flush(string fileName);

        /// <summary>
        /// Flushes the contents of the memory buffer
        /// asynchronously to the file name specified
        /// in the configuration object.
        /// </summary>
        Task FlushAsync();

        /// <summary>
        /// Flushes the contents of the memory buffer
        /// asynchronously to the specified file name.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task FlushAsync(string fileName);

        /// <summary>
        /// Uploads the flat file to the specified
        /// FTP Uri using the credentials provided
        /// </summary>
        /// <param name="ftpUri"></param>
        /// <param name="networkCredentials"></param>
        Task UploadWithFTP(string ftpUri, NetworkCredential networkCredentials);
    }
}
