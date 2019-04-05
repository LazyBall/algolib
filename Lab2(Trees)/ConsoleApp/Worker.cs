using System;
using System.Collections.Generic;
using System.Diagnostics;
using Trees;

namespace ConsoleApp
{
    class Worker
    {
        class BenchmarkResult
        {
            public long AddTimeInMs { get; set; }
            public long RemoveTimeInMs { get; set; }
            public long FindTimeInMs { get; set; }

            public void Add(BenchmarkResult result)
            {
                this.AddTimeInMs += result.AddTimeInMs;
                this.RemoveTimeInMs += result.RemoveTimeInMs;
                this.FindTimeInMs += result.FindTimeInMs;
            }
        }

        int[] CreateRandomUniqueValues(int length)
        {
            var random = new Random(DateTime.Now.Millisecond);
            int[] array = new int[length];
            var hashset = new HashSet<int>();
            for (int i = 0; i < length; i++)
            {
                var value = random.Next();
                while (hashset.Contains(value))
                {
                    value = random.Next();
                }
                hashset.Add(value);
                array[i] = value;
            }
            return array;
        }

        int[] CreateSortedUniqueValues(int length)
        {
            int[] array = new int[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = i;
            }
            return array;
        }

        void AddElements(IDictionary<int, int> dict, int[] arrayOfUniqueValues)
        {
            foreach (var elem in arrayOfUniqueValues)
            {
                dict.Add(elem, elem);
            }
        }

        void RemoveElements(IDictionary<int, int> dict, int[] arrayOfUniqueValues, int startDelete,
            int stopDelete)
        {
            for (int i = startDelete; i < stopDelete; i++)
            {
                dict.Remove(arrayOfUniqueValues[i]);
            }
        }

        void FindElements(IDictionary<int, int> dict, int[] array)
        {
            foreach (var element in array)
            {
                var flag = dict.ContainsKey(element);
            }
        }

        BenchmarkResult RunBenchmark<T>(int[] array,
           int startDel, int stopDel) where T : IDictionary<int, int>, new()
        {
            var dict = new T();
            var worker = new Worker();
            var watch = new Stopwatch();
            var result = new BenchmarkResult();

            watch.Start();
            worker.AddElements(dict, array);
            watch.Stop();
            result.AddTimeInMs = watch.ElapsedMilliseconds;

            watch.Restart();
            worker.RemoveElements(dict, array, startDel, stopDel);
            watch.Stop();
            result.RemoveTimeInMs = watch.ElapsedMilliseconds;

            watch.Restart();
            worker.FindElements(dict, array);
            watch.Stop();
            result.FindTimeInMs = watch.ElapsedMilliseconds;

            return result;
        }

        void RunTest(int[] array, int startDel, int stopDel, BenchmarkResult totalResultSD = null,
            BenchmarkResult totalResultAVL = null, BenchmarkResult totalResultBin = null)
        {
            Console.Write("SortedDictionary:\t");
            var result = RunBenchmark<SortedDictionary<int, int>>(array, startDel, stopDel);
            totalResultSD?.Add(result);
            Console.WriteLine(CreateTestInfo(result));

            Console.Write("AVLTree:\t\t");
            result = RunBenchmark<AVLTree<int, int>>(array, startDel, stopDel);
            totalResultAVL?.Add(result);
            Console.WriteLine(CreateTestInfo(result));

            Console.Write("BinarySearchTree:\t");
            result = RunBenchmark<BinarySearchTree<int, int>>(array, startDel, stopDel);
            totalResultBin?.Add(result);
            Console.WriteLine(CreateTestInfo(result));

            Console.Write("Dimas:\t\t\t");
            result = RunBenchmark<DimasAVLTree<int, int>>(array, startDel, stopDel);
            Console.WriteLine(CreateTestInfo(result));
            Console.WriteLine();
        }

        string CreateTestInfo(BenchmarkResult result)
        {
            return
                $"Add: {result.AddTimeInMs} ms\t" +
                $"Remove: {result.RemoveTimeInMs} ms\t" +
                $"Find: {result.FindTimeInMs} ms\t" +
                $"Total: {result.AddTimeInMs + result.RemoveTimeInMs + result.FindTimeInMs} ms";
        }

        BenchmarkResult CreateAverageValue(BenchmarkResult totalResult, int numberOfTests)
        {
            return new BenchmarkResult()
            {
                AddTimeInMs = totalResult.AddTimeInMs / numberOfTests,
                RemoveTimeInMs = totalResult.RemoveTimeInMs / numberOfTests,
                FindTimeInMs = totalResult.FindTimeInMs / numberOfTests
            };
        }

        public void ShowInfo(int lengthOfArray, int startDel, int stopDel, int numberOfTests)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            var totalResultSD = new BenchmarkResult();
            var totalResultAVL = new BenchmarkResult();
            var totalResultBin = new BenchmarkResult();
            Console.WriteLine("---------------------------Random Data-----------------------------");

            for (int i = 0; i < numberOfTests; i++)
            {
                var array = CreateRandomUniqueValues(lengthOfArray);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Test: {0}", i + 1);
                Console.ForegroundColor = ConsoleColor.White;
                RunTest(array, startDel, stopDel, totalResultSD, totalResultAVL, totalResultBin);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Average: ");
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("SortedDictionary: \t");
            Console.WriteLine(CreateTestInfo(CreateAverageValue(totalResultSD, numberOfTests)));

            Console.Write("AVLTree: \t\t");
            Console.WriteLine(CreateTestInfo(CreateAverageValue(totalResultAVL, numberOfTests)));

            Console.Write("BinarySearchTree: \t");
            Console.WriteLine(CreateTestInfo(CreateAverageValue(totalResultBin, numberOfTests)));
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("---------------------------Sorted Data-----------------------------");
            Console.ForegroundColor = ConsoleColor.White;
            RunTest(CreateSortedUniqueValues(lengthOfArray), startDel, stopDel);
        }
    }
}