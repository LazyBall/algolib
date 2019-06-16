using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLibrary.Algorithms.Sorting
{
    public static class MergeSort<T>
    {
        private static void Merge(T[] array, int left, int border, int right, IComparer<T> comparer)
        {
            int leftHalfIndex, rightHalfIndex;
            leftHalfIndex = rightHalfIndex = 0;
            var result = new T[right - left];

            while ((left + leftHalfIndex < border) && (border + rightHalfIndex < right))
            {
                if (comparer.Compare(array[left + leftHalfIndex], array[border + rightHalfIndex]) <= 0)
                {
                    result[leftHalfIndex + rightHalfIndex] = array[left + leftHalfIndex];
                    leftHalfIndex++;
                }
                else
                {
                    result[leftHalfIndex + rightHalfIndex] = array[border + rightHalfIndex];
                    rightHalfIndex++;
                }
            }

            while (left + leftHalfIndex < border)
            {
                result[leftHalfIndex + rightHalfIndex] = array[left + leftHalfIndex];
                leftHalfIndex++;
            }

            while (border + rightHalfIndex < right)
            {
                result[leftHalfIndex + rightHalfIndex] = array[border + rightHalfIndex];
                rightHalfIndex++;
            }

            for (int i = 0; i < result.Length; i++)
            {
                array[left + i] = result[i];
            }
        }

        private static void RecursiveMergeSort(T[] array, int left, int right, IComparer<T> comparer)
        {
            if (left + 1 < right)
            {
                var middle = left + (right - left) / 2;
                RecursiveMergeSort(array, left, middle, comparer);
                RecursiveMergeSort(array, middle, right, comparer);
                Merge(array, left, middle, right, comparer);
            }
        }

        private static void IterativeMergeSort(T[] array, IComparer<T> comparer)
        {
            for (int i = 1; i <= array.Length; i *= 2)
            {
                for (int j = 0; j <= array.Length - i; j += 2 * i)
                {
                    Merge(array, j, j + i, Math.Min(j + 2 * i, array.Length), comparer);
                }
            }
        }

        private static void MultithreadedIterativeMergeSort(T[] array, IComparer<T> comparer)
        {
            var tasks = new List<Task>((array.Length + 1) / 2);

            for (int i = 1; i <= array.Length; i *= 2)
            {
                for (int j = 0; j <= array.Length - i; j += 2 * i)
                {
                    int left = j, middle = j + i, right = Math.Min(j + 2 * i, array.Length);
                    tasks.Add(Task.Run(()
                        => Merge(array, left, middle, right, comparer)));
                }

                Task.WaitAll(tasks.ToArray());
                tasks.Clear();
            }
        }

        public static void Sort(T[] array)
        {
            Sort(array, Comparer<T>.Default);
        }

        public static void Sort(T[] array, IComparer<T> comparer)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array is null.");
            }
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer is null.");
            }
            //MultithreadedIterativeMergeSort(array, comparer);
            //RecursiveMergeSort(array, 0, array.Length, comparer);
            IterativeMergeSort(array, comparer);
        }
    }
}