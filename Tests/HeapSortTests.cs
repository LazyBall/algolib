using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLibrary.Algorithms.Sorting;

namespace Tests
{
    [TestClass]
    public class HeapSortTests
    {
        int n = 10000;
        SortTests tester = new SortTests();

        [TestMethod]
        public void EasyTest()
        {
            Assert.AreEqual(true, tester.EasyTest(HeapSort<int>.Sort));
        }

        [TestMethod]
        public void TestRandomValues()
        {
            Assert.AreEqual(true, tester.TestRandomValues(HeapSort<int>.Sort, n));
        }

        [TestMethod]
        public void TestSortedArray()
        {
            Assert.AreEqual(true, tester.TestSortedArray(HeapSort<int>.Sort, n));
        }

        [TestMethod]
        public void TestSortedByDescendingArray()
        {
            Assert.AreEqual(true, tester.TestSortedByDescendingArray(HeapSort<int>.Sort, n));
        }

        [TestMethod]
        public void TestSameValueArray()
        {
            Assert.AreEqual(true, tester.TestSameValueArray(HeapSort<int>.Sort, n));
        }
    }
}