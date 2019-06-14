using System;
using System.Collections.Generic;

namespace MyLibrary.Algorithms.Sorting
{
    public static class SelectionSort<T>
    {
        private static void Swap(ref T item1, ref T item2)
        {
            var temp = item1;
            item1 = item2;
            item2 = temp;
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

            for (int i = 0; i < array.Length - 1; i++)
            {
                int minIndex = i;

                for (int j = i + 1; j < array.Length; j++)
                {
                    if (comparer.Compare(array[j], array[minIndex]) < 0)
                    {
                        minIndex = j;
                    }
                }

                if (minIndex != i)
                {
                    Swap(ref array[i], ref array[minIndex]);
                }
            }

        }
    }
}