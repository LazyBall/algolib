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
            int length = 1000000, startDel = 500000, stopDel = 700000;
            var worker = new Worker();
            var array = worker.CreateRandomArray(length);
            var watch = new Stopwatch();
            watch.Start();
            worker.TestTree<BinarySearchTree<int, int>>(array, startDel, stopDel);
            watch.Stop();
            Console.WriteLine($"BinarySearchTree \n Time: {watch.ElapsedMilliseconds} ms.");
            Console.WriteLine();
            watch.Restart();
            worker.TestTree<SortedDictionary<int, int>>(array, startDel, stopDel);
            watch.Stop();
            Console.WriteLine($"SortDictionary \n Time: {watch.ElapsedMilliseconds} ms.");
        }
    }
}