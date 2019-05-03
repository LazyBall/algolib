using System;

namespace MyLibrary.Algorithms
{
    public static class QuickSort<T> where T : IComparable<T>
    {

        private static int LomutoPartition(T[] array, int start, int end)
        {
            var pivot = array[end];
            int i = start;

            for (int j = start; j < end; j++)
            {
                if (array[j].CompareTo(pivot) <= 0)
                {
                    Swap(ref array[i], ref array[j]);
                    i++;
                }
            }

            Swap(ref array[i], ref array[end]);
            return i;   //новая позиция опорного элемента
        }

        private static int RandomizedLomutoPartition(T[] array, int start, int end)
        {
            int pivotIndex = new Random(DateTime.Now.Millisecond).Next(start, end + 1);
            Swap(ref array[pivotIndex], ref array[end]);
            return LomutoPartition(array, start, end);
        }

        private static int HoarePartition(T[] array, int start, int end, int pivotIndex = -1)
        {
            var pivot = (pivotIndex != -1) ? array[pivotIndex] : array[start + (end - start) / 2];
            int leftIndex = start - 1, rightIndex = end + 1;

            while (true)
            {
                do
                {
                    leftIndex++;
                } while (array[leftIndex].CompareTo(pivot) < 0);

                do
                {
                    rightIndex--;
                } while (array[rightIndex].CompareTo(pivot) > 0);

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

        private static void QuickSortWithLomuto(T[] array, int start, int end)
        {
            if (start < end)
            {
                int pivotIndex = RandomizedLomutoPartition(array, start, end);
                QuickSortWithLomuto(array, start, pivotIndex - 1);
                QuickSortWithLomuto(array, pivotIndex + 1, end);
            }
        }

        private static void QuickSortWithHoare(T[] array, int start, int end)
        {
            if (start < end)
            {
                int pivotIndex = HoarePartition(array, start, end,
                    new Random(DateTime.Now.Millisecond).Next(start, end + 1));
                QuickSortWithHoare(array, start, pivotIndex);
                QuickSortWithHoare(array, pivotIndex + 1, end);
            }
        }

        public static void Sort(T[] array)
        {
            //QuickSortWithLomuto(array, 0, array.Length - 1);
            QuickSortWithHoare(array, 0, array.Length - 1);
        }

        private static void Swap(ref T item1, ref T item2)
        {
            var temp = item1;
            item1 = item2;
            item2 = temp;
        }

    }
}