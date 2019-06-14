using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLibrary.Algorithms.Sorting;

namespace Tests
{

    [TestClass]
    public class InsertionSortTests
    {

        int n = 10000;
        SortTests tester = new SortTests();

        [TestMethod]
        public void EasyTest()
        {
            Assert.AreEqual(true, tester.EasyTest(InsertionSort<int>.Sort));
        }

        [TestMethod]
        public void TestRandomValues()
        {
            Assert.AreEqual(true, tester.TestRandomValues(InsertionSort<int>.Sort, n));
        }

        [TestMethod]
        public void TestSortedArray()
        {
            Assert.AreEqual(true, tester.TestSortedArray(InsertionSort<int>.Sort, n));
        }

        [TestMethod]
        public void TestSortedByDescendingArray()
        {
            Assert.AreEqual(true, tester.TestSortedByDescendingArray(InsertionSort<int>.Sort, n));
        }

        [TestMethod]
        public void TestSameValueArray()
        {
            Assert.AreEqual(true, tester.TestSameValueArray(InsertionSort<int>.Sort, n));
        }

    }

}