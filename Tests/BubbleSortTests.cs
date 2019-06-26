using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLibrary.Algorithms.Sorting;

namespace Tests
{
    [TestClass]
    public class BubbleSortTests
    {
        int n = 10000;
        SortTests tester = new SortTests();

        [TestMethod]
        public void EasyTest()
        {
            Assert.AreEqual(true, tester.EasyTest(BubbleSort<int>.Sort));
        }

        [TestMethod]
        public void TestRandomValues()
        {
            Assert.AreEqual(true, tester.TestRandomValues(BubbleSort<int>.Sort, n));
        }

        [TestMethod]
        public void TestSortedArray()
        {
            Assert.AreEqual(true, tester.TestSortedArray(BubbleSort<int>.Sort, n));
        }

        [TestMethod]
        public void TestSortedByDescendingArray()
        {
            Assert.AreEqual(true, tester.TestSortedByDescendingArray(BubbleSort<int>.Sort, n));
        }

        [TestMethod]
        public void TestSameValueArray()
        {
            Assert.AreEqual(true, tester.TestSameValueArray(BubbleSort<int>.Sort, n));
        }
    }
}