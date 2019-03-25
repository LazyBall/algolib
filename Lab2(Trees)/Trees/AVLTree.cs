using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Trees
{
    //source: https://habr.com/ru/post/150732/
    public class AVLTree<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey : IComparable<TKey>
    {

        private class Node<NTKey, NTValue>
        {
            public NTKey Key { get; set; }
            public NTValue Value { get; set; }
            public Node<NTKey, NTValue> Left { get; set; }
            public Node<NTKey, NTValue> Right { get; set; }
            public Node<NTKey, NTValue> Parent { get; set; }
            public byte Height { get; set; }

            public Node(NTKey key, NTValue value, Node<NTKey, NTValue> left = null,
                Node<NTKey, NTValue> right = null, Node<NTKey, NTValue> parent = null, byte height = 1)
            {
                this.Key = key;
                this.Value = value;
                this.Left = left;
                this.Right = right;
                this.Parent = parent;
                this.Height = height;
            }

        }

        private Node<TKey, TValue> Root { get; set; }

        public int Count { get; private set; }

        #region Add, FindNode, RemoveNode

        public void Add(TKey key, TValue value)
        {
            CheckKey(key);
            var current = Root;
            Node<TKey, TValue> parentCurrent = null;

            while (current != null)
            {
                int comparisonResult = current.Key.CompareTo(key);
                Node<TKey, TValue> next = null;
                if (comparisonResult > 0)
                {
                    next = current.Left;
                }
                else if (comparisonResult < 0)
                {
                    next = current.Right;
                }
                else
                {
                    throw new ArgumentException
                     ("An element with the same key already exists in the AVLTree<TKey, TValue>.");
                }
                parentCurrent = current;
                current = next;
            }
            current = new Node<TKey, TValue>(key, value, parent: parentCurrent);
            if (parentCurrent != null)
            {
                if (parentCurrent.Key.CompareTo(current.Key) > 0) parentCurrent.Left = current;
                else parentCurrent.Right = current;
                Root = BalanceToRoot(parentCurrent);
            }
            else
            {
                Root = current;
            }
            Count++;
        }

        private Node<TKey, TValue> FindNode(TKey key)
        {
            CheckKey(key);
            var current = Root;

            while (current != null)
            {
                int comparisonResult = current.Key.CompareTo(key);
                if (comparisonResult > 0)
                {
                    current = current.Left;
                }
                else if (comparisonResult < 0)
                {
                    current = current.Right;
                }
                else
                {
                    return current;
                }
            }

            return null;
        }

        private bool RemoveNode(Node<TKey, TValue> node)
        {
            var current = node;
            if (current == null)
            {
                return false;
            }
            else
            {
                Node<TKey, TValue> balanceFrom = null;

                if (current.Left == null || current.Right == null)
                {
                    var replacement = current.Left ?? current.Right;
                    if (replacement != null) replacement.Parent = current.Parent;
                    if (current.Parent != null)
                    {
                        if (current == current.Parent.Right) current.Parent.Right = replacement;
                        else current.Parent.Left = replacement;
                    }
                    balanceFrom = current.Parent;
                }
                else
                {
                    var successor = current.Right;
                    Node<TKey, TValue> parentSuccessor = null;

                    while (successor.Left != null)
                    {
                        parentSuccessor = successor;
                        successor = successor.Left;
                    }

                    if (parentSuccessor != null)
                    {
                        parentSuccessor.Left = successor.Right;
                        if (successor.Right != null) successor.Right.Parent = parentSuccessor;
                    }
                    else
                    {
                        current.Right = successor.Right;
                        if (successor.Right != null) successor.Right.Parent = current;
                    }

                    current.Key = successor.Key;
                    balanceFrom = parentSuccessor ?? current;
                }

                Root = BalanceToRoot(balanceFrom);
            }
            Count--;
            return true;
        }

        #endregion

        public int Height => GetNodeHeight(Root);

        public ICollection<TKey> Keys => new List<TKey>(from elem in this select elem.Key);

        public ICollection<TValue> Values => new List<TValue>(from elem in this select elem.Value);

        public bool IsReadOnly => false;

        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out TValue value)) return value;
                else throw new KeyNotFoundException
                        ("The property is retrieved and key does not exist in the collection.");
            }
            set
            {
                var node = FindNode(key);
                if (node == null) throw new KeyNotFoundException
                        ("The property is retrieved and key does not exist in the collection.");
                else node.Value = value;
            }
        }

        private int GetNodeHeight(Node<TKey, TValue> node)
        {
            return (node != null) ? node.Height : 0;
        }

        private int GetBalancingFactor(Node<TKey, TValue> node)
        {
            return (node != null) ? (GetNodeHeight(node.Right) - GetNodeHeight(node.Left)) : 0;
        }

        private void FixNodeHeight(Node<TKey, TValue> node)
        {
            if (node != null)
            {
                node.Height = (byte)(Math.Max(GetNodeHeight(node.Left), GetNodeHeight(node.Right)) + 1);
            }
        }

        private void CheckKey(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key is null.");
            }
        }      

        private Node<TKey, TValue> RotateRight(Node<TKey, TValue> node)
        {
            var q = node.Left;
            var b = q.Right;
            node.Left = b;
            if (b != null) b.Parent = node;
            q.Right = node;
            node.Parent = q;
            FixNodeHeight(node);
            FixNodeHeight(q);
            return q;
        }

        private Node<TKey, TValue> RotateLeft(Node<TKey, TValue> node)
        {
            var p = node.Right;
            var b = p.Left;
            node.Right = b;
            if (b != null) b.Parent = node;
            p.Left = node;
            node.Parent = p;            
            FixNodeHeight(node);
            FixNodeHeight(p);
            return p;
        }

        private Node<TKey, TValue> Balance(Node<TKey, TValue> node)
        {
            FixNodeHeight(node);
            var balanceFactor = GetBalancingFactor(node);
            if (balanceFactor == 2)
            {
                if (GetBalancingFactor(node.Right) < 0)
                {
                    node.Right = RotateRight(node.Right);
                    node.Right.Parent = node;
                }
                return RotateLeft(node);
            }
            else if (balanceFactor == -2)
            {
                if (GetBalancingFactor(node.Left) > 0)
                {
                    node.Left = RotateLeft(node.Left);
                    node.Left.Parent = node;
                }
                return RotateRight(node);
            }
            return node;
        }

        private Node<TKey, TValue> BalanceToRoot(Node<TKey, TValue> node)
        {
            if (node == null) return node;
            var current = node;

            while (current.Parent != null)
            {
                var parent = current.Parent;               
                bool right = parent.Right == current;
                int oldHeight = current.Height;
                current = Balance(current);                
                current.Parent = parent;
                if (right) parent.Right = current;
                else parent.Left = current;
                if (oldHeight == current.Height) return Root;
                current = parent;                
            }

            current = Balance(current);
            current.Parent = null;
            return current;
        }     

        public bool Remove(TKey key)
        {
            return RemoveNode(FindNode(key));
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var current = FindNode(item.Key);
            bool found = true;
            if (current != null)
            {
                if (item.Value is IComparable<TValue> compValue)
                {
                    found = (compValue.CompareTo(current.Value) == 0);
                }
                else
                {
                    found = item.Value.Equals(current.Value);
                }
            }
            if (found) found = RemoveNode(current);
            return found;
        }

        public bool ContainsKey(TKey key)
        {
            return FindNode(key) != null;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var node = FindNode(key);
            if (node != null)
            {
                value = node.Value;
                return true;
            }
            else
            {
                value = default(TValue);
                return false;
            }            
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            Count = 0;
            Root = null;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var node = FindNode(item.Key);
            if (node == null)
            {
                return false;
            }
            else
            {
                if (item.Value is IComparable<TValue> compValue)
                {
                    return (compValue.CompareTo(node.Value) == 0);
                }
                else
                {
                    return item.Value.Equals(node.Value);
                }
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array is null.");
            }
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex is less than 0.");
            }
            if (array.Length < arrayIndex + this.Count)
            {
                throw new ArgumentException("The number of elements in the source " +
                        "AVLTree<TKey,TValue> " +
                        "is greater than the available space from index to the end of the " +
                        "destination array.");
            }

            int i = arrayIndex;

            foreach (var elem in this)
            {
                array[i] = elem;
                i++;
            }
        }

        private IEnumerable<KeyValuePair<TKey, TValue>> DoInorderTraversal(Node<TKey, TValue> node)
        {
            Stack<Node<TKey, TValue>> stack = new Stack<Node<TKey, TValue>>(Count);
            var current = node;

            while (current != null || stack.Count > 0)
            {
                
                while (current != null)
                {
                    stack.Push(current);
                    current = current.Left;
                }

                current = stack.Pop();
                Console.WriteLine(current.Key);
                yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
                current = current.Right;
            }
        }

        // Function to print inorder traversal using parent pointer 
        // source https://www.geeksforgeeks.org/inorder-non-threaded-binary-tree-traversal-without-recursion-or-stack/
        private IEnumerable<KeyValuePair<TKey, TValue>> DoInorderTraversalUseParent(Node<TKey, TValue> node)
        {
            var current = node;
            bool leftdone = false;

            // Start traversal from root 
            while (current != null)
            {
                // If left child is not traversed,  
                // find the leftmost child 
                if (!leftdone)
                {
                    while (current.Left != null)
                    {
                        current = current.Left;
                    }
                }

                // Return root's data 
                yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);

                // Mark left as done 
                leftdone = true;

                // If right child exists 
                if (current.Right != null)
                {
                    leftdone = false;
                    current = current.Right;
                }

                // If right child doesn't exist,  
                // move to parent 
                else if (current.Parent != null)
                {
                    // If this node is right child  
                    // of its parent, visit parent's  
                    // parent first 
                    while (current.Parent != null &&
                           current == current.Parent.Right)
                        current = current.Parent;

                    if (current.Parent == null)
                        break;
                    current = current.Parent;
                }
                else
                    break;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var elem in DoInorderTraversalUseParent(Root))
            {
                yield return elem;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}