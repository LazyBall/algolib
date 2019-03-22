using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleApp
{
    class Worker
    {
        public int[] CreateRandomValues(int length)
        {
            var random = new Random(DateTime.Now.Millisecond);
            int[] array = new int[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = random.Next();
            }
            return array;
        }

        public int[] CreateSortedValues(int length)
        {            
            int[] array = new int[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = i;
            }
            return array;
        }

        public void TestAllOperations<T>(int[] array, int startDelete, int stopDelete)
            where T : IDictionary<int, int>, new()
        {
            var dict = new T();
            AddElements(dict, array);
            RemoveElements(dict, array, startDelete, stopDelete);
            FindElements(dict, array);
        }

        public void AddElements(IDictionary<int, int> dict, int[] array)
        {
            foreach (var elem in array)
            {
                if (!dict.ContainsKey(elem))
                {
                    dict.Add(elem, elem);
                }
            }
        }

        public void RemoveElements(IDictionary<int, int> dict, int[] array, int startDelete,
            int stopDelete)
        {
            for (int i = startDelete; i < stopDelete; i++)
            {
                dict.Remove(array[i]);
            }
        }

        public void FindElements(IDictionary<int, int> dict, int[] array)
        {
            foreach(var element in array)
            {
                dict.ContainsKey(element);
            }
        }

        public void RunBenchmark<T>(int[] array,
           int startDel, int stopDel) where T : IDictionary<int, int>, new()
        {
            var watch = new Stopwatch();
            long time = 0;
            var dict = new T();
            var worker = new Worker();
            watch.Start();
            worker.AddElements(dict, array);
            watch.Stop();
            time += watch.ElapsedMilliseconds;
            Console.Write($"Add {watch.ElapsedMilliseconds} ms \t");
            watch.Restart();
            worker.RemoveElements(dict, array, startDel, stopDel);
            watch.Stop();
            time += watch.ElapsedMilliseconds;
            Console.Write($"Remove {watch.ElapsedMilliseconds} ms \t");
            worker.FindElements(dict, array);
            watch.Restart();
            worker.FindElements(dict, array);
            watch.Stop();
            time += watch.ElapsedMilliseconds;
            Console.Write($"Find {watch.ElapsedMilliseconds} ms \t");
            Console.WriteLine($"In sum: {time} ms");
        }
    }
}