using System;
using System.Collections.Generic;
using Trees;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            int length = 10000, startDel = 5000, stopDel = 7000, numberOfTests = 10;
            var worker = new Worker();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("-------------------Random Data--------------------");
            for (int i = 0; i < numberOfTests; i++)
            {
                var array = worker.CreateRandomValues(length);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Test: {0}", i + 1);
                Console.ForegroundColor = ConsoleColor.White;
                ShowInfo(array, startDel, stopDel);
            }
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("-------------------Sorted Data--------------------");
            Console.ForegroundColor = ConsoleColor.White;
            ShowInfo(worker.CreateSortedValues(length), startDel, stopDel);
        }


        static void ShowInfo(int[] array, int startDel, int stopDel)
        {
            var worker = new Worker();
            Console.Write("SortedDictionary: \t");
            worker.RunBenchmark<SortedDictionary<int, int>>(array, startDel, stopDel);
            Console.Write("AVLTree: \t\t");
            worker.RunBenchmark<AVLTree<int, int>>(array, startDel, stopDel);
            Console.Write("BinarySearchTree: \t");
            worker.RunBenchmark<BinarySearchTree<int, int>>(array, startDel, stopDel);
            Console.WriteLine();
        }       
    }
}