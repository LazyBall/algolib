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

        public bool Remove(TKey key)
        {
            var current = Find(key);
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
                        // Смотрим, какой current: левый или правый сын parent
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

        //TODO два раза вызывается метод Find. Нужно либо реализовать метод удаления
        // принимающий сам узел, либоне реализовывать этот.
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
            if(resultRemoving)
            {
                resultRemoving = Remove(item.Key);
            }
            return resultRemoving;
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