using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;
using Trees;

namespace Tests
{
    [TestClass]
    public class BinarySearchTreeTests
    {
        [TestMethod]
        public void TestCountWhenAdd()
        {
            int n = 1000;
            var bin = new BinarySearchTree<int, int>();
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
            var bin = new BinarySearchTree<int, int>();
            for (int i = 0; i < n; i++)
            {
                bin.Add(i, i);
            }
            for (int i = 0; i < r; i++)
            {
                bin.Remove(i);
            }
            Assert.AreEqual(n-r, bin.Count);
        }

        [TestMethod]
        public void TestRemoveWhenRightIsNull()
        {
            var bin = new BinarySearchTree<int, int>
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
            var bin = new BinarySearchTree<int, int>
            {
                new KeyValuePair<int, int>(8,1),
                new KeyValuePair<int, int>(10,1),
                new KeyValuePair<int, int>(5,1),
                new KeyValuePair<int, int>(2,10),
                new KeyValuePair<int, int>(6,10),
                new KeyValuePair<int, int>(7,10)
            };
            bin.Remove(5);
            CollectionAssert.AreEqual(new int[] { 2,6,7, 8, 10 }, (ICollection)bin.Keys);
        }


        [TestMethod]
        public void TestRemove()
        {
            var bin = new BinarySearchTree<int, int>
            {
                new KeyValuePair<int, int>(8,1),
                new KeyValuePair<int, int>(10,1),
                new KeyValuePair<int, int>(5,1),
                new KeyValuePair<int, int>(2,10),
                new KeyValuePair<int, int>(7,10),
                new KeyValuePair<int, int>(6,10)             
            };
            bin.Remove(5);
            CollectionAssert.AreEqual(new int[] { 2, 6, 7, 8, 10 }, (ICollection)bin.Keys);
        }

        [TestMethod]
        public void TestAdd()
        {
            int n = 1000000;
            var random = new Random(DateTime.Now.Millisecond);
            var hash = new HashSet<int>();
            var bin = new BinarySearchTree<int, int>();
            for(int i=0; i<n; i++)
            {
                var key = random.Next();
                hash.Add(key);
                if(!bin.ContainsKey(key))
                {
                    bin.Add(key, key);
                }
            }
            foreach(var key in hash)
            {
                bin.Remove(key);
            }
            CollectionAssert.AreEqual(new List<int>(), (ICollection)bin.Keys);
        }
    }
}