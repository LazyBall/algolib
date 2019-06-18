using System;
using System.Collections.Generic;
using System.Linq;

namespace MyLibrary.DataStructures
{
    public class BinaryHeap<T> where T : IComparable<T>
    {
        public int Count { get; private set; }

        private T[] _array;

        public BinaryHeap()
        {
            _array = new T[16];
            Count = 0;
        }

        public BinaryHeap(int capacity)
        {
            _array = new T[capacity];
            Count = 0;
        }

        public BinaryHeap(IEnumerable<T> collection)
        {
            _array = collection.ToArray();
            Count = _array.Length;

            for (int i = Count / 2 - 1; i >= 0; i--)
            {
                Heapify(i);
            }
        }

        private void Resize()
        {
            var oldArray = _array;
            _array = new T[oldArray.Length * 2];
            Array.Copy(oldArray, _array, oldArray.Length);
        }

        private void Heapify(int index)
        {
            long current = index;

            do
            {
                long left = current * 2 + 1,
                    right = left + 1,
                    largest = current;

                if ((left < Count) &&
                    (_array[left].CompareTo(_array[largest]) > 0))
                {
                    largest = left;
                }

                if ((right < Count) &&
                    (_array[right].CompareTo(_array[largest]) > 0))
                {
                    largest = right;
                }

                if (largest != current)
                {
                    var temp = _array[current];
                    _array[current] = _array[largest];
                    _array[largest] = temp;
                    current = largest;
                }
                else
                {
                    current = -1;
                }
            } while (current != -1);
        }

        public void Add(T item)
        {
            if (Count >= _array.Length)
            {
                Resize();
            }
            _array[Count] = item;

            int itemIndex = Count;
            int parentIndex = (itemIndex - 1) / 2;

            while (itemIndex > 0 && _array[parentIndex].CompareTo(_array[itemIndex]) < 0)
            {
                var temp = _array[itemIndex];
                _array[itemIndex] = _array[parentIndex];
                _array[parentIndex] = temp;

                itemIndex = parentIndex;
                parentIndex = (itemIndex - 1) / 2;
            }

            Count++;
        }

        public T Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException();
            }
            else
            {
                return _array[0];
            }
        }

        public T Dequeue()
        {
            var value = Peek();
            Count--;
            if (Count > 0)
            {
                _array[0] = _array[Count];
                Heapify(0);
            }
            return value;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_array[i].CompareTo(item) == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}