using System;
using System.Collections.Generic;
using System.Diagnostics;
using MyLibrary.Algorithms.Sorting;

namespace Lab5
{
    class Worker
    {
        class BenchmarkResult
        {
            public long SortedTime { get; set; }
            public long PartiallySortedTime { get; set; }
            public long RandomWithRepeatitionsTime { get; set; }
            public long RandomUniqueTime { get; set; }

            public void Add(BenchmarkResult result)
            {
                this.SortedTime += result.SortedTime;
                this.PartiallySortedTime += result.PartiallySortedTime;
                this.RandomWithRepeatitionsTime += result.RandomWithRepeatitionsTime;
                this.RandomUniqueTime += result.RandomUniqueTime;
            }

            public override string ToString()
            {
                return
                $"Sorted: {SortedTime} ms\t" +
                $"PartiallySorted: {PartiallySortedTime} ms\t" +
                $"RandomWithRepeatitions: {RandomUniqueTime} ms\t" +
                $"RandomUnique: {RandomWithRepeatitionsTime} ms";
            }
        }

        int[] CreateRandomValues(int length, bool unique = false)
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
                } while (unique && !set.Add(value));

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

        int[] CreatePartiallySortedValues(int length)
        {
            var array = new int[length];
            Array.Copy(CreateSortedUniqueValues(length / 2), array, length / 2);
            Array.Copy(CreateRandomValues(length - length / 2), 0, array, length / 2, length - length / 2);
            return array;
        }

        long RunBenchmark(Action<int[]> action, int[] array)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            var watch = new Stopwatch();
            
            watch.Start();
            action(array);
            watch.Stop();

            return watch.ElapsedMilliseconds;
        }

        BenchmarkResult RunTest(Action<int[]> action, int length)
        {
            var result = new BenchmarkResult
            {
                SortedTime = RunBenchmark(action, CreateSortedUniqueValues(length))
            };

            result.PartiallySortedTime = RunBenchmark(action, CreatePartiallySortedValues(length));

            result.RandomUniqueTime = RunBenchmark(action, CreateRandomValues(length, true));

            result.RandomWithRepeatitionsTime = RunBenchmark(action, CreateRandomValues(length, false));

            return result;
        }

        BenchmarkResult CreateAverageValue(BenchmarkResult totalResult, int numberOfTests)
        {
            return new BenchmarkResult()
            {
                SortedTime = totalResult.SortedTime / numberOfTests,
                PartiallySortedTime = totalResult.PartiallySortedTime / numberOfTests,
                RandomUniqueTime = totalResult.RandomUniqueTime / numberOfTests,
                RandomWithRepeatitionsTime = totalResult.RandomWithRepeatitionsTime / numberOfTests
            };
        }

        void PrintTestInfo(int length, BenchmarkResult totalResultQuick, BenchmarkResult totalResultHeap)
        {
            var result = RunTest(QuickSort<int>.Sort, length);
            totalResultQuick.Add(result);
            Console.WriteLine("QuickSort:");
            Console.WriteLine(result.ToString());

            Console.WriteLine();
            result = RunTest(HeapSort<int>.Sort, length);
            totalResultHeap.Add(result);
            Console.WriteLine("HeapSort:");
            Console.WriteLine(result.ToString());
            Console.WriteLine();
        }

        public void ShowInfo(int lengthOfArray, int numberOfTests)
        {
            var totalResultQuick = new BenchmarkResult();
            var totalResultHeap = new BenchmarkResult();
            Console.WriteLine();

            for (int i = 0; i < numberOfTests; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Test: {0}", i + 1);
                Console.ForegroundColor = ConsoleColor.White;
                PrintTestInfo(lengthOfArray, totalResultQuick, totalResultHeap);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Average: ");
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("QuickSort:\t");
            Console.WriteLine(CreateAverageValue(totalResultQuick, numberOfTests).ToString());

            Console.Write("HeapSort:\t");
            Console.WriteLine(CreateAverageValue(totalResultHeap, numberOfTests).ToString());
            Console.WriteLine();
        }
    }
}