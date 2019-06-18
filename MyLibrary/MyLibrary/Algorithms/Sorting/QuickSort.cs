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

        private static int LomutoPartition(T[] array, int left, int right, IComparer<T> comparer)
        {
            var pivot = array[right];
            int i = left;

            for (int j = left; j < right; j++)
            {
                if (comparer.Compare(array[j], pivot) < 0)
                {
                    Swap(ref array[i], ref array[j]);
                    i++;
                }
            }

            Swap(ref array[i], ref array[right]);
            return i;   //новая позиция опорного элемента
        }

        private static int RandomizedLomutoPartition(T[] array, int left, int right, IComparer<T> comparer,
            Random random)
        {
            int pivotIndex = random.Next(left, right + 1);
            Swap(ref array[pivotIndex], ref array[right]);
            return LomutoPartition(array, left, right, comparer);
        }

        private static void QuickSortWithLomuto(T[] array, int left, int right, IComparer<T> comparer,
            Random random)
        {
            if (left < right)
            {
                var pivotIndex = RandomizedLomutoPartition(array, left, right, comparer, random);
                //var pivotIndex = LomutoPartition(array, start, end, comparer);
                QuickSortWithLomuto(array, left, pivotIndex - 1, comparer, random);
                QuickSortWithLomuto(array, pivotIndex + 1, right, comparer, random);
            }
        }

        private static int HoarePartition(T[] array, int left, int right, IComparer<T> comparer,
            int pivotIndex)
        {
            var pivot = array[pivotIndex];
            int leftIndex = left - 1, rightIndex = right + 1;
            int comparisonResultLeft, comparisonResultRight;

            do
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
            } while (true);

            return rightIndex;
        }

        private static void QuickSortWithHoare(T[] array, int left, int right, IComparer<T> comparer,
            Random random)
        {
            if (left < right)
            {
                //var pivotIndex = left + (right - left) / 2;
                var pivotIndex = random.Next(left, right + 1);
                pivotIndex = HoarePartition(array, left, right, comparer,
                   pivotIndex);
                QuickSortWithHoare(array, left, pivotIndex, comparer, random);
                QuickSortWithHoare(array, pivotIndex + 1, right, comparer, random);
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