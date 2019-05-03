using System;
using System.Collections.Generic;
using System.Linq;

namespace MyLibrary.DataStructures
{
    public class BinaryHeap<T> where T : IComparable<T>
    {
        public int Count { get; private set; }

        private T[] _array;

        public BinaryHeap(int capacity = 16)
        {
            _array = new T[capacity];
            Count = 0;
        }

        public BinaryHeap(IEnumerable<T> collection)
        {
            _array = collection.ToArray();
            Count = _array.Length;

            for (int i = Count / 2; i >= 0; i--)
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

            while (true)
            {
                var leftChildIndex = (long)2 * index + 1;
                var rightChildIndex = (long)2 * index + 2;
                long largestChildIndex = index;

                if (leftChildIndex < Count &&
                    _array[leftChildIndex].CompareTo(_array[largestChildIndex]) > 0)
                {
                    largestChildIndex = leftChildIndex;
                }

                if (rightChildIndex < Count &&
                    _array[rightChildIndex].CompareTo(_array[largestChildIndex]) > 0)
                {
                    largestChildIndex = rightChildIndex;
                }

                if (largestChildIndex != index)
                {
                    var temp = _array[index];
                    _array[index] = _array[largestChildIndex];
                    _array[largestChildIndex] = temp;
                    index = (int)largestChildIndex;
                }
                else
                {
                    break;
                }
            }

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

        public T Pop()
        {
            var value = Peek();
            if (--Count > 0)
            {
                _array[0] = _array[Count];
                Heapify(0);
            }
            return value;
        }

    }
}