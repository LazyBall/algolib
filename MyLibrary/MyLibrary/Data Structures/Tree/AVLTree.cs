using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyLibrary.DataStructures
{
    //source: https://habr.com/ru/post/150732/
    public class AVLTree<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey : IComparable<TKey>
    {

        private class Node
        {
            public TKey Key { get; private set; }
            public TValue Value { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public Node Parent { get; set; }
            public int Height { get; set; }

            public Node(TKey key, TValue value, Node left = null,
                Node right = null, Node parent = null, int height = 1)
            {
                this.Key = key;
                this.Value = value;
                this.Left = left;
                this.Right = right;
                this.Parent = parent;
                this.Height = height;
            }

        }

        private Node _root;

        public int Count { get; private set; }

        //public int Height => GetNodeHeight(_root);

        public ICollection<TKey> Keys => new List<TKey>(from node in this.EnumerateNodes()
                                                        select node.Key);

        public ICollection<TValue> Values => new List<TValue>(from node in this.EnumerateNodes()
                                                              select node.Value);

        public bool IsReadOnly => false;

        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out TValue value))
                {
                    return value;
                }
                else
                {
                    throw new KeyNotFoundException
                        ("The property is retrieved and key does not exist in the collection.");
                }
            }

            set
            {
                var node = FindNode(key);
                if (node == null)
                {
                    throw new KeyNotFoundException
                        ("The property is retrieved and key does not exist in the collection.");
                }
                else
                {
                    node.Value = value;
                }
            }
        }

        private int GetNodeHeight(Node node) => ((node != null) ? node.Height : 0);

        private int GetBalancingFactor(Node node)
        {
            return (node != null) ? (GetNodeHeight(node.Right) - GetNodeHeight(node.Left)) : 0;
        }

        private void FixNodeHeight(Node node)
        {
            if (node != null)
            {
                node.Height = Math.Max(GetNodeHeight(node.Left), GetNodeHeight(node.Right)) + 1;
            }
        }

        private void CheckKey(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key is null.");
            }
        }

        public void Add(TKey key, TValue value)
        {
            CheckKey(key);
            var current = _root;
            Node parentCurrent = null;

            while (current != null)
            {
                parentCurrent = current;
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
                    throw new ArgumentException
                     ("An element with the same key already exists in the AVLTree<TKey, TValue>.");
                }
            }
        
            current = new Node(key, value, parent: parentCurrent);
            if (parentCurrent != null)
            {
                if (parentCurrent.Key.CompareTo(current.Key) > 0)
                {
                    parentCurrent.Left = current;
                }
                else
                {
                    parentCurrent.Right = current;
                }
                _root = BalanceToRoot(parentCurrent);
            }
            else
            {
                _root = current;
            }
            Count++;
        }

        private Node FindNode(TKey key)
        {
            CheckKey(key);
            var current = _root;

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

        private bool RemoveNode(Node node)
        {
            var current = node;
            if (current == null)
            {
                return false;
            }
            else
            {
                if (current.Left == null || current.Right == null)
                {
                    var replacement = current.Left ?? current.Right;
                    if (replacement != null)
                    {
                        replacement.Parent = current.Parent;
                    }
                    if (current.Parent != null)
                    {
                        if (current == current.Parent.Right)
                        {
                            current.Parent.Right = replacement;
                        }
                        else
                        {
                            current.Parent.Left = replacement;
                        }
                        _root = BalanceToRoot(current.Parent);
                    }
                    else
                    {
                        _root = replacement;
                    }
                }
                else if (current.Right.Left == null)
                {
                    current.Right.Left = current.Left;
                    current.Left.Parent = current.Right;
                    current.Right.Parent = current.Parent;
                    if (current.Parent != null)
                    {
                        if (current == current.Parent.Right)
                        {
                            current.Parent.Right = current.Right;
                        }
                        else
                        {
                            current.Parent.Left = current.Right;
                        }
                    }
                    _root = BalanceToRoot(current.Right);
                }
                else
                {
                    var successor = current.Right.Left;

                    while (successor.Left != null)
                    {
                        successor = successor.Left;
                    }

                    successor.Parent.Left = successor.Right;
                    if (successor.Right != null)
                    {
                        successor.Right.Parent = successor.Parent;
                    }
                    successor.Left = current.Left;
                    current.Left.Parent = successor;
                    successor.Right = current.Right;
                    current.Right.Parent = successor;
                    var successorParent = successor.Parent;
                    successor.Parent = current.Parent;
                    if (current.Parent != null)
                    {
                        if (current.Parent.Left == current)
                        {
                            current.Parent.Left = successor;
                        }
                        else
                        {
                            current.Parent.Right = successor;
                        }
                    }
                    _root = BalanceToRoot(successorParent);
                }                
            }
            Count--;
            return true;
        }
         
        private Node RotateRight(Node node)
        {
            var left = node.Left;
            var leftRight = left.Right;
            left.Right = node;
            left.Parent = node.Parent;
            if (node.Parent != null)
            {
                if (node == node.Parent.Right)
                {
                    node.Parent.Right = left;
                }
                else
                {
                    node.Parent.Left = left;
                }
            }
            node.Parent = left;
            node.Left = leftRight;
            if (leftRight != null)
            {
                leftRight.Parent = node;
            }           
            FixNodeHeight(node);
            FixNodeHeight(left);
            return left;
        }

        private Node RotateLeft(Node node)
        {
            var right = node.Right;
            var rightLeft = right.Left;
            right.Left = node;
            right.Parent = node.Parent;
            if (node.Parent != null)
            {
                if (node == node.Parent.Right)
                {
                    node.Parent.Right = right;
                }
                else
                {
                    node.Parent.Left = right;
                }
            }
            node.Parent = right;
            node.Right = rightLeft;
            if (rightLeft != null)
            {
                rightLeft.Parent = node;
            }                        
            FixNodeHeight(node);
            FixNodeHeight(right);
            return right;
        }

        private Node Balance(Node node)
        {
            FixNodeHeight(node);
            var balanceFactor = GetBalancingFactor(node);
            if (balanceFactor == 2)
            {
                if (GetBalancingFactor(node.Right) < 0)
                {
                    RotateRight(node.Right);
                }
                return RotateLeft(node);
            }
            else if (balanceFactor == -2)
            {
                if (GetBalancingFactor(node.Left) > 0)
                {
                    RotateLeft(node.Left);
                }
                return RotateRight(node);
            }
            return node;
        }

        private Node BalanceToRoot(Node node)
        {
            if (node == null)
            {
                return node;
            }
            var current = node;

            while (current.Parent != null)
            {
                var oldHeight = current.Height;
                current = Balance(current);
                if (oldHeight == current.Height)
                {
                    return _root;
                }
                current = current.Parent;
            }

            return Balance(current);
        }     

        public bool Remove(TKey key)
        {
            return RemoveNode(FindNode(key));
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var node = FindNode(item.Key);
            if (node == null || !node.Value.Equals(item.Value)) return false;
            else return RemoveNode(node);
        }

        public bool ContainsKey(TKey key) => (FindNode(key) != null);

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
            _root = null;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var node = FindNode(item.Key);
            if (node == null) return false;
            else return item.Value.Equals(node.Value);
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

            foreach (var elem in this)
            {
                array[arrayIndex++] = elem;
            }

        }

        private IEnumerable<Node> DoInorderTraversal(Node node)
        {
            Stack<Node> stack = new Stack<Node>(Count);
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
                yield return current;
                current = current.Right;
            }
        }

        // Function to print inorder traversal using parent pointer 
        // source https://www.geeksforgeeks.org/inorder-non-threaded-binary-tree-traversal-without-recursion-or-stack/
        private IEnumerable<Node> DoInorderTraversalUseParent(Node node)
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
                yield return current;

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
                else break;
            }
        }

        private IEnumerable<Node> EnumerateNodes()
        {

            foreach(var node in DoInorderTraversalUseParent(_root))
            {
                yield return node;
            }

        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {

            foreach (var node in this.EnumerateNodes())
            {
                yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
            }

        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}