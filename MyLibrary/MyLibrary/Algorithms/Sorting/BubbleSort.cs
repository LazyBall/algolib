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

        private static void SortWithoutOptimization(T[] array, IComparer<T> comparer)
        {

            for (int i = 1; i < array.Length; i++)
            {

                for (int j = 1; j < array.Length; j++)
                {
                    if (comparer.Compare(array[j], array[j - 1]) < 0)
                    {
                        Swap(ref array[j], ref array[j - 1]);
                    }
                }

            }

        }

        private static void SortWithOptimization(T[] array, IComparer<T> comparer)
        {
            bool swapped = true;
            int i = 1;

            do
            {
                swapped = false;

                for (int j = 0; j < array.Length - i; j++)
                {
                    if (comparer.Compare(array[j], array[j + 1]) > 0)
                    {
                        Swap(ref array[j], ref array[j + 1]);
                        swapped = true;
                    }
                }

                i++;
            } while (swapped && (i < array.Length));

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
            //SortWithoutOptimization(array, comparer);
            SortWithOptimization(array, comparer);          
        }
    }
}