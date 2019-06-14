using System;
using System.Linq;

namespace Tests
{

    public class SortTests
    {

        private int[] GenerateRandomValues(int count)
        {
            var array = new int[count];
            var random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next();
            }

            return array;
        }

        private int[] GenerateSortedArray(int count)
        {
            var array = new int[count];

            for (int i = 0; i < count; i++)
            {
                array[i] = i;
            }

            return array;
        }

        private int[] GeneratedSortedByDescendingArray(int count)
        {
            var array = new int[count];

            for (int i = 0; i < count; i++)
            {
                array[i] = count - i;
            }

            return array;
        }

        private int[] GenerateSameValueArray(int count)
        {
            var array = new int[count];

            for (int i = 0; i < count; i++)
            {
                array[i] = count;
            }

            return array;
        }

        public bool EasyTest(Action<int[]> sort)
        {
            var arr1 = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            var arr2 = new int[arr1.Length];
            Array.Copy(arr1, arr2, arr1.Length);
            Array.Sort(arr1);
            sort(arr2);
            return arr1.SequenceEqual(arr2);
        }

        public bool TestRandomValues(Action<int[]> sort, int n)
        {
            var arr1 = GenerateRandomValues(n);
            var arr2 = new int[arr1.Length];
            Array.Copy(arr1, arr2, arr1.Length);
            Array.Sort(arr1);
            sort(arr2);
            return arr1.SequenceEqual(arr2);
        }

        public bool TestSortedArray(Action<int[]> sort, int n)
        {
            var arr1 = GenerateSortedArray(n);
            var arr2 = new int[arr1.Length];
            Array.Copy(arr1, arr2, arr1.Length);
            Array.Sort(arr1);
            sort(arr2);
            return arr1.SequenceEqual(arr2);
        }

        public bool TestSortedByDescendingArray(Action<int[]> sort, int n)
        {
            var arr1 = GeneratedSortedByDescendingArray(n);
            var arr2 = new int[arr1.Length];
            Array.Copy(arr1, arr2, arr1.Length);
            Array.Sort(arr1);
            sort(arr2);
            return arr1.SequenceEqual(arr2);
        }

        public bool TestSameValueArray(Action<int[]> sort, int n)
        {
            var arr1 = GenerateSameValueArray(n);
            var arr2 = new int[arr1.Length];
            Array.Copy(arr1, arr2, arr1.Length);
            Array.Sort(arr1);
            sort(arr2);
            return arr1.SequenceEqual(arr2);
        }

    }

}