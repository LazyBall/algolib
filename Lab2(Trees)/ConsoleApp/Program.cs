using System;
using System.Collections.Generic;
using Trees;
using System.Diagnostics;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            int length = 10000, startDel = 5000, stopDel = 7000, numberOfTests = 10;
            var worker = new Worker();

            for (int i = 0; i < numberOfTests; i++)
            {
                var array = worker.CreateRandomArray(length);
                Console.WriteLine("Test: {0}", i + 1);
                RunBenchmark(worker.TestTree<BinarySearchTree<int, int>>, array, startDel, stopDel,
                    "BinarySearchTree");
                RunBenchmark(worker.TestTree<SortedDictionary<int, int>>, array, startDel, stopDel,
                   "SortedDictionary");
                Console.WriteLine();
            }
        }

        static void RunBenchmark(Action<int[], int, int> action, int[] array,
            int startDel, int stopDel, string nameDataStruct)
        {
            var watch = new Stopwatch();
            watch.Start();
            action(array, startDel, stopDel);
            watch.Stop();
            Console.WriteLine($"{nameDataStruct} Time: {watch.ElapsedMilliseconds} ms.");
        }
    }
}