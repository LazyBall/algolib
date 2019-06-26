using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLibrary.Algorithms.Sorting;

namespace Tests
{
    [TestClass]
    public class MergeSortTests
    {
        int n = 10000;
        SortTests tester = new SortTests();

        [TestMethod]
        public void EasyTest()
        {
            Assert.AreEqual(true, tester.EasyTest(MergeSort<int>.Sort));
        }

        [TestMethod]
        public void TestRandomValues()
        {
            Assert.AreEqual(true, tester.TestRandomValues(MergeSort<int>.Sort, n));
        }

        [TestMethod]
        public void TestSortedArray()
        {
            Assert.AreEqual(true, tester.TestSortedArray(MergeSort<int>.Sort, n));
        }

        [TestMethod]
        public void TestSortedByDescendingArray()
        {
            Assert.AreEqual(true, tester.TestSortedByDescendingArray(MergeSort<int>.Sort, n));
        }

        [TestMethod]
        public void TestSameValueArray()
        {
            Assert.AreEqual(true, tester.TestSameValueArray(MergeSort<int>.Sort, n));
        }
    }
}