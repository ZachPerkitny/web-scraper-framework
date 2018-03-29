using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
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

        private FileStream _bufferStream;
        private long _bufferPos;

        private Timer _flushTimer;

        private readonly byte[] _rowSeparator;

        private bool _disposed = false;

        public FlatFileEngine(FlatFileConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _buffer = new MemoryBuffer();
            _table = Table<T>.Create();
            _bufferStream = new FileStream(_config.FileName,
                FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _bufferPos = _bufferStream.Length;
            _rowSeparator = _config.Encoding.GetBytes(_config.RowSeparator);
            
            if (_config.FlushInterval > 0)
            {
                _flushTimer = new Timer
                {
                    AutoReset = true,
                    Enabled = _config.AutoStartTimer,
                    Interval = _config.FlushInterval
                };
                _flushTimer.Elapsed += OnTimerCallback;
            }
        }

        public IEnumerable<T> Read()
        {
            byte[] data = new byte[_bufferStream.Length];
            int toRead = (int)_bufferStream.Length;
            int read = 0;

            while (toRead > 0)
            {
                int n = _bufferStream.Read(data, read, toRead);

                if (n == 0)
                {
                    break;
                }

                toRead -= n;
                read += n;
            }

            string[] rows = _config.Encoding.GetString(data)
                .Split(new string [] { _config.RowSeparator  }, StringSplitOptions.None);

            // TODO(zvp): What about the header ?
            List<T> entities = new List<T>();
            for (int i = 1; i < rows.Length; i++)
            {
                entities.Add(_table.ParseRow(rows[i]));
            }


            return entities;
        }

        public Task<IEnumerable<T>> ReadAsync()
        {
            throw new NotImplementedException();
        }

        public void Write(T record)
        {
            WriteHeader();
            WriteRecord(record);

            if (_config.AutoFlush)
            {
                Flush();
            }
        }

        public void Write(IEnumerable<T> records)
        {
            WriteHeader();

            foreach (T record in records)
            {
                WriteRecord(record);
            }

            if (_config.AutoFlush)
            {
                Flush();
            }
        }

        public async Task WriteAsync(T record)
        {
            WriteHeader();
            WriteRecord(record);

            if (_config.AutoFlush)
            {
                await FlushAsync();
            }
        }

        public async Task WriteAsync(IEnumerable<T> records)
        {
            WriteHeader();

            foreach (T record in records)
            {
                WriteRecord(record);
            }

            if (_config.AutoFlush)
            {
                await FlushAsync();
            }
        }

        public void Flush()
        {
            // write to file synchronously
            byte[] data = _buffer.Read();
            _bufferStream.Position = _bufferPos;
            _bufferStream.Write(data, 0, data.Length);
            _bufferStream.Flush();

            // increment position
            _bufferPos += data.Length;

            // clear memory buffer
            FlushBuffer();
        }

        public async Task FlushAsync()
        {
            // write to file
            byte[] data = _buffer.Read();
            _bufferStream.Position = _bufferPos;
            await _bufferStream.WriteAsync(data, 0, data.Length);
            await _bufferStream.FlushAsync();

            // increment position
            _bufferPos += data.Length;

            // clear memory buffer
            FlushBuffer();
        }

        public void StartFlushTimer()
        {
            _flushTimer.Start();
        }

        public void StopFlushTimer()
        {
            _flushTimer.Stop();
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

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _bufferStream.Close();
                    _bufferStream.Dispose();
                    _bufferStream = null;
                }

                _disposed = true;
            }
        }

        private void WriteRecord(T record)
        {
            byte[] serializedRecord = _config.Encoding.GetBytes(
                _table.BuildRow(record));
            _buffer.Write(serializedRecord);
            _buffer.Write(_rowSeparator);
        }

        private void WriteHeader()
        {
            // clean slate, just write the header
            if (_table.IncludeHeader && _buffer.Count == 0 &&
                _bufferPos == 0)
            {
                byte[] header = _config.Encoding.GetBytes(
                    _table.BuildHeader());
                _buffer.Write(header);
                _buffer.Write(_rowSeparator);
            }
        }

        private void FlushBuffer()
        {
            // clear memory buffer
            _buffer.Clear();
            _bufferPos = 0;
        }

        private async void OnTimerCallback(object sender, ElapsedEventArgs e)
        {
            await FlushAsync();
        }
    }
}
