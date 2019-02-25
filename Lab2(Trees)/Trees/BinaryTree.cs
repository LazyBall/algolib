using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public class BinaryTree<TKey, TValue>: IDictionary<TKey, TValue>
        where TKey:IComparable<TKey>
    {       
        public TValue this[TKey key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count { get; private set; }

        public ICollection<TKey> Keys => throw new NotImplementedException();

        public ICollection<TValue> Values => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        private Node<TKey,TValue> Root { get; set; }


        public void Add(TKey key, TValue value)
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
                    throw new ArgumentException();
                }
            }
            current = new Node<TKey, TValue>(key, value);
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
            var current = Root;
            while (current != null)
            {
                int compResult = current.Key.CompareTo(item.Key);
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
                    if (item.Value is IComparable<TValue> compValue)
                    {
                        return (compValue.CompareTo(current.Value) == 0);
                    }
                    return item.Value.Equals(current.Value);
                }
            }
            return false;
        }

        public bool ContainsKey(TKey key)
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
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            var list = new List<KeyValuePair<TKey, TValue>>(Count);
            Traverse(Root, list);
            foreach (var elem in list)
                yield return elem;
        }
        
        private void Traverse(Node<TKey,TValue> node,
            List<KeyValuePair<TKey, TValue>> list)
        {
            if(node!=null)
            {
                Traverse(node.Left, list);
                list.Add(new KeyValuePair<TKey, TValue>(node.Key, node.Value));
                Traverse(node.Right, list);
            }
        }

        public bool Remove(TKey key)
        {
            var current = FindWithParent(key, out Node<TKey, TValue> parent);
            if (current == null)
            {
                return false;
            }
            else
            {
                // Случай 1: Если у current нет детей справа, левый ребенок встает на место удаляемого
                if (current.Right == null)
                {
                    if (parent == null)
                    {
                        Root = current.Left;
                    }
                    else
                    {
                        // Смотрим, какой current: левый или правый сын parent
                        int compResult = parent.Key.CompareTo(current.Key);
                        if (compResult > 0)
                        {
                            // Если key родителя больше текущего,
                            // левый ребенок current становится левым ребенком родителя.
                            parent.Left = current.Left;
                        }
                        else if (compResult < 0)
                        {
                            // Если key родителя меньше текущего, 
                            // левый ребенок current становится правым ребенком родителя. 
                            parent.Right = current.Left;
                        }
                    }
                }
                // Случай 2: Если у правого ребенка current нет детей слева 
                // то он занимает место удаляемого узла. 
                else if (current.Right.Left == null)
                {
                    current.Right.Left = current.Left;
                    if (parent == null)
                    {
                        Root = current.Right;
                    }
                    else
                    {
                        int compResult = parent.Key.CompareTo(current.Key);
                        if (compResult > 0)
                        {
                            // Если key родителя больше текущего,
                            // правый ребенок current становится левым ребенком родителя.
                            parent.Left = current.Right;
                        }
                        else if (compResult < 0)
                        {
                            // Если key родителя меньше текущего, 
                            // правый ребенок current становится правым ребенком родителя.
                            parent.Right = current.Right;
                        }
                    }
                }
                // Случай 3: Если у правого ребенка есть дети слева, крайний левый ребенок 
                // из правого поддерева заменяет удаляемый узел. 
                else
                {
                    // Найдем крайний левый узел. 
                    var leftMost = current.Right.Left;
                    var leftMostParent = current.Right;
                    while (leftMost.Left != null)
                    {
                        leftMostParent = leftMost;
                        leftMost = leftMost.Left;
                    }
                    // Правое поддерево самого левого узла становится левым поддеревом родителя
                    // самого левого узла 
                    leftMostParent.Left = leftMost.Right;
                    // Левый и правый ребенок текущего узла становится левым и правым ребенком 
                    // крайнего левого
                    leftMost.Left = current.Left;
                    leftMost.Right = current.Right;
                    if (parent == null)
                    {
                        Root = leftMost;
                    }
                    else
                    {
                        int result = parent.Key.CompareTo(current.Key);
                        if (result > 0)
                        {
                            // Если значение родителя больше текущего,
                            // крайний левый узел становится левым ребенком родителя.
                            parent.Left = leftMost;
                        }
                        else if (result < 0)
                        {
                            // Если значение родителя меньше текущего,
                            // крайний левый узел становится правым ребенком родителя.
                            parent.Right = leftMost;
                        }
                    }
                }
            }
            Count--;
            return true;
        }

        private Node<TKey,TValue> FindWithParent(TKey key, out Node<TKey,TValue> parent)
        {
            parent = null;
            var current = Root;
            while (current != null)
            {
                int compResult = current.Key.CompareTo(key);
                if (compResult > 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else if (compResult < 0)
                {
                    parent = current;
                    current = current.Right;
                }
                else
                {
                    return current;
                }
            }
            return current;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}