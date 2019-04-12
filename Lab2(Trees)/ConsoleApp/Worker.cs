using System;
using System.Collections.Generic;
using System.Diagnostics;
using Trees;
using System.Linq;

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
            var random = new Random(DateTime.Now.Millisecond);
            var hashset = new HashSet<int>();

            while (hashset.Count < length)
            {
                hashset.Add(random.Next());
            }

            return hashset.ToArray();
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

        void AddElements(IDictionary<int, int> dict, IEnumerable<int> arrayOfUniqueValues)
        {
            foreach (var elem in arrayOfUniqueValues)
            {
                dict.Add(elem, elem);
            }

            Console.WriteLine($"Elements after adding: {dict.Count}");
        }

        void RemoveElements(IDictionary<int, int> dict, IEnumerable<int> values)
        {
            foreach (var elem in values)
            {
                dict.Remove(elem);
            }

            Console.WriteLine($"Elements after removing: {dict.Count}");
        }

        void FindElements(IDictionary<int, int> dict, IEnumerable<int> array)
        {
            foreach (var element in array)
            {
                dict.ContainsKey(element);
            }            
        }

        BenchmarkResult RunBenchmark<T>(IEnumerable<int> array, IEnumerable<int> delete) 
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

        void RunTest(IEnumerable<int> array, IEnumerable<int> delete, BenchmarkResult totalResultSD = null,
            BenchmarkResult totalResultAVL = null, BenchmarkResult totalResultBin = null, 
            BenchmarkResult totalResultDimas = null)
        {
            Console.WriteLine("SortedDictionary:\t");
            var result = RunBenchmark<SortedDictionary<int, int>>(array, delete);
            totalResultSD?.Add(result);
            Console.WriteLine(result.ToString());
            Console.WriteLine();

            Console.WriteLine("AVLTree:\t\t");
            result = RunBenchmark<AVLTree<int, int>>(array, delete);
            totalResultAVL?.Add(result);
            Console.WriteLine(result.ToString());
            Console.WriteLine();

            Console.WriteLine("BinarySearchTree:\t");
            result = RunBenchmark<BinarySearchTree<int, int>>(array, delete);
            totalResultBin?.Add(result);
            Console.WriteLine(result.ToString());
            Console.WriteLine();

            Console.WriteLine("Dimas:\t\t\t");
            result = RunBenchmark<DimasAVLTree<int, int>>(array, delete);
            totalResultDimas?.Add(result);
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
            var totalResultSD = new BenchmarkResult();
            var totalResultAVL = new BenchmarkResult();
            var totalResultBin = new BenchmarkResult();
            var totalResultDimas = new BenchmarkResult();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---------------------------Random Data-----------------------------");

            for (int i = 0; i < numberOfTests; i++)
            {
                var array = CreateRandomUniqueValues(lengthOfArray);
                var delete = new int[stopDel - startDel];
                Array.Copy(array, startDel, delete, 0, stopDel - startDel);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Test: {0}", i + 1);
                Console.ForegroundColor = ConsoleColor.White;
                RunTest(array, delete, totalResultSD, totalResultAVL, totalResultBin, totalResultDimas);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Average: ");
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("SortedDictionary: \t");
            Console.WriteLine(CreateAverageValue(totalResultSD, numberOfTests).ToString());

            Console.Write("AVLTree: \t\t");
            Console.WriteLine(CreateAverageValue(totalResultAVL, numberOfTests).ToString());

            Console.Write("BinarySearchTree: \t");
            Console.WriteLine(CreateAverageValue(totalResultBin, numberOfTests).ToString());

            Console.Write("Dimas: \t\t\t");
            Console.WriteLine(CreateAverageValue(totalResultDimas, numberOfTests).ToString());
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