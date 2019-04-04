using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HashTable;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class Test
    {
        int n = 10000;

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

        private int[] DoRandomUniqueValues(int count)
        {
            var random = new Random(DateTime.Now.Millisecond);
            var hashset = new HashSet<int>();

            while(hashset.Count<count)
            {
                hashset.Add(random.Next());
            }
            
            return hashset.ToArray();
        }

        [TestMethod]
        public void TestContainsKey()
        {
            var uniqueValues = DoRandomUniqueValues(n);
            var hashTable = new HashTable<int, int>();

            foreach (var value in uniqueValues)
            {
                hashTable.Add(value, value);
            }

            bool flag = true;

            foreach (var value in uniqueValues)
            {
                flag = flag && (hashTable.ContainsKey(value));
            }

            Assert.AreEqual(true, flag);
        }

        [TestMethod]
        public void TestIndexerByKey()
        {
            var uniqueValues = DoRandomUniqueValues(n);
            var hashTable = new HashTable<int,int>();

            foreach (var value in uniqueValues)
            {
                hashTable.Add(value, value);
            }

            bool flag = true;

            foreach (var value in uniqueValues)
            {
                flag = flag && (hashTable[value] == value);
            }

            Assert.AreEqual(true, flag);
        }

        [TestMethod]
        public void TestAdd()
        {
            var hashTable = new HashTable<int,int>();
            var uniqueValues = DoRandomUniqueValues(n);

            foreach (var value in uniqueValues)
            {
                hashTable.Add(value, value);
            }

            bool flag = true;

            foreach (var value in uniqueValues)
            {
                flag = flag && hashTable.Contains(new KeyValuePair<int, int>(value, value));
            }

            Assert.AreEqual(true, flag);
        }

        [TestMethod]
        public void TestCountWhenAdd()
        {
            var hashTable = new HashTable<int, int>();
            for (int i = 0; i < n; i++)
            {
                hashTable.Add(i, i);
            }
            Assert.AreEqual(n, hashTable.Count);
        }

        [TestMethod]
        public void TestCountWhenRemove()
        {
            var hashTable = new HashTable<int,int>();
            for (int i = 0; i < n; i++)
            {
                hashTable.Add(i, i);
            }
            for (int i = 0; i < n/2; i++)
            {
                hashTable.Remove(i);
            }
            Assert.AreEqual(n - n/2, hashTable.Count);
        }       

        [TestMethod]
        public void TestRemove()
        {
            var uniqueValues = DoRandomUniqueValues(n);
            var hashTable = new HashTable<int, int>();
            var dict = new Dictionary<int, int>();

            foreach (var value in uniqueValues)
            {
                dict.Add(value, value);
                hashTable.Add(value, value);
            }

            for (int i = n / 4; i < 3 * n / 4; i++)
            {
                dict.Remove(uniqueValues[i]);
                hashTable.Remove(uniqueValues[i]);
            }

            bool flag = true;
            foreach (var value in uniqueValues)
            {
                flag = flag && (dict.ContainsKey(value) == hashTable.ContainsKey(value));
            }

            Assert.AreEqual(true, flag);
        }

        [TestMethod]
        public void TestIEnumerator()
        {
            var uniqueValues = DoRandomUniqueValues(n);
            var set = new HashSet<int>(uniqueValues);
            var hashTable = new HashTable<int, int>();
            bool flag = true;
            foreach(var value in uniqueValues)
            {
                hashTable.Add(value, value);
            }

            int i = 0;
            foreach(var key in hashTable.Keys)
            {
                flag = flag & set.Contains(key);
                i++;
            }
            flag = flag & (set.Count == i);

            Assert.AreEqual(true, flag);
        }
    }
}