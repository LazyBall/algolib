using System;
using System.Collections.Generic;

namespace MyLibrary.Algorithms.Sorting
{
    public static class HeapSort<T>
    {
        private static void Swap(ref T item1, ref T item2)
        {
            var temp = item1;
            item1 = item2;
            item2 = temp;
        }

        private static void Heapify(T[] array, int index, int heapSize, IComparer<T> comparer)
        {
            long current = index;

            do
            {
                long left = current * 2 + 1,
                    right = left + 1,
                    largest = current;

                if ((left < heapSize) &&
                    (comparer.Compare(array[largest], array[left]) < 0))
                {
                    largest = left;
                }

                if ((right < heapSize) &&
                    (comparer.Compare(array[largest], array[right]) < 0))
                {
                    largest = right;
                }

                if (largest != current)
                {
                    Swap(ref array[current], ref array[largest]);
                    current = largest;
                }
                else
                {
                    current = -1;
                }
            } while (current != -1);
        }

        private static void BuildHeap(T[] array, IComparer<T> comparer)
        {
            for (int i = array.Length / 2 - 1; i >= 0; i--)
            {
                Heapify(array, i, array.Length, comparer);
            }
        }

        public static void Sort(T[] array)
        {
            Sort(array, Comparer<T>.Default);
        }

        public static void Sort(T[] array, IComparer<T> comparer)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array is null.");
            }
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer is null.");
            }
            BuildHeap(array, comparer);

            for (int i = array.Length - 1; i > 0; i--)
            {
                Swap(ref array[0], ref array[i]);
                Heapify(array, 0, i, comparer);
            }
        }
    }
}