using System;

namespace SkipList
{
    internal class Node<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public Node<TKey, TValue> Right,
                            Up,
                            Down;
        public Node()
        { }
        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Right = null;
            Up = null;
            Down = null;
        }
    }
    public class SkipList<TKey, TValue> where TKey : IComparable<TKey>
    {
        public int Count { get; private set; }
        int _maxLevel;
        int _curLevel;
        double _probability;
        Random _rd;

        Node<TKey, TValue>[] _head;
        Node<TKey, TValue> _tail;

        public SkipList(int maxLevels = 14, double p = 0.5)
        {
            _maxLevel = maxLevels;
            _probability = p;
            _curLevel = 0;
            _rd = new Random();


            _head = new Node<TKey, TValue>[_maxLevel];
            _tail = new Node<TKey, TValue>();

            for (int i = 0; i < _maxLevel; i++)
            {
                _head[i] = new Node<TKey, TValue>();
                _head[i].Right = _tail;
                if (i == 0) continue;
                _head[i].Down = _head[i - 1];
                _head[i - 1].Up = _head[i];
            }
        }
        public void Add(TKey key, TValue value)
        {
            var previousItems = new Node<TKey, TValue>[_maxLevel];
            var current = _head[_curLevel];
            for (int i = _curLevel; i >= 0; i--)
            {
                while (current.Right != _tail && current.Right.Key.CompareTo(key) < 0)
                {
                    current = current.Right;
                }
                if (current.Right.Key.CompareTo(key) == 0)
                    throw new ArgumentException("Key must be unique");
                previousItems[i] = current;
                current = current.Down;
            }
            // подбрасываем монетку, чтобы понять на какую высоту поднимется новый узел 
            int height = 0;
            while (_rd.NextDouble() < _probability && height < _maxLevel - 1)
            {
                height++;
            }
            // если высота больше текущего  уровня в SkipList, добавим недостающие элементы 
            if (height > _curLevel)
            {
                for (int i = _curLevel + 1; i <= height; i++)
                {
                    previousItems[i] = _head[i];
                }
                _curLevel = height;
            }
            // вставляем новый узел на все уровни от нижнего до height
            for (int i = 0; i <= height; i++)
            {
                var newItem = new Node<TKey, TValue>(key, value);
                newItem.Right = previousItems[i].Right;
                previousItems[i].Right = newItem;
                if (i == 0) continue;
                newItem.Down = previousItems[i - 1].Right;
                previousItems[i - 1].Right.Up = newItem;
            }

            Count++;
        }
        public void Print()
        {
            for (int i = _curLevel; i >= 0; i--)
            {
                var current = _head[i].Right;
                while (current != _tail)
                {
                    Console.Write(current.Key + " ");
                    current = current.Right;
                }
                Console.WriteLine();
            }


        }
        private Node<TKey, TValue> Find(TKey key)
        {
            var current = _head[_curLevel];
            for (int i = _curLevel; i >= 0; i--)
            {
                while (current.Right != _tail && current.Right.Key.CompareTo(key) < 0)
                {
                    current = current.Right;
                }
                if (current.Right.Key.CompareTo(key) == 0)
                {
                    return current;
                }
                current = current.Down;
            }
            return null;
        }
        public bool ContainsKey(TKey key)
        {
            if (Find(key) != null)
                return true;
            return false;
        }
        public bool RemoveKey(TKey key)
        {
            var previousItem = Find(key);
            if (previousItem == null)
                return false;
            while (previousItem.Down != null)
            {
                while (previousItem.Right.Key.CompareTo(key) < 0)
                    previousItem = previousItem.Right;
                previousItem.Right = previousItem.Right.Right;
                previousItem = previousItem.Down;
            }
            Count--;
            return true;
        }
    }
}