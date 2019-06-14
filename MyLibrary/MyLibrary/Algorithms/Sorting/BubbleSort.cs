using System;
using System.Collections.Generic;

namespace MyLibrary.Algorithms.Sorting
{
    public static class BubbleSort<T>
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

            bool swapped = true;
            int i = 0;

            do
            {
                swapped = false;

                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (comparer.Compare(array[j], array[j + 1]) > 0)
                    {
                        Swap(ref array[j], ref array[j + 1]);
                        swapped = true;
                    }
                }

                i++;
            } while (swapped && (i < array.Length - 1));
          
        }
    }
}