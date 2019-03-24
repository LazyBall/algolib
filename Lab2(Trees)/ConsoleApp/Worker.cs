using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        }

        int[] CreateRandomUniqueValues(int length)
        {
            var random = new Random(DateTime.Now.Millisecond);
            int[] array = new int[length];
            var hashset = new HashSet<int>();
            for (int i = 0; i < length; i++)
            {
                var value = random.Next();
                while(hashset.Contains(value))
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
            foreach(var element in array)
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

        void RunTest(int[] array, int startDel, int stopDel, List<BenchmarkResult> resultsSD=null,
            List<BenchmarkResult> resultsAVL=null, List<BenchmarkResult> resultsBin=null)
        {
            Console.Write("SortedDictionary: \t");
            var result = RunBenchmark<SortedDictionary<int, int>>(array, startDel, stopDel);
            resultsSD?.Add(result);
            Console.WriteLine(CreateTestInfo(result));        
            
            Console.Write("AVLTree: \t\t");
            result = RunBenchmark<AVLTree<int, int>>(array, startDel, stopDel);
            resultsAVL?.Add(result);
            Console.WriteLine(CreateTestInfo(result));   
            
            Console.Write("BinarySearchTree: \t");
            result = RunBenchmark<BinarySearchTree<int, int>>(array, startDel, stopDel);
            resultsBin?.Add(result);
            Console.WriteLine(CreateTestInfo(result)); 
            
            Console.WriteLine();
        }

        string CreateTestInfo(BenchmarkResult result)
        {
            return 
                $"Add: {result.AddTimeInMs} ms \t" +
                $"Remove: {result.RemoveTimeInMs} ms \t" +
                $"Find: {result.FindTimeInMs} ms \t" +
                $"Total time: {result.AddTimeInMs + result.RemoveTimeInMs + result.FindTimeInMs } ms";
        }

        BenchmarkResult CreateAverageValues(List<BenchmarkResult> results)
        {
            return new BenchmarkResult()
            {
                AddTimeInMs = (long)(from t in results select t.AddTimeInMs).Average(),
                RemoveTimeInMs = (long)(from t in results select t.RemoveTimeInMs).Average(),
                FindTimeInMs = (long)(from t in results select t.FindTimeInMs).Average()
            };
        }

        public void ShowInfo(int lengthOfArray, int startDel, int stopDel, int numberOfTests)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            var listSD = new List<BenchmarkResult>();
            var listAVL = new List<BenchmarkResult>();
            var listBin = new List<BenchmarkResult>();
            Console.WriteLine("---------------------------Random Data-----------------------------");

            for (int i = 0; i < numberOfTests; i++)
            {
                var array = CreateRandomUniqueValues(lengthOfArray);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Test: {0}", i + 1);
                Console.ForegroundColor = ConsoleColor.White;
                RunTest(array, startDel, stopDel, listSD, listAVL, listBin);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("In Average: ");
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("SortedDictionary: \t");
            Console.WriteLine(CreateTestInfo(CreateAverageValues(listSD)));

            Console.Write("AVLTree: \t\t");
            Console.WriteLine(CreateTestInfo(CreateAverageValues(listAVL)));

            Console.Write("BinarySearchTree: \t");
            Console.WriteLine(CreateTestInfo(CreateAverageValues(listBin)));
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("---------------------------Sorted Data-----------------------------");
            Console.ForegroundColor = ConsoleColor.White;
            RunTest(CreateSortedUniqueValues(lengthOfArray), startDel, stopDel);
        }
    }
}