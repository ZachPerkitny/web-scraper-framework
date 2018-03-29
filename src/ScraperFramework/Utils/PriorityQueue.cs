using System;
using System.Collections;

namespace ScraperFramework.Utils
{
    internal class PriorityQueue<T> : ICollection
        where T: IComparable<T>
    {
        private const int INITIAL_CAPACITY = 10;

        private T[] _heap;

        private int _initialCapacity;
        private int _capacity;
        private int _count;

        private object _sync;

        /// <summary>
        /// 
        /// </summary>
        public PriorityQueue()
            : this(INITIAL_CAPACITY) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialCapacity"></param>
        public PriorityQueue(int initialCapacity)
        {
            _count = 0;
            _initialCapacity = (initialCapacity > INITIAL_CAPACITY) ? initialCapacity : INITIAL_CAPACITY;
            _capacity = _initialCapacity;
            _heap = new T[_initialCapacity];
        }

        /// <summary>
        /// Gets the number of elements
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        public bool IsEmpty
        {
            get { return _count == 0; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Gets an object used to synchronize access to 
        /// the collection
        /// </summary>
        public object SyncRoot
        {
            get
            {
                if (_sync == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _sync, new object(), null);
                }

                return _sync;
            }
        }

        /// <summary>
        /// Enqueues an item
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(T item)
        {
            if (_count >= _capacity)
            {
                ExpandCapacity();
            }

            int i = _count;
            int p;
            _heap[_count++] = item;
            // while node is less than the parent
            // or the node has become the new root
            while (i > 0 && item.CompareTo(_heap[(p = GetParentIndex(i))]) < 0)
            {
                T temp = _heap[i];
                _heap[i] = _heap[p];
                _heap[p] = temp;
                i = p;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            // store root
            T item = _heap[0];

            int i = 0;
            _heap[0] = _heap[--_count];
            while (true)
            {
                int lc = GetLeftChildIndex(i);
                int rc = GetRightChildIndex(i);
                int si = i;

                // swap with smallest child node
                if (_heap[si].CompareTo(_heap[lc]) < 0)
                {
                    si = lc;
                }

                if (_heap[si].CompareTo(_heap[rc]) < 0)
                {
                    si = rc;
                }

                if (si == i)
                {
                    break;
                }

                T temp = _heap[i];
                _heap[i] = _heap[si];
                _heap[si] = temp;
                i = si;
            }

            return item;
        }

        /// <summary>
        /// Removes all items from the Priority Queue
        /// </summary>
        public void Clear()
        {
            // reset count
            _count = 0;
            _capacity = _initialCapacity;

            // clear heap
            _heap = new T[_initialCapacity];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        private int GetLeftChildIndex(int i)
        {
            return 2 * i + 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int GetRightChildIndex(int i)
        {
            return 2 * i + 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int GetParentIndex(int i)
        {
            return (int)Math.Floor((decimal)(i - 1) / 2);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExpandCapacity()
        {
            // double capacity
            _capacity *= 2;

            // create new heap with new capacity
            // and deep copy contents of the old
            // buffer
            T[] newHeap = new T[_capacity];
            Array.Copy(_heap, newHeap, _count);
            _heap = newHeap;
        }
    }
}
