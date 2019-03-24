using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Trees;

namespace Tests
{
    [TestClass]
    public class AVLTreeTests
    {
        GeneralTests<AVLTree<int, int>> general = new GeneralTests<AVLTree<int, int>>();
        readonly int n = 10000;

        [TestMethod]
        public void TestContainsKey()
        {
            general.TestContainsKey(n);
        }

        [TestMethod]
        public void TestIndexerByKey()
        {
            general.TestIndexerByKey(n);
        }

        [TestMethod]
        public void TestAdd()
        {
            general.TestAdd(n);
        }

        [TestMethod]
        public void TestCountWhenAdd()
        {
            general.TestCountWhenAdd(n);
        }

        [TestMethod]
        public void TestCountWhenRemove()
        {
            general.TestCountWhenRemove(n, n / 2);
        }

        [TestMethod]
        public void TestRemoveWhenRightIsNull()
        {
            general.TestRemoveWhenRightIsNull();
        }

        [TestMethod]
        public void TestRemoveWhenRightLeftIsNull()
        {
            general.TestRemoveWhenRightLeftIsNull();
        }


        [TestMethod]
        public void TestRemove()
        {
            general.TestRemove();
        }

        [TestMethod]
        public void TestTraversal()
        {
            general.TestTraversal(n);
        }

        [TestMethod]
        public void TestAddNotRandom()
        {
            general.TestAddNotRandom();
        }

        [TestMethod]
        public void TestAddIncreasedEnum()
        {
            general.TestAddSortedValues(n);
        }

        [TestMethod]
        public void TestAfterRemove()
        {
            general.TestAfterRemove(n);
        }

        [TestMethod]
        public void TestHeight()
        {
            var tree = new AVLTree<int, int>();
            var coef = 1.45 / Math.Log(2);
            bool flag = true;
            for(int i=-n; i<n; i++)
            {
                tree.Add(i, i);
                flag = flag && (tree.Height <= Math.Log(tree.Count + 2) * coef);
            }
            Assert.AreEqual(true, flag);
        }
    }
}