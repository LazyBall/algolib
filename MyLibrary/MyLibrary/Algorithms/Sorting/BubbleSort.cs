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
            int swapIndex = array.Length;

            do
            {
                int newSwapIndex = 0;

                for (int i = 1; i < swapIndex; i++)
                {
                    if (comparer.Compare(array[i], array[i - 1]) < 0)
                    {
                        Swap(ref array[i], ref array[i - 1]);
                        newSwapIndex = i;
                    }
                }

                swapIndex = newSwapIndex;
            } while (swapIndex > 1);

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