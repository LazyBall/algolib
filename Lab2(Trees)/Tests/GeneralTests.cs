using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Tests
{
    public class GeneralTests <T> where T : IDictionary<int, int>, new()
    {

        private  IEnumerable<int> DoRandomValues(int count)
        {
            var random = new Random(DateTime.Now.Millisecond);
            var list = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(random.Next());
            }
            return list;
        }

        public  void TestContainsKey(int n)
        {
            var uniqueValues = new HashSet<int>(DoRandomValues(n));
            var tree = new T();

            foreach (var value in uniqueValues)
            {
                tree.Add(value, value);
            }

            bool flag = true;

            foreach (var value in uniqueValues)
            {
                flag = flag && (tree.ContainsKey(value));
            }

            Assert.AreEqual(true, flag);
        }

        public  void TestIndexerByKey(int n)
        {
            var uniqueValues = new HashSet<int>(DoRandomValues(n));
            var tree = new T();

            foreach (var value in uniqueValues)
            {
                    tree.Add(value, value);               
            }

            bool flag = true;

            foreach (var value in uniqueValues)
            {
                flag = flag && (tree[value] == value);
            }

            Assert.AreEqual(true, flag);
        }

        public  void TestAdd(int n)
        {
            var tree = new T();
            var uniqueValues = new HashSet<int>(DoRandomValues(n));

            foreach (var value in uniqueValues)
            {
                    tree.Add(value, value);                
            }

            bool flag = true;

            foreach (var value in uniqueValues)
            {
                flag = flag && tree.Contains(new KeyValuePair<int, int>(value, value));
            }

            Assert.AreEqual(true, flag);
        }

        public  void TestCountWhenAdd(int n)
        {
            var tree = new T();
            for (int i = 0; i < n; i++)
            {
                tree.Add(i, i);
            }
            Assert.AreEqual(n, tree.Count);
        }

        public  void TestCountWhenRemove(int countAdd, int countRemove)
        {
            var tree = new T();
            for (int i = 0; i < countAdd; i++)
            {
                tree.Add(i, i);
            }
            for (int i = 0; i < countRemove; i++)
            {
                tree.Remove(i);
            }
            Assert.AreEqual(countAdd - countRemove, tree.Count);
        }

        public  void TestRemoveWhenRightIsNull()
        {
            var tree = new T
            {
                new KeyValuePair<int, int>(8,1),
                new KeyValuePair<int, int>(10,1),
                new KeyValuePair<int, int>(5,1),
                new KeyValuePair<int, int>(2,10),
            };
            tree.Remove(5);
            CollectionAssert.AreEqual(new int[] { 2, 8, 10 }, (ICollection)tree.Keys);
        }

        public  void TestRemoveWhenRightLeftIsNull()
        {
            var tree = new T
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

        public  void TestRemove()
        {
            var tree = new T
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

        public  void TestTraversal(int n)
        {
            var random = new Random(DateTime.Now.Millisecond);
            var sortDict = new SortedDictionary<int, int>();
            var tree = new T();
            for (int i = 0; i < n; i++)
            {
                var value = random.Next();
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

        public  void TestAddNotRandom()
        {
            var tree = new T
            {
                { 2, 2 },
                { 1, 1 },
                { 3, 3 },
                { -1, -1 }
            };
            CollectionAssert.AreEqual(new int[]{ -1,1,2,3}, (ICollection) tree.Keys);
        }


        public void TestAddIncreasedEnum(int n)
        {
            var tree = new T();
            var array = new int[n];
            for (int i = 0; i < n; i++)
            {
                array[i] = i;
                tree.Add(i, i);
            }
            CollectionAssert.AreEqual(array, (ICollection)tree.Keys);
        }
    }
}