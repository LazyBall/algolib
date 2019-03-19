using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Trees
{
    public class BinarySearchTree<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey : IComparable<TKey>
    {

        private class Node<NTKey, NTValue>
        {
            public NTKey Key { get; private set; }
            public NTValue Value { get; set; }
            public Node<NTKey, NTValue> Left { get; set; }
            public Node<NTKey, NTValue> Right { get; set; }


            public Node(NTKey key, NTValue value, Node<NTKey, NTValue> left = null,
                Node<NTKey, NTValue> right = null)
            {
                this.Key = key;
                this.Value = value;
                this.Left = left;
                this.Right = right;
            }
        }

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
                var node = FindNodeWithParent(key, out Node<TKey, TValue> parent);
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

        public int Count { get; private set; }

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

        private Node<TKey,TValue> Root { get; set; }

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

        private IEnumerable<KeyValuePair<TKey, TValue>> DoMorrisTraversal(Node<TKey,TValue> node)
        {
            Node<TKey,TValue> current, pre;
            current = node;

            while (current != null)
            {
                if (current.Left == null)
                {
                    yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
                    current = current.Right;
                }
                else
                {
                    /* Find the inorder predecessor of current */
                    pre = current.Left;

                    while (pre.Right != null && pre.Right != current)
                    {
                        pre = pre.Right;
                    }

                    /* Make current as right child  
                    of its inorder predecessor */
                    if (pre.Right == null)
                    {
                        pre.Right = current;
                        current = current.Left;
                    }

                    /* Revert the changes made in  
                    if part to restore the original  
                    tree i.e., fix the right child  
                    of predecssor*/
                    else
                    {
                        pre.Right = null;
                        yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
                        current = current.Right;
                    } /* End of if condition pre->right == NULL */

                } /* End of if condition current->left == NULL*/
            }  

        }

        public void Add(TKey key, TValue value)
        {
            var current = FindNodeWithParent(key, out Node<TKey, TValue> parentCurrent);
            if (current != null)
            {
                throw new ArgumentException
                       ("An element with the same key already exists in the BinarySearchTree<TKey,TValue>.");
            }
            else
            {
                current = new Node<TKey, TValue>(key, value);
                if (parentCurrent == null)
                {
                    Root = current;
                }
                else
                {
                    if (current.Key.CompareTo(parentCurrent.Key) < 0)
                    {
                        parentCurrent.Left = current;
                    }
                    else
                    {
                        parentCurrent.Right = current;
                    }
                }
            }
            Count++;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            Root = null;
            Count = 0;
        }

        public bool ContainsKey(TKey key)
        {
            if (FindNodeWithParent(key, out Node<TKey, TValue> parent) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var current = FindNodeWithParent(item.Key, out Node<TKey, TValue> parent);
            if (current == null)
            {
                return false;
            }
            else
            {
                if (item.Value is IComparable<TValue> compValue)
                {
                    return (compValue.CompareTo(current.Value) == 0);
                }
                else
                {
                    return item.Value.Equals(current.Value);
                }             
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array is null.");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index is less than 0.");
            }
            if (array.Length < index + this.Count)
            {
                throw new ArgumentException("The number of elements in the source " +
                        "BinarySearchTree<TKey,TValue> " +
                        "is greater than the available space from index to the end of the " +
                        "destination array.");
            }

            int i = index;

            foreach (var elem in this)
            {
                array[i] = elem;
                i++;
            }

        }

        private Node<TKey, TValue> FindNodeWithParent(TKey key, out Node<TKey, TValue> parent)
        {
            CheckKey(key);
            parent = null;
            var current = Root;
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
                parent = current;
                current = next;
            }
            return null;
        }

        private bool RemoveNode(Node<TKey, TValue> node, Node<TKey, TValue> nodeParent)
        {
            var current = node;
            var currentParent = nodeParent;
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
                    Node<TKey, TValue> successorParent = null;

                    while (successor.Left != null)
                    {
                        successorParent = successor;
                        successor = successor.Left;
                    }

                    if (successorParent != null)
                    {
                        successorParent.Left = successor.Right;
                    }
                    else
                    {
                        current.Right = successor.Right;
                    }

                    replacement = successor;
                    replacement.Left = current.Left;
                    replacement.Right = current.Right;
                }
                if (currentParent == null)
                {
                    Root = replacement;
                }
                else
                {
                    // Смотрим, какой current: левый или правый сын currentParent
                    if (currentParent.Key.CompareTo(current.Key) > 0)
                    {
                        // Если key родителя больше текущего,
                        // replacement становится левым ребенком родителя.
                        currentParent.Left = replacement;
                    }
                    else
                    {
                        // Если key родителя меньше текущего, 
                        // replacement становится правым ребенком родителя. 
                        currentParent.Right = replacement;
                    }
                }
            }
            Count--;
            return true;
        }

        public bool Remove(TKey key)
        {
            return RemoveNode(FindNodeWithParent(key, out Node<TKey, TValue> parent), parent);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool found = true;
            var current = FindNodeWithParent(item.Key, out Node<TKey, TValue> parent);
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
            if (found)
            {
                found = RemoveNode(current, parent);
            }
            return found;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var node = FindNodeWithParent(key, out Node<TKey, TValue> parent);
            if (node == null)
            {
                value = default(TValue);
                return false;
            }
            else
            {
                value = node.Value;
                return true;
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

        private void CheckKey(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key is null.");
            }
        }
    }
}