using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;
using Trees;

namespace Tests
{
    [TestClass]
    public class AVLTreeTests
    {

        private int GetNumber()
        {
            return 1000000;
        }

        private IEnumerable<int> DoRandomValues(int count)
        {
            var random = new Random(DateTime.Now.Millisecond);
            var list = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(random.Next());
            }
            return list;
        }

        [TestMethod]
        public void TestIndexerByKey()
        {
            int n = GetNumber();
            var randomValues = DoRandomValues(n);
            var avl = new AVLTree<int, int>();

            foreach (var value in randomValues)
            {
                if (!avl.ContainsKey(value))
                {
                    avl.Add(value, value);
                }
            }

            bool flag = true;

            foreach (var value in randomValues)
            {
                flag = flag && (avl[value] == value);
            }

            Assert.AreEqual(true, flag);
        }

        [TestMethod]
        public void TestAdd()
        {
            int n = GetNumber();
            var bin = new AVLTree<int, int>();
            var randomValues = DoRandomValues(n);

            foreach (var value in randomValues)
            {
                if (!bin.ContainsKey(value))
                {
                    bin.Add(value, value);
                }
            }

            bool flag = true;

            foreach (var value in randomValues)
            {
                flag = flag && bin.Contains(new KeyValuePair<int, int>(value, value));
            }

            Assert.AreEqual(true, flag);
        }

        [TestMethod]
        public void TestCountWhenAdd()
        {
            int n = GetNumber();
            var bin = new AVLTree<int, int>();
            for (int i = 0; i < n; i++)
            {
                bin.Add(i, i);
            }
            Assert.AreEqual(n, bin.Count);
        }

        [TestMethod]
        public void TestCountWhenRemove()
        {
            int n = 10000, r = 1000;
            var bin = new AVLTree<int, int>();
            for (int i = 0; i < n; i++)
            {
                bin.Add(i, i);
            }
            for (int i = 0; i < r; i++)
            {
                bin.Remove(i);
            }
            Assert.AreEqual(n - r, bin.Count);
        }

        [TestMethod]
        public void TestRemoveWhenRightIsNull()
        {
            var bin = new AVLTree<int, int>
            {
                new KeyValuePair<int, int>(8,1),
                new KeyValuePair<int, int>(10,1),
                new KeyValuePair<int, int>(5,1),
                new KeyValuePair<int, int>(2,10),
            };
            bin.Remove(5);
            CollectionAssert.AreEqual(new int[] { 2, 8, 10 }, (ICollection)bin.Keys);
        }

        [TestMethod]
        public void TestRemoveWhenRightLeftIsNull()
        {
            var bin = new AVLTree<int, int>
            {
                new KeyValuePair<int, int>(8,1),
                new KeyValuePair<int, int>(10,1),
                new KeyValuePair<int, int>(5,1),
                new KeyValuePair<int, int>(2,10),
                new KeyValuePair<int, int>(6,10),
                new KeyValuePair<int, int>(7,10)
            };
            bin.Remove(5);
            CollectionAssert.AreEqual(new int[] { 2, 6, 7, 8, 10 }, (ICollection)bin.Keys);
        }


        [TestMethod]
        public void TestRemove()
        {
            var bin = new AVLTree<int, int>
            {
                { 8, 1 },
                { 10, 1 },
                { 5, 1 },
                { 2, 1 },
                { 7, 1 },
                { 6, 1 }
            };
            bin.Remove(5);
            CollectionAssert.AreEqual(new int[] { 2, 6, 7, 8, 10 }, (ICollection)bin.Keys);
        }



        [TestMethod]
        public void TestTraversal()
        {
            int n = GetNumber();
            var random = new Random(DateTime.Now.Millisecond);
            var sortDict = new SortedDictionary<int, int>();
            var bin = new AVLTree<int, int>();
            for (int i = 0; i < n; i++)
            {
                var value = random.Next();
                if (!sortDict.ContainsKey(value))
                {
                    sortDict.Add(value, value);
                }
                if (!bin.ContainsKey(value))
                {
                    bin.Add(value, value);
                }
            }
            CollectionAssert.AreEqual(sortDict.Keys, (ICollection)bin.Keys);
        }

        [TestMethod]
        public void TestAdd1()
        {
            var bin = new AVLTree<int, int>
            {
                { 2, 2 },
                { 1, 1 },
                { 3, 3 },
                { -1, -1 }
            };
            Assert.AreEqual(4, bin.Count);
        }
    }
}
