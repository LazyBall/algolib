using System;
using MyLibrary.DataStructures;

namespace MyLibrary.Algorithms.Sorting
{
    public static class HeapSort<T> where T : IComparable<T>
    {
        public static void Sort(T[] array)
        {
            var binHeap = new BinaryHeap<T>(array);

            for (int i = array.Length - 1; i >= 0; i--)
            {
                array[i] = binHeap.Dequeue();
            }

        }
    }
}