using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyLibrary.DataStructures
{

    public class SkipList<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey : IComparable<TKey>
    {

        private class Node
        {
            public TKey Key { get; private set; }
            public TValue Value { get; set; }
            public Node Right { get; set; }
            public Node Down { get; set; }

            public Node(TKey key, TValue value, Node right = null, Node down = null)
            {
                this.Key = key;
                this.Value = value;
                this.Right = right;
                this.Down = down;
            }

        }

        public int Count { get; private set; }

        public ICollection<TKey> Keys => new List<TKey>(from item in this select item.Key);

        public ICollection<TValue> Values => new List<TValue>(from item in this select item.Value);

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

                do
                {
                    node.Value = value;
                    node = node.Down;
                } while (node != null);

            }

        }

        readonly double _probability;
        readonly int _maxLevel;
        readonly Random _random;
        readonly Node[] _head;
        int _currentLevel;

        public SkipList(int numberOfLevels = 8, double probability = 0.5)
        {
            if (numberOfLevels < 1) throw new ArgumentException("number of levels is less than 1.");
            if (probability < 0 || probability > 1)
                throw new ArgumentException("probability value does not belong to the range from 0 to 1.");

            this._probability = probability;
            this._maxLevel = numberOfLevels;
            _head = new Node[_maxLevel];
            _currentLevel = 0;
            _random = new Random(DateTime.Now.Millisecond);

            _head[0] = new Node(default(TKey), default(TValue));
            for (int i = 1; i < _maxLevel; i++)
            {
                _head[i] = new Node(default(TKey), default(TValue), null, _head[i - 1]);
            }

        }

        public void Add(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException("key is null.");
            var previousItems = new Node[_maxLevel];
            var current = _head[_currentLevel];

            for (int i = _currentLevel; i >= 0; i--)
            {

                while (current.Right != null && current.Right.Key.CompareTo(key) < 0)
                {
                    current = current.Right;
                }

                if (current.Right?.Key.CompareTo(key) == 0)
                    throw new ArgumentException("An element with the same key already exists " +
                        "in the SkipList<TKey,TValue>.");

                previousItems[i] = current;
                current = current.Down;
            }

            // подбрасываем монетку, чтобы понять на какую высоту поднимется новый узел 
            int height = 0;
            while (_random.NextDouble() < _probability && height < _maxLevel - 1)
            {
                height++;
            }

            // если высота больше текущего  уровня в SkipList, добавим недостающие элементы 
            if (height > _currentLevel)
            {
                for (int i = _currentLevel + 1; i <= height; i++)
                {
                    previousItems[i] = _head[i];
                }

                _currentLevel = height;
            }

            // вставляем новый узел на все уровни от нижнего до height
            previousItems[0].Right = new Node(key, value, previousItems[0].Right);
            for (int i = 1; i <= height; i++)
            {
                previousItems[i].Right = new Node(key, value, previousItems[i].Right,
                    previousItems[i - 1].Right);
            }

            Count++;
        }

        //public void Print()
        //{
        //    for (int i = _currentLevel; i >= 0; i--)
        //    {
        //        var current = _head[i].Right;
        //        while (current != null)
        //        {
        //            Console.Write(current.Key + " ");
        //            current = current.Right;
        //        }
        //        Console.WriteLine();
        //    }
        //}

        private Node FindNode(TKey key)
        {
            if (key == null) throw new ArgumentNullException("key is null.");

            var current = _head[_currentLevel];

            for (int i = _currentLevel; i >= 0; i--)
            {
                while (current.Right != null && current.Right.Key.CompareTo(key) < 0)
                {
                    current = current.Right;
                }

                if (current.Right?.Key.CompareTo(key) == 0) return current.Right;

                current = current.Down;
            }

            return null;
        }

        private Node FindNodeWithParent(TKey key, out Node parent)
        {
            if (key == null) throw new ArgumentNullException("key is null.");

            var current = _head[_currentLevel];
            parent = null;

            for (int i = _currentLevel; i >= 0; i--)
            {
                while (current.Right != null && current.Right.Key.CompareTo(key) < 0)
                {
                    current = current.Right;
                }

                if (current.Right?.Key.CompareTo(key) == 0)
                {
                    parent = current;
                    return current.Right;
                }

                current = current.Down;
            }

            return null;
        }

        public bool ContainsKey(TKey key) => (FindNode(key) != null);

        private bool RemoveNode(Node node, Node nodeParent)
        {

            do
            {
                while (nodeParent.Right.Key.CompareTo(node.Key) < 0)
                {
                    nodeParent = nodeParent.Right;
                }

                nodeParent.Right = nodeParent.Right.Right;
                nodeParent = nodeParent.Down;
            } while (nodeParent != null);

            Count--;
            return true;
        }

        public bool Remove(TKey key)
        {
            var node = FindNodeWithParent(key, out Node nodeParent);
            if (node == null) return false;
            else return RemoveNode(node, nodeParent);
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

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {

            for (int i = 0; i <= _currentLevel; i++)
            {
                _head[i].Right = null;
            }

            _currentLevel = 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var node = FindNode(item.Key);
            if (node == null) return false;
            else return node.Value.Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("array is null.");
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex is less than 0.");
            }
            if (array.Length < arrayIndex + this.Count)
            {
                throw new ArgumentException("The number of elements in the source " +
                        "SkipList<TKey,TValue> " +
                        "is greater than the available space from arrayIndex to the end of the " +
                        "destination array.");
            }

            foreach (var elem in this)
            {
                array[arrayIndex++] = elem;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var node = FindNodeWithParent(item.Key, out Node nodeParent);
            if (node == null || !node.Value.Equals(item.Value)) return false;
            else return RemoveNode(node, nodeParent);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            var node = _head[0].Right;

            while (node != null)
            {
                yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
                node = node.Right;
            }

        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    }
}