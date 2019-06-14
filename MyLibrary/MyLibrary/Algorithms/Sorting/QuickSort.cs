using System;
using System.Collections.Generic;

namespace MyLibrary.Algorithms.Sorting
{
    public static class QuickSort<T>
    {
        private static void Swap(ref T item1, ref T item2)
        {
            var temp = item1;
            item1 = item2;
            item2 = temp;
        }

        private static int LomutoPartition(T[] array, int start, int end, IComparer<T> comparer)
        {
            var pivot = array[end];
            int i = start;

            for (int j = start; j < end; j++)
            {
                if (comparer.Compare(array[j], pivot) < 0)
                {
                    Swap(ref array[i], ref array[j]);
                    i++;
                }
            }

            Swap(ref array[i], ref array[end]);
            return i;   //новая позиция опорного элемента
        }

        private static int RandomizedLomutoPartition(T[] array, int start, int end, IComparer<T> comparer,
            Random random)
        {
            int pivotIndex = random.Next(start, end + 1);
            Swap(ref array[pivotIndex], ref array[end]);
            return LomutoPartition(array, start, end, comparer);
        }

        private static void QuickSortWithLomuto(T[] array, int start, int end, IComparer<T> comparer,
            Random random)
        {
            if (start < end)
            {
                var pivotIndex = RandomizedLomutoPartition(array, start, end, comparer, random);
                //var pivotIndex = LomutoPartition(array, start, end, comparer);
                QuickSortWithLomuto(array, start, pivotIndex - 1, comparer, random);
                QuickSortWithLomuto(array, pivotIndex + 1, end, comparer, random);
            }
        }

        private static int HoarePartition(T[] array, int start, int end, IComparer<T> comparer,
            int pivotIndex = -1)
        {
            var pivot = (pivotIndex != -1) ? array[pivotIndex] : array[start + (end - start) / 2];
            int leftIndex = start - 1, rightIndex = end + 1;
            int comparisonResultLeft, comparisonResultRight;

            while (true)
            {
                do
                {
                    leftIndex++;
                    comparisonResultLeft = comparer.Compare(array[leftIndex], pivot);
                } while (comparisonResultLeft < 0);

                do
                {
                    rightIndex--;
                    comparisonResultRight = comparer.Compare(array[rightIndex], pivot);
                } while (comparisonResultRight > 0);

                if (leftIndex >= rightIndex)
                {
                    break;                    
                }
                else if (comparisonResultLeft != comparisonResultRight)
                {
                    Swap(ref array[leftIndex], ref array[rightIndex]);
                }
            }

            return rightIndex;
        }

        private static void QuickSortWithHoare(T[] array, int start, int end, IComparer<T> comparer, 
            Random random)
        {
            if (start < end)
            {
                var pivotIndex = HoarePartition(array, start, end, comparer,
                    random.Next(start, end + 1));
                //var pivotIndex = HoarePartition(array, start, end, comparer);
                QuickSortWithHoare(array, start, pivotIndex, comparer, random);
                QuickSortWithHoare(array, pivotIndex + 1, end, comparer, random);
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

            //QuickSortWithLomuto(array, 0, array.Length - 1, comparer, new Random(DateTime.Now.Millisecond));
            QuickSortWithHoare(array, 0, array.Length - 1, comparer, new Random(DateTime.Now.Millisecond));
        }
    }
}