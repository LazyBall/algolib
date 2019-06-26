using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLibrary.DataStructures;

namespace Tests
{
    [TestClass]
    public class SkipListTests
    {
        int n = 10000;
        IDictionaryTests<SkipList<int, int>> tester = new IDictionaryTests<SkipList<int, int>>();

        [TestMethod]
        public void TestIndexerByKey()
        {
            Assert.AreEqual(true, tester.TestIndexerByKey(n));
        }

        [TestMethod]
        public void TestKeysProperty()
        {
            Assert.AreEqual(true, tester.TestKeysProperty(n));
        }

        [TestMethod]
        public void TestValuesProperty()
        {
            Assert.AreEqual(true, tester.TestValuesProperty(n));
        }

        [TestMethod]
        public void TestCountWhenAdd()
        {
            Assert.AreEqual(true, tester.TestCountWhenAdd(n));

        }

        [TestMethod]
        public void TestCountWhenRemove()
        {
            Assert.AreEqual(true, tester.TestCountWhenRemove(n, n / 2));
        }

        [TestMethod]
        public void TestAdd()
        {
            Assert.AreEqual(true, tester.TestAdd(n));
        }

        [TestMethod]
        public void TestRemove()
        {
            Assert.AreEqual(true, tester.TestRemove(n));
        }

        [TestMethod]
        public void TestClear()
        {
            Assert.AreEqual(true, tester.TestClear());
        }

        [TestMethod]
        public void TestContains()
        {
            Assert.AreEqual(true, tester.TestContains(n));
        }

    }
}