using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using Trees;

namespace Tests
{
    [TestClass]
    public class BinaryTreeTests
    {
        [TestMethod]
        public void CountIncreaseTest()
        {
            var bin = new BinaryTree<int, int>();
            for(int i=0; i<10; i++)
            {
                bin.Add(i, i);
            }
            Assert.AreEqual(10, bin.Count);
        }

        [TestMethod]
        public void CountRemoveTest()
        {
            var bin = new BinaryTree<int, int>();
            for (int i = 0; i < 10; i++)
            {
                bin.Add(i, i);
            }
            bin.Remove(7);
            Assert.AreEqual(9, bin.Count);
        }


        [TestMethod]
        public void Add1000RandomKeys()
        {
            var bin = new BinaryTree<int, int>();
            var hashset = new HashSet<int>();
            var random = new Random(DateTime.Now.Millisecond);
            for (long i = 0; i < 1000; i++)
            {
                var value = random.Next();
                if(!hashset.Contains(value))
                {
                    bin.Add(value, (int)i);
                    hashset.Add(value);
                }          
            }
            Assert.AreEqual(hashset.Count, bin.Count);
        }

        [TestMethod]
        public void CompareWithSortDict()
        {
            var bin = new BinaryTree<int, int>();
            var sortDict = new SortedDictionary<int, int>();
            var random = new Random(DateTime.Now.Millisecond);
            var watch1 = new Stopwatch();
            var watch2 = new Stopwatch();
            for (long i = 0; i < 100000; i++)
            {
                var value = random.Next();
                watch1.Start();
                if (!bin.ContainsKey(value))
                {
                    bin.Add(value, (int)i);
                    bin.Remove(value);
                }
                watch1.Stop();

                watch2.Start();
                if (!sortDict.ContainsKey(value))
                {
                    sortDict.Add(value, (int)i);
                    sortDict.Remove(value);
                }
                watch2.Stop();

            }
            var result = watch2.ElapsedMilliseconds*3 > watch1.ElapsedMilliseconds;
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void TestingRemove1()
        {
            var bin = new BinaryTree<int, int>
            {
                new KeyValuePair<int, int>(8,1),
                new KeyValuePair<int, int>(10,1),
                new KeyValuePair<int, int>(5,1),
                new KeyValuePair<int, int>(2,10),
            };
            bin.Remove(5);
            CollectionAssert.AreEqual(new int[] { 2, 8, 10 }, (ICollection) bin.Keys);
        }

        [TestMethod]
        public void TestingRemove2()
        {
            var bin = new BinaryTree<int, int>
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
        public void TestingRemove3()
        {
            var bin = new BinaryTree<int, int>
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
    }
}
