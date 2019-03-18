using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Trees
{
    public class AVLTree<TKey, TValue> :IDictionary<TKey,TValue>
        where TKey : IComparable<TKey>
    {
        public int Count { get; private set; }

        private class Node<NTKey, NTValue>
        {
            public NTKey Key { get; private set; }
            public NTValue Value { get; set; }
            public Node<NTKey, NTValue> Left { get; set; }
            public Node<NTKey, NTValue> Right { get; set; }
            public int Height { get; set; }

            public Node(NTKey key, NTValue value, Node<NTKey, NTValue> left = null,
                Node<NTKey, NTValue> right = null, int height = 1)
            {
                this.Key = key;
                this.Value = value;
                this.Left = left;
                this.Right = right;
                this.Height = height;
            }
        }

        private Node<TKey, TValue> Root { get; set; }

        public ICollection<TKey> Keys
        {
            get
            {
                var list = new List<TKey>(Count);
                list.AddRange(from elem in this select elem.Key);
                return list;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                var list = new List<TValue>(Count);
                list.AddRange(from elem in this select elem.Value);
                return list;
            }
        }

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

        private int GetNodeHeight(Node<TKey, TValue> node)
        {
            return (node != null) ? node.Height : 0;
        }

        private int GetBalancingFactor(Node<TKey, TValue> node)
        {
            if (node == null) return 0;
            return GetNodeHeight(node.Right) - GetNodeHeight(node.Left);
        }

        private Node<TKey,TValue> FixNodeHeight(Node<TKey, TValue> node)
        {
            if (node != null)
            {
                node.Height = Math.Max(GetNodeHeight(node.Left), GetNodeHeight(node.Right)) + 1;
            }
            return node;
        }

        private void CheckKey(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key is null.");
            }
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

        private Node<TKey,TValue> FindNodeWithPath(TKey key, out Stack<Node<TKey,TValue>> path)
        {
            CheckKey(key);
            var current = Root;
            path = new Stack<Node<TKey, TValue>>(GetNodeHeight(Root) + 2);

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
                    return current;
                }
                path.Push(current);
                current = next;
            }

            return null;
        }

        private Node<TKey, TValue> RotateRight(Node<TKey, TValue> node)
        {
            var left = node.Left;
            node.Left = left.Right;
            left.Right = node;
            node=FixNodeHeight(node);
            left=FixNodeHeight(left);
            return left;
        }

        private Node<TKey, TValue> RotateLeft(Node<TKey, TValue> node)
        {
            var right = node.Right;
            node.Right = right.Left;
            right.Left = node;
            node=FixNodeHeight(node);
            right=FixNodeHeight(right);
            return right;
        }

        private Node<TKey, TValue> Balance(Node<TKey, TValue> node)
        {
            node=FixNodeHeight(node);
            var balanceFactor = GetBalancingFactor(node);
            if (balanceFactor == 2)
            {
                if (GetBalancingFactor(node.Right) < 0)
                {
                    node.Right = RotateRight(node.Right);
                }
                return RotateLeft(node);
            }
            else if (balanceFactor == -2)
            {
                if (GetBalancingFactor(node.Left) > 0)
                {
                    node.Left = RotateLeft(node.Left);
                }
                return RotateRight(node);
            }
            return node;
        }

        private Node<TKey, TValue> BalancePath(Stack<Node<TKey, TValue>> path)
        {
            Node<TKey, TValue> current = null;
            if (path.Count > 0)
            {
                current = path.Pop();
                current = Balance(current);
            }

            while (path.Count > 0)
            {
                var parent = path.Pop();
                int comparisonResult = current.Key.CompareTo(parent.Key);
                if (comparisonResult < 0)
                {
                    parent.Left = current;
                }
                else
                {
                    parent.Right = current;
                }
                current = Balance(parent);
            }

            return current;
        }

        private bool RemoveNode(Node<TKey,TValue> node, Stack<Node<TKey,TValue>> pathToNode)
        {
            var current = node;
            if (current == null)
            {
                return false;
            }
            else
            {
                Node<TKey, TValue> replacement = null; //элемент, который заменит current
                // Если один из сыновей или оба отсутсвуют, то просто заменяем
                // удаляемый элемент на существующего сына, или на null, если сыновей нет
                if (current.Left == null || current.Right == null)
                {
                    replacement = current.Left ?? current.Right;
                }
                // Если оба сына есть, то ищем элемент, который будет следующим после current
                // при обходе дерева (successor) и заменяем им текущий узел
                else
                {
                    var successor = current.Right;
                    var stackSuccessor = new Stack<Node<TKey, TValue>>(GetNodeHeight(Root)
                        - pathToNode.Count + 2);

                    while (successor.Left != null)
                    {
                        stackSuccessor.Push(successor);
                        successor = successor.Left;
                    }

                    if (stackSuccessor.Count > 0)
                    {
                        stackSuccessor.Peek().Left = successor.Right;
                    }
                    else
                    {
                        current.Right = successor.Right;
                    }

                    replacement = successor;
                    replacement.Left = current.Left;
                    replacement.Right = current.Right;
                    pathToNode.Push(replacement);

                    var replacementChild = BalancePath(stackSuccessor);
                    if (replacementChild != null)
                    {
                        pathToNode.Push(replacementChild);
                    }

                }
                Root = BalancePath(pathToNode);
            }
            Count--;
            return true;
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
                yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
                current = current.Right;
            }
        }

        public void Add(TKey key, TValue value)
        {
            CheckKey(key);
            var current = FindNodeWithPath(key, out Stack<Node<TKey, TValue>> stack);
            if (current != null)
            {
                throw new ArgumentException
                      ("An element with the same key already exists in the AVLTree<TKey, TValue>.");
            }
            current = new Node<TKey, TValue>(key, value);
            stack.Push(current);
            Root = BalancePath(stack);
            Count++;
        }

        public bool Remove(TKey key)
        {
            var current = FindNodeWithPath(key, out Stack<Node<TKey, TValue>> stack);
            return RemoveNode(current, stack);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var current = FindNodeWithPath(item.Key, out Stack<Node<TKey, TValue>> stack);
            bool found = true;
            if (current!=null)
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
            if (found) found = RemoveNode(current, stack);
            return found;
        }

        public bool ContainsKey(TKey key)
        {
            return (FindNode(key) != null);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var node = FindNode(key);
            if (node != null)
            {
                value = node.Value;
                return true;
            }
            value = default(TValue);
            return false;
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

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var elem in DoInorderTraversal(Root))
            {
                yield return elem;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}