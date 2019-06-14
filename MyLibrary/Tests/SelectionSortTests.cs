using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLibrary.Algorithms.Sorting;

namespace Tests
{    
    [TestClass]
    public class SelectionSortTests
    {
        int n = 10000;
        SortTests tester = new SortTests();

        [TestMethod]
        public void EasyTest()
        {
            Assert.AreEqual(true, tester.EasyTest(SelectionSort<int>.Sort));
        }

        [TestMethod]
        public void TestRandomValues()
        {
            Assert.AreEqual(true, tester.TestRandomValues(SelectionSort<int>.Sort, n));
        }

        [TestMethod]
        public void TestSortedArray()
        {
            Assert.AreEqual(true, tester.TestSortedArray(SelectionSort<int>.Sort, n));
        }

        [TestMethod]
        public void TestSortedByDescendingArray()
        {
            Assert.AreEqual(true, tester.TestSortedByDescendingArray(SelectionSort<int>.Sort, n));
        }

        [TestMethod]
        public void TestSameValueArray()
        {
            Assert.AreEqual(true, tester.TestSameValueArray(SelectionSort<int>.Sort, n));
        }
    }
}