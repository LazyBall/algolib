using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLibrary.Algorithms;

namespace Tests
{
    [TestClass]
    public class HeapSortTests
    {
        int n = 10000;

        int[] GenerateRandomValues(int count)
        {
            var array = new int[count];
            var random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next();
            }

            return array;
        }

        int[] GenerateSortedArray(int count)
        {
            var array = new int[count];

            for (int i = 0; i < count; i++)
            {
                array[i] = i;
            }

            return array;
        }

        int[] GeneratedSortedByDescendingArray(int count)
        {
            var array = new int[count];

            for (int i = count - 1; i >= 0; i--)
            {
                array[i] = i;
            }

            return array;
        }

        int[] GenerateSameValueArray(int count)
        {
            var array = new int[count];

            for (int i = 0; i < count; i++)
            {
                array[i] = count;
            }

            return array;
        }

        [TestMethod]
        public void TestRandomValues()
        {
            var arr1 = GenerateRandomValues(n);
            var arr2 = new int[arr1.Length];
            Array.Copy(arr1, arr2, arr1.Length);

            Array.Sort(arr1);
            HeapSort<int>.Sort(arr2);

            CollectionAssert.AreEqual(arr1, arr2);
        }

        [TestMethod]
        public void TestSortedArray()
        {
            var arr1 = GenerateSortedArray(n);
            var arr2 = new int[arr1.Length];
            Array.Copy(arr1, arr2, arr1.Length);

            Array.Sort(arr1);
            HeapSort<int>.Sort(arr2);

            CollectionAssert.AreEqual(arr1, arr2);
        }

        [TestMethod]
        public void TestSortedByDescendingArray()
        {
            var arr1 = GeneratedSortedByDescendingArray(n);
            var arr2 = new int[arr1.Length];
            Array.Copy(arr1, arr2, arr1.Length);

            Array.Sort(arr1);
            HeapSort<int>.Sort(arr2);

            CollectionAssert.AreEqual(arr1, arr2);
        }

        [TestMethod]
        public void TestSameValueArray()
        {
            var arr1 = GenerateSameValueArray(n);
            var arr2 = new int[arr1.Length];
            Array.Copy(arr1, arr2, arr1.Length);

            Array.Sort(arr1);
            HeapSort<int>.Sort(arr2);

            CollectionAssert.AreEqual(arr1, arr2);
        }
    }
}