using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FlatFileDB.Tables;

namespace FlatFileDB
{
    public class FlatFileEngine<T> : IFlatFileEngine<T>
    {
        public event EventHandler OnOpen;
        public event EventHandler OnClose;
        public event EventHandler OnFlush;

        private readonly FlatFileConfiguration _config;
        private readonly MemoryBuffer _buffer;
        private readonly Table<T> _table;

        public FlatFileEngine(FlatFileConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _buffer = new MemoryBuffer();
            _table = Table<T>.Create();
        }

        public IEnumerable<T> Read()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ReadAsync()
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<T> records)
        {
            if (_table.IncludeHeader)
            {
                byte[] header = _config.Encoding.GetBytes(
                    _table.BuildHeader());
                _buffer.Write(header);
            }

            foreach (T record in records)
            {
                Write(record);
            }
        }

        public void Flush()
        {
            Flush(_config.FileName);
        }

        public void Flush(string fileName)
        {
            string buffer = _config.Encoding.GetString(_buffer.Read());

            // write to file synchronously
            StreamWriter sw = new StreamWriter(fileName);
            sw.Write(buffer);
            sw.Close();

            // clear memory buffer
            _buffer.Clear();
        }

        public Task FlushAsync()
        {
            return FlushAsync(_config.FileName);
        }

        public async Task FlushAsync(string fileName)
        {
            string buffer = _config.Encoding.GetString(_buffer.Read());

            // write to file
            StreamWriter sw = new StreamWriter(fileName);
            await sw.WriteAsync(buffer);
            sw.Close();

            // clear memory buffer
            _buffer.Clear();
        }

        public async Task UploadWithFTP(string ftpUri, NetworkCredential networkCredentials)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUri);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // set credentials
            request.Credentials = networkCredentials;

            // read contents of flat file for uploading
            byte[] flatFileContents = null;
            using (StreamReader sr = new StreamReader(_config.FileName))
            {
                flatFileContents = _config.Encoding.GetBytes(
                    await sr.ReadToEndAsync());
            }

            // send flat file to ftp server
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(flatFileContents, 0, flatFileContents.Length);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        protected void Write(T record)
        {
            byte[] serializedRecord = _config.Encoding.GetBytes(
                _table.BuildRow(record));

            _buffer.Write(serializedRecord);
        }
    }
}
