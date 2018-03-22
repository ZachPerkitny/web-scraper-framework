using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperFramework.Utils
{
    internal class DoubleOrderedMap<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey: IComparable
        where TValue: IComparable
    {
        // 0 KEY, 1 VALUE
        private Node[] _rootNode;
        private int _nodeCount = 0;

        public DoubleOrderedMap()
        {
            _rootNode = new Node[2];
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets a collection containing the value in the DoubleOrderedMap
        /// </summary>
        public ICollection<TKey> Keys => throw new NotImplementedException();

        /// <summary>
        /// Gets a collection containing the value in the DoubleOrderedMap
        /// </summary>
        public ICollection<TValue> Values => throw new NotImplementedException();

        /// <summary>
        /// Gets the number of elements
        /// </summary>
        public int Count
        {
            get { return _nodeCount; }
        }

        /// <summary>
        /// Gets a value indicating whether the collection
        /// is read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; } // always writable
        }

        /// <summary>
        /// Adds an element with the provided key and value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds an element with the provided key value pair
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes all items from the DoubleOrderedMap
        /// </summary>
        public void Clear()
        {
            _nodeCount = 0;
            _rootNode = new Node[2];
        }

        /// <summary>
        /// Determines whether the DoubleOrderedMap
        /// contains the specified item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether the DoubleOrderedmap
        /// contains the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Copies the Elements of the DoubleOrderedMap to
        /// any array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through
        /// the collection
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void Insert(TKey key, TValue value)
        {
            Node newNode = new Node(key, value);

            if (_rootNode[0] == null) // if key is null
            {
                newNode.MakeBlack(); // the root node is black
                _rootNode[0] = newNode;
                _rootNode[1] = newNode;
                _nodeCount += 1; // increment node count
                return;
            }

            // find where to insert the node
            Node prev = null;
            Node node = _rootNode[0];
            while (node != null)
            {
                prev = node;
                if (newNode.Value.CompareTo(node.Value) < 0)
                {
                    node = node.GetLeftNode();
                }
                else
                {
                    node = node.GetRightNode();
                }
            }
            
            newNode.SetParent(prev);
            newNode.MakeRed(); // newly inserted nodes are colored red
            if (newNode.Value.CompareTo(prev.Value) < 0)
            {
                prev.SetLeftNode(newNode);
            }
            else
            {
                prev.SetRightNode(newNode);
            }

            // TODO (zvp) Handle Fix Cases
        }

        /// <summary>
        /// 
        /// </summary>
        private void RepairTree()
        {

        }

        /// <summary>
        /// RBT Node
        /// </summary>
        private class Node
        {
            private enum Color
            {
                Red,
                Black
            }

            private Color _color; // each node is either red or black

            public TKey Key { get; private set; }
            public TValue Value { get; private set; }

            private Node[] _left;
            private Node[] _right;
            private Node[] _parent;

            /// <summary>
            /// Construct a new Node with
            /// the given Key and Value
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public Node(TKey key, TValue value)
            {
                Key = key;
                Value = value;

                _left = new Node[2];
                _right = new Node[2];
                _parent = new Node[2];
            }

            /// <summary>
            /// Marks the Node as Red
            /// </summary>
            public void MakeRed()
            {
                _color = Color.Red;
            }

            /// <summary>
            /// Marks the Node as black
            /// </summary>
            public void MakeBlack()
            {
                _color = Color.Black;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public Node GetLeftNode(bool key = true)
            {
                return _left[(key) ? 0 : 1];
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public Node GetRightNode(bool key = true)
            {
                return _right[(key) ? 0 : 1];
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="key"></param>
            public void SetLeftNode(Node left, bool key = true)
            {
                _left[(key) ? 0 : 1] = left;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="right"></param>
            /// <param name="key"></param>
            public void SetRightNode(Node right, bool key = true)
            {
                _right[(key) ? 0 : 1] = right;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="parent"></param>
            /// <param name="key"></param>
            public void SetParent(Node parent, bool key = true)
            {
                _parent[(key) ? 0 : 1] = parent;
            }
        }
    }
}
