using System;
using System.Collections;
using System.Collections.Generic;

namespace ScraperFramework.Utils
{
    /// <summary>
    /// Red Black Tree Implementation of a Map. Based off of 
    /// Java's DoubleOrderedMap, hence the name.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
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
        public TValue this[TKey key]
        {
            get
            {
                Node node = FindByKey(key);
                return (node == null) ? default(TValue) : node.Value;
            }
            set => throw new NotImplementedException();
        }

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
            Insert(key, value);
        }

        /// <summary>
        /// Adds an element with the provided key value pair
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Insert(item.Key, item.Value);
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
            Node node = FindByKey(item.Key);
            return node != null && item.Value.CompareTo(node.Value) == 0;
        }

        /// <summary>
        /// Determines whether the DoubleOrderedmap
        /// contains the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return FindByKey(key) != null;
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
            return new Enumerator(this);
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
                // set prev node
                prev = node;

                int cmp = newNode.Key.CompareTo(node.Key);
                if (cmp < 0)
                {
                    node = node.GetLeftNode();
                }
                else if (cmp > 0)
                {
                    node = node.GetRightNode();
                }
                else
                {
                    // todo (zvp): proper error
                    throw new Exception();
                }
            }

           // InsertValue(newNode);
            newNode.SetParent(prev);
            newNode.MakeRed(); // newly inserted nodes are colored red
            if (newNode.Key.CompareTo(prev.Key) < 0)
            {
                prev.SetLeftNode(newNode);
            }
            else
            {
                prev.SetRightNode(newNode);
            }

            RepairTree(newNode);
            _nodeCount += 1; // increment node count
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newNode"></param>
        private void InsertValue(Node newNode)
        {
            Node prev = null;
            Node node = _rootNode[1]; // value node

            while (node != null)
            {
                prev = node;

                int cmp = newNode.Value.CompareTo(node.Value);
                if (cmp < 0)
                {
                    node = node.GetLeftNode(false);
                }
                else if (cmp > 0)
                {
                    node = node.GetRightNode(false);
                }
                else
                {
                    // todo (zvp): proper error
                    throw new Exception();
                }
            }
            
            newNode.SetParent(prev, false);
            newNode.MakeRed(false); // newly inserted nodes are colored red
            if (newNode.Value.CompareTo(prev.Value) < 0)
            {
                prev.SetLeftNode(newNode, false);
            }
            else
            {
                prev.SetRightNode(newNode, false);
            }

            RepairTree(newNode, false);
        }

        /// <summary>
        /// 
        /// </summary>
        private void RepairTree(Node node, bool key = true)
        {
            // case 1 : repaint root node
            if (node.GetParent(key) == null)
            {
                node.MakeBlack(key);
                return;
            }

            // case 2 : do nothing
            if (node.GetParent(key).IsBlack())
            {
                return;
            }
            
            // if uncle is red, repaint
            if (node.GetUncle(key) != null
                && node.GetUncle(key).IsRed(key))
            {
                node.GetParent(key).MakeBlack(key);
                node.GetUncle(key).MakeBlack(key);
                node.GetGrandParent(key).MakeRed(key);
                // grandparent may now violate property(2)
                // root must be a black node
                RepairTree(node.GetGrandParent(key), key);
            }
            else
            {
                Node grandParent = node.GetGrandParent(key);
                Node parent = node.GetParent(key);
                
                if (grandParent.GetLeftNode(key) != null &&
                    grandParent.GetLeftNode(key).GetRightNode(key) == node)
                {
                    RotateLeft(parent, key);
                    parent = node;
                    node = node.GetLeftNode(key);
                    grandParent = parent.GetParent(key);
                }
                else if (grandParent.GetRightNode(key) != null &&
                    grandParent.GetRightNode(key).GetLeftNode(key) == node)
                {
                    RotateRight(parent, key);
                    parent = node;
                    node = node.GetRightNode(key);
                    grandParent = parent.GetParent(key);
                }

                // if node is left-left
                if (node == parent.GetLeftNode(key))
                {
                    RotateRight(grandParent, key);
                }
                // right-right
                else
                {
                    RotateLeft(grandParent, key);
                }

                // swap colors
                parent.MakeBlack(key);
                grandParent.MakeRed(key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RotateLeft(Node node, bool key = true)
        {
            Node np = node.GetRightNode(key);

            // set np's left node as node's right node
            node.SetRightNode(np.GetLeftNode(key), key);

            // update np left's parent node if
            // it is not null
            if (np.GetLeftNode(key) != null)
            {
                np.GetLeftNode(key).SetParent(node, key);
            }

            np.SetParent(node.GetParent(key));

            // if node was root, replace
            // it with np
            if (node.GetParent(key) == null)
            {
                _rootNode[(key) ? 0 : 1] = np;
            }
            // if node was a left node, replace the parent's
            // left node with np
            else if (node.GetParent(key).GetLeftNode(key) == node)
            {
                node.GetParent(key).SetLeftNode(np);
            }
            // if node was a right node, replace the parent's
            // right node with np
            else
            {
                node.GetParent(key).SetRightNode(np);
            }

            // put node as np's left node
            np.SetLeftNode(node);
            // update node's parent node to np
            node.SetParent(np);
        }

        /// <summary>
        /// 
        /// </summary>
        private void RotateRight(Node node, bool key = true)
        {
            Node np = node.GetLeftNode(key);

            // set np's right node as node's left node
            node.SetLeftNode(np.GetRightNode(key), key);

            // update np right's parent node if
            // it is not null
            if (np.GetRightNode(key) != null)
            {
                np.GetRightNode(key).SetParent(node, key);
            }

            np.SetParent(node.GetParent(key));

            // if node was root, replace
            // it with np
            if (node.GetParent(key) == null)
            {
                _rootNode[(key) ? 0 : 1] = np;
            }
            // if node was a left node, replace the parent's
            // left node with np
            else if (node.GetParent(key).GetLeftNode(key) == node)
            {
                node.GetParent(key).SetLeftNode(np);
            }
            // if node was a right node, replace the parent's
            // right node with np
            else
            {
                node.GetParent(key).SetRightNode(np);
            }

            // put node as np's right node
            np.SetRightNode(node);
            // update node's parent node to np
            node.SetParent(np);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Node FindByKey(TKey key)
        {
            Node node = _rootNode[0];
            while (node != null)
            {
                int cmp = node.Key.CompareTo(key);
                if (cmp == 0)
                {
                    return node;
                }
                else if (cmp < 0)
                {
                    node = node.GetLeftNode();
                }
                else // cmp > 0
                {
                    node = node.GetRightNode();
                }
            }

            return node;
        }

        /// <summary>
        /// RBT Node
        /// </summary>
        private class Node
        {
            private enum Color
            {
                Black,
                Red
            }

            public TKey Key { get; private set; }

            public TValue Value { get; private set; }

            private Color[] _color; // each node is either red or black

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

                _color = new Color[2];

                _left = new Node[2];
                _right = new Node[2];
                _parent = new Node[2];
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public bool IsRed(bool key = true)
            {
                return _color[(key) ? 0 : 1] == Color.Red;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public bool IsBlack(bool key = true)
            {
                return _color[(key) ? 0 : 1] == Color.Black;
            }

            /// <summary>
            /// Marks the Node as Red
            /// </summary>
            public void MakeRed(bool key = true)
            {
                _color[(key) ? 0 : 1] = Color.Red;
            }

            /// <summary>
            /// Marks the Node as black
            /// </summary>
            public void MakeBlack(bool key = true)
            {
                _color[(key) ? 0 : 1] = Color.Black;
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
            /// <param name="key"></param>
            public Node GetParent(bool key = true)
            {
                return _parent[(key) ? 0 : 1];
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public Node GetSibling(bool key = true)
            {
                Node parent = GetParent(key);
                if (parent == null)
                {
                    return null;
                }

                if (this == parent.GetLeftNode(key))
                {
                    return parent.GetRightNode(key);
                }
                else
                {
                    return parent.GetLeftNode(key);
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            public Node GetGrandParent(bool key = true)
            {
                Node parent = GetParent(key);
                if (parent == null)
                {
                    return null;
                }

                return parent.GetParent(key);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public Node GetUncle(bool key = true)
            {
                if (GetGrandParent(key) == null)
                {
                    return null;
                }

                Node parent = GetParent(key);
                if (parent == null)
                {
                    return null;
                }

                return parent.GetSibling(key);
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

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly DoubleOrderedMap<TKey, TValue> _doubleOrderedMap;
            private Node _node;
            private KeyValuePair<TKey, TValue> _current;
            private Stack<Node> _stack;

            internal Enumerator(DoubleOrderedMap<TKey, TValue> doubleOrderedMap)
            {
                _doubleOrderedMap = doubleOrderedMap;
                _current = new KeyValuePair<TKey, TValue>();
                _node = _doubleOrderedMap._rootNode[0];
                _stack = new Stack<Node>();
            }

            public KeyValuePair<TKey, TValue> Current
            {
                get { return _current; }
            }

            object IEnumerator.Current => throw new NotImplementedException();

            public bool MoveNext()
            {
                while (true)
                {
                    if (_node != null)
                    {
                        _stack.Push(_node);
                        _node = _node.GetLeftNode();
                    }
                    else
                    {
                        if (_stack.Count == 0)
                        {
                            return false;
                        }

                        Node node = _stack.Pop();
                        _current = new KeyValuePair<TKey, TValue>(node.Key, node.Value);
                        _node = node.GetRightNode();
                        return true;
                    }
                }
            }

            public void Reset()
            {
                _node = _doubleOrderedMap._rootNode[0];
                _stack = new Stack<Node>();
                _current = new KeyValuePair<TKey, TValue>();
            }

            public void Dispose() { }
        }
    }
}
