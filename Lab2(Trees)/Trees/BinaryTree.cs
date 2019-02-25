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
                    throw new ArgumentException();
                }
            }
            set
            {
                var node = Find(key);
                if (node == null) throw new ArgumentException();
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
            Node<TKey, TValue> parentCurrent = null;
            var current = Root;
            while (current != null)
            {
                parentCurrent = current;
                int compResult = current.Key.CompareTo(key);
                if (compResult > 0)
                {
                    current = current.Left;
                }
                else if (compResult < 0)
                {
                    current = current.Right;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            current = new Node<TKey, TValue>(key, value, parent: parentCurrent);
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
            var current = Find(item.Key);
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
            if (Find(key) == null)
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
                throw new ArgumentOutOfRangeException("Значение параметра arrayIndex меньше 0.");
            }
            if(array==null)
            {
                throw new ArgumentNullException("Свойство array имеет значение null.");
            }
            int i = arrayIndex;
            foreach (var elem in this)
            {
                array[i] = elem;
                i++;
                if (i >= array.Length)
                {
                    throw new ArgumentException("Число элементов в исходной коллекции ICollection<T> " +
                        "больше доступного места от положения, заданного значением параметра arrayIndex, " +
                        "до конца массива назначения array.");
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

        private Node<TKey, TValue> Find(TKey key)
        {
            var current = Root;
            while (current != null)
            {
                int compResult = current.Key.CompareTo(key);
                if (compResult > 0)
                {
                    current = current.Left;
                }
                else if (compResult < 0)
                {
                    current = current.Right;
                }
                else
                {
                    return current;
                }
            }
            return current;
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
                // Случай 1: Если у current нет детей справа, левый ребенок встает на место удаляемого
                if (current.Right == null)
                {
                    if (current.Parent == null)
                    {
                        Root = current.Left;
                    }
                    else
                    {
                        // Смотрим, какой current: левый или правый сын Parent
                        int compResult = current.Parent.Key.CompareTo(current.Key);
                        if (compResult > 0)
                        {
                            // Если key родителя больше текущего,
                            // левый ребенок current становится левым ребенком родителя.
                            current.Parent.Left = current.Left;
                        }
                        else if (compResult < 0)
                        {
                            // Если key родителя меньше текущего, 
                            // левый ребенок current становится правым ребенком родителя. 
                            current.Parent.Right = current.Left;
                        }
                    }
                }
                // Случай 2: Если у правого ребенка current нет детей слева 
                // то он занимает место удаляемого узла. 
                else if (current.Right.Left == null)
                {
                    current.Right.Left = current.Left;
                    if (current.Parent == null)
                    {
                        Root = current.Right;
                    }
                    else
                    {
                        int compResult = current.Parent.Key.CompareTo(current.Key);
                        if (compResult > 0)
                        {
                            // Если key родителя больше текущего,
                            // правый ребенок current становится левым ребенком родителя.
                            current.Parent.Left = current.Right;
                        }
                        else if (compResult < 0)
                        {
                            // Если key родителя меньше текущего, 
                            // правый ребенок current становится правым ребенком родителя.
                            current.Parent.Right = current.Right;
                        }
                    }
                }
                // Случай 3: Если у правого ребенка есть дети слева, крайний левый ребенок 
                // из правого поддерева заменяет удаляемый узел. 
                else
                {
                    // Найдем крайний левый узел. 
                    var leftMost = current.Right.Left;
                    while (leftMost.Left != null)
                    {
                        leftMost = leftMost.Left;
                    }
                    // Правое поддерево самого левого узла становится левым поддеревом родителя
                    // самого левого узла 
                    leftMost.Parent.Left = leftMost.Right;
                    // Левый и правый ребенок текущего узла становится левым и правым ребенком 
                    // крайнего левого
                    leftMost.Left = current.Left;
                    leftMost.Right = current.Right;
                    if (current.Parent == null)
                    {
                        Root = leftMost;
                    }
                    else
                    {
                        int result = current.Parent.Key.CompareTo(current.Key);
                        if (result > 0)
                        {
                            // Если значение родителя больше текущего,
                            // крайний левый узел становится левым ребенком родителя.
                            current.Parent.Left = leftMost;
                        }
                        else if (result < 0)
                        {
                            // Если значение родителя меньше текущего,
                            // крайний левый узел становится правым ребенком родителя.
                            current.Parent.Right = leftMost;
                        }
                    }
                }
            }
            Count--;
            return true;
        }

        public bool Remove(TKey key)
        {
            var current = Find(key);
            return RemoveNode(current);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool resultRemoving = true;
            var current = Find(item.Key);
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
                resultRemoving = RemoveNode(current);
            }
            return resultRemoving;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var node = Find(key);
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
            foreach (var elem in Traverse(Root))
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