using System;
using System.Collections.Generic;
using System.Diagnostics;
using MyLibrary.DataStructures;

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

            public override string ToString()
            {
                return
                $"Add: {AddTimeInMs} ms\t" +
                $"Remove: {RemoveTimeInMs} ms\t" +
                $"Find: {FindTimeInMs} ms\t" +
                $"Total: {AddTimeInMs + RemoveTimeInMs + FindTimeInMs} ms";
            }
        }

        int[] CreateRandomUniqueValues(int length)
        {
            var array = new int[length];
            var random = new Random(DateTime.Now.Millisecond);
            var set = new HashSet<int>();

            for (int i = 0; i < length; i++)
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

            Console.WriteLine($"Elements after adding: {dict.Count}");
        }

        void RemoveElements(IDictionary<int, int> dict, int[] values)
        {
            foreach (var elem in values)
            {
                dict.Remove(elem);
            }

            Console.WriteLine($"Elements after removing: {dict.Count}");
        }

        void FindElements(IDictionary<int, int> dict, int[] array)
        {
            foreach (var element in array)
            {
                dict.ContainsKey(element);
            }            
        }

        BenchmarkResult RunBenchmark<T>(int[] array, int[] delete) 
            where T : IDictionary<int, int>, new()
        {
            var dict = new T();
            var watch = new Stopwatch();
            var result = new BenchmarkResult();

            watch.Start();
            AddElements(dict, array);
            watch.Stop();
            result.AddTimeInMs = watch.ElapsedMilliseconds;

            watch.Restart();
            RemoveElements(dict, delete);
            watch.Stop();
            result.RemoveTimeInMs = watch.ElapsedMilliseconds;

            watch.Restart();
            FindElements(dict, array);
            watch.Stop();
            result.FindTimeInMs = watch.ElapsedMilliseconds;

            return result;
        }

        void RunTest(int[] array, int[] delete, BenchmarkResult totalResultSL = null, 
            BenchmarkResult totalResultSkip = null)
        {
            Console.WriteLine("SortedList:");
            var result = RunBenchmark<SortedList<int, int>>(array, delete);
            totalResultSL?.Add(result);
            Console.WriteLine(result.ToString());
            Console.WriteLine();

            Console.WriteLine("SkipList:");
            result = RunBenchmark<SkipList<int, int>>(array, delete);
            totalResultSkip?.Add(result);
            Console.WriteLine(result.ToString());
            Console.WriteLine();
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
            var totalResultSL = new BenchmarkResult();
            var totalResultSkip = new BenchmarkResult();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("---------------------------Random Data-----------------------------");

            for (int i = 0; i < numberOfTests; i++)
            {
                var array = CreateRandomUniqueValues(lengthOfArray);
                var delete = new int[stopDel - startDel];
                Array.Copy(array, startDel, delete, 0, stopDel - startDel);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Test: {0}", i + 1);
                Console.ForegroundColor = ConsoleColor.White;
                RunTest(array, delete, totalResultSL, totalResultSkip);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Average: ");
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("SortedList:\t");
            Console.WriteLine(CreateAverageValue(totalResultSL, numberOfTests).ToString());

            Console.Write("SkipList:\t");
            Console.WriteLine(CreateAverageValue(totalResultSkip, numberOfTests).ToString());
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;            
            Console.WriteLine("---------------------------Sorted Data-----------------------------");
            Console.ForegroundColor = ConsoleColor.White;
            var arraySort = CreateSortedUniqueValues(lengthOfArray);
            var deleteSort = new int[stopDel - startDel];
            Array.Copy(arraySort, startDel, deleteSort, 0, stopDel - startDel);
            RunTest(arraySort, deleteSort);
        }

    }
}