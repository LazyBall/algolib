using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLibrary.DataStructures;

namespace Tests
{
    [TestClass]
    public class BinarySearchTreeTests
    {
        int n = 10000;
        IDictionaryTests<BinarySearchTree<int, int>> tester = 
            new IDictionaryTests<BinarySearchTree<int, int>>();

        [TestMethod]
        public void TestAddSortedValues()
        {
            var tree = new BinarySearchTree<int, int>();
            var array = new int[n];

            for (int i = 0; i < n; i++)
            {
                array[i] = i;
                tree.Add(i, i);
            }

            CollectionAssert.AreEqual(array, (ICollection)tree.Keys);
        }

        [TestMethod]
        public void TestAddNotRandom()
        {
            var tree = new BinarySearchTree<int, int>()
            {
                { 2, 2 },
                { 1, 1 },
                { 3, 3 },
                { -1, -1 }
            };
            CollectionAssert.AreEqual(new int[] { -1, 1, 2, 3 }, (ICollection)tree.Keys);
        }

        [TestMethod]
        public void TestTraversal()
        {
            var randomValues = tester.GenerateRandomUniqueValues(n);
            var sortDict = new SortedDictionary<int, int>();
            var tree = new BinarySearchTree<int, int>();

            foreach (var value in randomValues)
            {
                if (!sortDict.ContainsKey(value))
                {
                    sortDict.Add(value, value);
                }
                if (!tree.ContainsKey(value))
                {
                    tree.Add(value, value);
                }
            }

            CollectionAssert.AreEqual(sortDict.Keys, (ICollection)tree.Keys);
        }

        [TestMethod]
        public void TestKeysAfterRemove()
        {
            var tree = new BinarySearchTree<int, int>()
            {
                { 8, 1 },
                { 10, 1 },
                { 5, 1 },
                { 2, 1 },
                { 7, 1 },
                { 6, 1 }
            };
            tree.Remove(5);
            CollectionAssert.AreEqual(new int[] { 2, 6, 7, 8, 10 }, (ICollection)tree.Keys);
        }

        [TestMethod]
        public void TestRemoveWhenRightIsNull()
        {
            var tree = new BinarySearchTree<int, int>()
            {
                new KeyValuePair<int, int>(8,1),
                new KeyValuePair<int, int>(10,1),
                new KeyValuePair<int, int>(5,1),
                new KeyValuePair<int, int>(2,10),
            };
            tree.Remove(5);
            CollectionAssert.AreEqual(new int[] { 2, 8, 10 }, (ICollection)tree.Keys);
        }

        [TestMethod]
        public void TestRemoveWhenRightLeftIsNull()
        {
            var tree = new BinarySearchTree<int, int>()
            {
                new KeyValuePair<int, int>(8,1),
                new KeyValuePair<int, int>(10,1),
                new KeyValuePair<int, int>(5,1),
                new KeyValuePair<int, int>(2,10),
                new KeyValuePair<int, int>(6,10),
                new KeyValuePair<int, int>(7,10)
            };
            tree.Remove(5);
            CollectionAssert.AreEqual(new int[] { 2, 6, 7, 8, 10 }, (ICollection)tree.Keys);
        }

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