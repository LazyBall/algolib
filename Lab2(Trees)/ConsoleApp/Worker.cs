using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    class Worker
    {
        public int[] CreateRandomArray(int length)
        {
            var random = new Random(DateTime.Now.Millisecond);
            int[] array = new int[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = random.Next();
            }
            return array;
        }

        public void TestTree<T>(int[] array, int startDelete, int stopDelete)
            where T : IDictionary<int, int>, new()
        {
            var dict = new T();
            AddElements(dict, array);
            RemoveElements(dict, array, startDelete, stopDelete);
            FindElements(dict, array);
        }

        private void AddElements(IDictionary<int, int> dict, int[] array)
        {
            foreach (var elem in array)
            {
                if (!dict.ContainsKey(elem))
                {
                    dict.Add(elem, elem);
                }
            }
        }

        private void RemoveElements(IDictionary<int, int> dict, int[] array, int startDelete,
            int stopDelete)
        {
            for (int i = startDelete; i < stopDelete; i++)
            {
                dict.Remove(array[i]);
            }
        }

        private void FindElements(IDictionary<int, int> dict, int[] array)
        {
            foreach(var element in array)
            {
                dict.ContainsKey(element);
            }
        }        
    }
}