using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyLibrary.DataStructures
{
    public class BinarySearchTree<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey : IComparable<TKey>
    {

        private class Node
        {
            public TKey Key { get; private set; }
            public TValue Value { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }


            public Node(TKey key, TValue value, Node left = null,
                Node right = null)
            {
                this.Key = key;
                this.Value = value;
                this.Left = left;
                this.Right = right;
            }
        }

        private Node _root;

        public int Count { get; private set; }

        public void Add(TKey key, TValue value)
        {
            var current = FindNodeWithParent(key, out Node parentCurrent);
            if (current != null)
            {
                throw new ArgumentException
                       ("An element with the same key already exists in the BinarySearchTree<TKey,TValue>.");
            }
            else
            {
                current = new Node(key, value);
                if (parentCurrent == null)
                {
                    _root = current;
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

        private bool RemoveNode(Node node, Node nodeParent)
        {
            var current = node;
            var currentParent = nodeParent;
            if (current == null)
            {
                return false;
            }
            else
            {
                Node replacement = null; //элемент, который заменит current
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
                    Node successorParent = null;

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
                    _root = replacement;
                }
                else
                {
                    // Смотрим, какой current: левый или правый сын currentParent
                    if (currentParent.Left == current)
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

        private void CheckKey(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key is null.");
            }
        }

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

        public ICollection<TKey> Keys => new List<TKey>(from item in this select item.Key);

        public ICollection<TValue> Values => new List<TValue>(from item in this select item.Value);

        public bool IsReadOnly => false;       

        //source: https://www.geeksforgeeks.org/inorder-tree-traversal-without-recursion/
        private IEnumerable<KeyValuePair<TKey, TValue>> DoInorderTraversal(Node node)
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
                yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
                current = current.Right;
            }
        }

        //source: https://www.geeksforgeeks.org/inorder-tree-traversal-without-recursion-and-without-stack/
        private IEnumerable<KeyValuePair<TKey, TValue>> DoMorrisTraversal(Node node)
        {
            Node current, pre;
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

        private Node FindNodeWithParent(TKey key, out Node parent)
        {
            CheckKey(key);
            parent = null;
            var current = _root;

            while (current != null)
            {
                int comparisonResult = current.Key.CompareTo(key);
                Node next = null;
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

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _root = null;
            Count = 0;
        }

        public bool ContainsKey(TKey key) => (FindNode(key) != null);

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var node = FindNode(item.Key);
            if (node == null) return false;
            else return item.Value.Equals(node.Value);
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

            foreach (var item in this)
            {
                array[index++] = item;
            }

        }

        public bool Remove(TKey key) => RemoveNode(FindNodeWithParent(key, out Node parent), parent);

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var node = FindNodeWithParent(item.Key, out Node parent);
            if (node == null || !item.Value.Equals(node.Value)) return false;
            else return RemoveNode(node, parent);            
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var node = FindNode(key);
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
            foreach (var item in DoInorderTraversal(_root))
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}