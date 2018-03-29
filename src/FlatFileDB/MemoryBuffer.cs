using System;

namespace FlatFileDB
{
    internal class MemoryBuffer
    {
        private const int INITIAL_CAPACITY = 128;

        private byte[] _buffer;
        private int _capacity;
        private int _count;
        private int _initialCapacity;

        private object _locker = new object();

        /// <summary>
        /// 
        /// </summary>
        public MemoryBuffer()
            : this(INITIAL_CAPACITY) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialCapacity"></param>
        public MemoryBuffer(int initialCapacity)
        {
            _count = 0;
            _initialCapacity = (initialCapacity > INITIAL_CAPACITY) ? initialCapacity : INITIAL_CAPACITY;
            _capacity = _initialCapacity;
            _buffer = new byte[_initialCapacity];
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                lock (_locker)
                {
                    return _count;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Capacity
        {
            get
            {
                lock (_locker)
                {
                    return _capacity;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public int this[int i]
        {
            get
            {
                lock (_locker)
                {
                    if (i < 0 || i >= _count)
                    {
                        throw new ArgumentOutOfRangeException(nameof(i));
                    }

                    return _buffer[i];
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void Write(byte[] data)
        {
            lock (_locker)
            {
                // realloc and expand buffer if needed
                int ns = _count + data.Length;
                if (ns >= _capacity)
                {
                    ExpandCapacity(ns);
                }

                // copy data buffer into internal buffer
                Buffer.BlockCopy(data, 0, _buffer, _count, data.Length);
                _count += data.Length;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] Read()
        {
            lock (_locker)
            {
                // deep copy buffer
                byte[] copy = new byte[_count];
                Buffer.BlockCopy(_buffer, 0, copy, 0, _count);

                return copy;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            lock (_locker)
            {
                // reset count
                _count = 0;
                _capacity = _initialCapacity;

                // create new buffer with initial capacity
                _buffer = new byte[_initialCapacity];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExpandCapacity(int min)
        {
            // double the array capacity until
            // it's greater than the minimum
            // required capacity
            while ((_capacity *= 2) < min)
                ;

            // create new buffer with new capacity
            // and deep copy contents of the old
            // buffer
            byte[] newBuffer = new byte[_capacity];
            Buffer.BlockCopy(_buffer, 0, newBuffer, 0, _count);
            _buffer = newBuffer;
        }
    }
}
