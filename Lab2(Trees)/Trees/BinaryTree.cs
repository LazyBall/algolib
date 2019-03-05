using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Trees
{
    public class BinaryTree<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey : IComparable<TKey>
    {
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
                var node = FindWithParent(key, out Node<TKey, TValue> parent);
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



        public void Add(TKey key, TValue value)
        {
            CheckKey(key);
            Node<TKey, TValue> parentCurrent = null;
            var current = Root;
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
                        ("An element with the same key already exists in the BinaryTree<TKey,TValue>.");
                }
            }
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

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var current = FindWithParent(item.Key, out Node<TKey, TValue> parent);
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

        public bool ContainsKey(TKey key)
        {
            if (FindWithParent(key, out Node<TKey, TValue> parent) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex is less than 0.");
            }
            if(array==null)
            {
                throw new ArgumentNullException("array is null.");
            }
            int i = arrayIndex;
            foreach (var elem in this)
            {
                array[i] = elem;
                i++;
                if (i >= array.Length)
                {
                    throw new ArgumentException("The number of elements in the source " +
                        "BinaryTree<TKey,TValue> " +
                        "is greater than the available space from index to the end of the " +
                        "destination array.");
                }
            }
        }

        private IEnumerable<KeyValuePair<TKey, TValue>> Traverse(Node<TKey, TValue> node)
        {
            if (node != null)
            {
                foreach (var elem in Traverse(node.Left))
                {
                    yield return elem;
                }
                yield return (new KeyValuePair<TKey, TValue>(node.Key, node.Value));
                foreach (var elem in Traverse(node.Right))
                {
                    yield return elem;
                }
            }
        }

        private Node<TKey, TValue> FindWithParent(TKey key, out Node<TKey, TValue> parent)
        {
            CheckKey(key);
            parent = null;
            var current = Root;
            while (current != null)
            {              
                int comparisonResult = current.Key.CompareTo(key);
                Node<TKey, TValue> next;
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
                // Случай 1: Если у current нет детей справа, левый ребенок встает на место удаляемого
                if (current.Right == null)
                {
                    if (currentParent == null)
                    {
                        Root = current.Left;
                    }
                    else
                    {
                        // Смотрим, какой current: левый или правый сын Parent
                        int compResult = currentParent.Key.CompareTo(current.Key);
                        if (compResult > 0)
                        {
                            // Если key родителя больше текущего,
                            // левый ребенок current становится левым ребенком родителя.
                            currentParent.Left = current.Left;
                        }
                        else if (compResult < 0)
                        {
                            // Если key родителя меньше текущего, 
                            // левый ребенок current становится правым ребенком родителя. 
                            currentParent.Right = current.Left;
                        }
                    }
                }
                // Случай 2: Если у правого ребенка current нет детей слева 
                // то он занимает место удаляемого узла. 
                else if (current.Right.Left == null)
                {
                    current.Right.Left = current.Left;
                    if (currentParent == null)
                    {
                        Root = current.Right;
                    }
                    else
                    {
                        int compResult = currentParent.Key.CompareTo(current.Key);
                        if (compResult > 0)
                        {
                            // Если key родителя больше текущего,
                            // правый ребенок current становится левым ребенком родителя.
                            currentParent.Left = current.Right;
                        }
                        else if (compResult < 0)
                        {
                            // Если key родителя меньше текущего, 
                            // правый ребенок current становится правым ребенком родителя.
                            currentParent.Right = current.Right;
                        }
                    }             
                }
                // Случай 3: Если у правого ребенка есть дети слева, крайний левый ребенок 
                // из правого поддерева заменяет удаляемый узел. 
                else
                {
                    // Найдем крайний левый узел. 
                    var leftmost = current.Right.Left;
                    var parentLeftmost = current.Right;
                    while (leftmost.Left != null)
                    {
                        parentLeftmost = leftmost;
                        leftmost = leftmost.Left;
                    }

                    // Правое поддерево самого левого узла становится левым поддеревом родителя
                    // самого левого узла 
                    parentLeftmost.Left = leftmost.Right;

                    // Левый и правый ребенок текущего узла становится левым и правым ребенком 
                    // крайнего левого
                    leftmost.Left = current.Left;
                    leftmost.Right = current.Right;
                    if (currentParent == null)
                    {
                        Root = leftmost;
                    }
                    else
                    {
                        int result = currentParent.Key.CompareTo(current.Key);
                        if (result > 0)
                        {
                            // Если значение родителя больше текущего,
                            // крайний левый узел становится левым ребенком родителя.
                            currentParent.Left = leftmost;
                        }
                        else if (result < 0)
                        {
                            // Если значение родителя меньше текущего,
                            // крайний левый узел становится правым ребенком родителя.
                            currentParent.Right = leftmost;
                        }
                    }
                }
            }
            Count--;
            return true;
        }

        public bool Remove(TKey key)
        {
            var current = FindWithParent(key, out Node<TKey, TValue> parent);
            return RemoveNode(current, parent);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool resultRemoving = true;
            var current = FindWithParent(item.Key, out Node<TKey, TValue> parent);
            if (current == null)
            {
                resultRemoving = false;
            }
            else
            {
                if (item.Value is IComparable<TValue> compValue)
                {
                    resultRemoving = (compValue.CompareTo(current.Value) == 0);
                }
                else
                {
                    resultRemoving = item.Value.Equals(current.Value);
                }
            }
            if (resultRemoving)
            {
                resultRemoving = RemoveNode(current, parent);
            }
            return resultRemoving;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var node = FindWithParent(key, out Node<TKey, TValue> parent);
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

        private IEnumerable<KeyValuePair<TKey, TValue>> DoInorderTraversal(Node<TKey,TValue> node)
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