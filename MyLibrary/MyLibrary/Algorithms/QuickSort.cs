using System;
using System.Collections.Generic;

namespace MyLibrary.Algorithms
{
    public static class QuickSort<T>
    {

        private static void Swap(ref T item1, ref T item2)
        {
            var temp = item1;
            item1 = item2;
            item2 = temp;
        }

        private static int LomutoPartition(T[] array, int start, int end,
            IComparer<T> comparer)
        {
            var pivot = array[end];
            int i = start;

            for (int j = start; j < end; j++)
            {
                if (comparer.Compare(array[j], pivot) <= 0)
                {
                    Swap(ref array[i], ref array[j]);
                    i++;
                }
            }

            Swap(ref array[i], ref array[end]);
            return i;   //новая позиция опорного элемента
        }

        private static int RandomizedLomutoPartition(T[] array, int start, int end,
            IComparer<T> comparer)
        {
            int pivotIndex = new Random(DateTime.Now.Millisecond).Next(start, end + 1);
            Swap(ref array[pivotIndex], ref array[end]);
            return LomutoPartition(array, start, end, comparer);
        }

        private static int HoarePartition(T[] array, int start, int end, IComparer<T> comparer,
            int pivotIndex = -1)
        {
            var pivot = (pivotIndex != -1) ? array[pivotIndex] : array[start + (end - start) / 2];
            int leftIndex = start - 1, rightIndex = end + 1;

            while (true)
            {
                do
                {
                    leftIndex++;
                } while (comparer.Compare(array[leftIndex], pivot) < 0);

                do
                {
                    rightIndex--;
                } while (rightIndex > leftIndex && comparer.Compare(array[rightIndex], pivot) >= 0);

                if (leftIndex < rightIndex)
                {
                    Swap(ref array[leftIndex], ref array[rightIndex]);
                }
                else
                {
                    return rightIndex;
                }
            }

        }

        private static void QuickSortWithLomuto(T[] array, int start, int end, IComparer<T> comparer)
        {
            if (start < end)
            {
                int pivotIndex = RandomizedLomutoPartition(array, start, end, comparer);
                QuickSortWithLomuto(array, start, pivotIndex - 1, comparer);
                QuickSortWithLomuto(array, pivotIndex + 1, end, comparer);
            }
        }

        private static void QuickSortWithHoare(T[] array, int start, int end, IComparer<T> comparer)
        {
            if (start < end)
            {
                int pivotIndex = HoarePartition(array, start, end, comparer,
                    new Random(DateTime.Now.Millisecond).Next(start, end + 1));
                QuickSortWithHoare(array, start, pivotIndex - 1, comparer);
                QuickSortWithHoare(array, pivotIndex, end, comparer);
            }
        }

        public static void Sort(T[] array)
        {
            Sort(array, Comparer<T>.Default);
        }

        public static void Sort(T[] array, IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer is null");
            }

            //QuickSortWithLomuto(array, 0, array.Length - 1, Comparer<T>.Default);
            QuickSortWithHoare(array, 0, array.Length - 1, comparer);
        }

    }
}