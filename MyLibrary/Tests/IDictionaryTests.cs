using System;
using System.Collections.Generic;

namespace Tests
{
    public class IDictionaryTests<T> where T : IDictionary<int, int>, new()
    {

        public int[] GenerateRandomValues(int count)
        {
            var array = new int[count];
            var random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next();
            }

            return array;
        }

        public int[] GenerateRandomUniqueValues(int count)
        {
            var array = new int[count];
            var set = new HashSet<int>();
            var random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < array.Length; i++)
            {
                int value;

                do
                {
                    value = random.Next();
                } while (!set.Add(value));

                array[i] = value;
            }


            return array;
        }

        public bool TestIndexerByKey(int n)
        {
            var uniqueValues = GenerateRandomUniqueValues(n);
            var dict = new T();

            foreach (var value in uniqueValues)
            {
                dict.Add(value, value);
            }

            bool flag = true;

            foreach (var value in uniqueValues)
            {
                flag = flag && (dict[value] == value);
            }

            return flag;
        }

        public bool TestKeysProperty(int n)
        {
            var uniqueValues = GenerateRandomUniqueValues(n);
            var dict = new T();

            foreach (var value in uniqueValues)
            {
                dict.Add(value, value);
            }

            var set = new HashSet<int>(dict.Keys);
            bool flag = true;

            foreach (var value in uniqueValues)
            {
                flag = flag && set.Contains(value);
            }

            return flag;

        }

        public bool TestValuesProperty(int n)
        {
            var uniqueValues = GenerateRandomUniqueValues(n);
            var dict = new T();

            foreach (var value in uniqueValues)
            {
                dict.Add(value, value + 1);
            }

            var set = new HashSet<int>(dict.Values);
            bool flag = true;

            foreach (var value in uniqueValues)
            {
                flag = flag && set.Contains(value + 1);
            }

            return flag;
        }

        public bool TestCountWhenAdd(int n)
        {
            var dict = new T();

            for (int i = 0; i < n; i++)
            {
                dict.Add(i, i);
            }

            return (dict.Count == n);
        }

        public bool TestCountWhenRemove(int countAdd, int countRemove)
        {
            var dict = new T();

            for (int i = 0; i < countAdd; i++)
            {
                dict.Add(i, i);
            }

            for (int i = 0; i < countRemove; i++)
            {
                dict.Remove(i);
            }

            return (countAdd - countRemove == dict.Count);

        }

        public bool TestAdd(int n)
        {
            var dict = new T();
            var uniqueValues = GenerateRandomUniqueValues(n);

            foreach (var value in uniqueValues)
            {
                dict.Add(value, value);
            }

            bool flag = true;

            foreach (var value in uniqueValues)
            {
                flag = flag && dict.Contains(new KeyValuePair<int, int>(value, value));
            }

            return flag;
        }

        public bool TestRemove(int n)
        {
            var uniqueValues = GenerateRandomUniqueValues(n);
            var dict = new T();
            var set = new HashSet<int>();

            foreach (var value in uniqueValues)
            {
                set.Add(value);
                dict.Add(value, value);
            }

            for (int i = n / 4; i < 3 * n / 4; i++)
            {
                set.Remove(uniqueValues[i]);
                dict.Remove(uniqueValues[i]);
            }

            bool flag = true;

            foreach (var value in uniqueValues)
            {
                flag = flag && (set.Contains(value) == dict.ContainsKey(value));
            }

            return flag;
        }

        public bool TestClear()
        {
            var dict = new T();

            for (int i = 0; i < 100; i++)
            {
                dict.Add(i, i);
            }

            dict.Clear();
            bool flag = true;
            flag = flag && (dict.Count == 0);
            flag = flag && (!dict.ContainsKey(0));
            flag = flag && (!dict.Remove(0));
            flag = flag && (dict.Keys.Count == 0);
            flag = flag && (dict.Values.Count == 0);
            flag = flag && (!dict.TryGetValue(0, out int val));
            return flag;

        }

        public bool TestContains(int n)
        {
            var values = GenerateRandomValues(n);
            var set = new HashSet<int>(values);
            var dict = new T();

            foreach (var value in values)
            {
                if (!dict.ContainsKey(value))
                {
                    dict.Add(value, value);
                }
            }

            bool flag = (dict.Count == set.Count);

            foreach (var value in values)
            {
                flag = flag && (dict.ContainsKey(value));
            }

            return flag;
        }

    }
}