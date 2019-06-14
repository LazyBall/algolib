using System;
using System.Collections.Generic;

namespace MyLibrary.Algorithms.Sorting
{
    public static class InsertionSort<T>
    {
        private static void SortWithLinearSearch(T[] array, IComparer<T> comparer)
        {
            for (int i = 1; i < array.Length; i++)
            {
                var item = array[i];
                int j = i - 1;

                while ((j >= 0) && (comparer.Compare(array[j], item) > 0))
                {
                    array[j + 1] = array[j];
                    j--;
                }
               
                //проверка отличия позиции вставки от текущей позиции элемента
                if (++j != i)
                {
                    array[j] = item;
                }
            }
        }

        private class ComparerIgnoringEqual : IComparer<T>
        {
            readonly IComparer<T> _comparer;

            public ComparerIgnoringEqual(IComparer<T> comparer)
            {                
                this._comparer = comparer;
            }

            public int Compare(T x, T y)
            {
                var comparisonResult = _comparer.Compare(x, y);
                return (comparisonResult == 0) ? -1 : comparisonResult;
            }
        }

        private static void SortWithBinarySearch(T[] array, IComparer<T> comparer)
        {
            for (int i = 1; i < array.Length; i++)
            {                                
                int index = ~Array.BinarySearch(array, 0, i, array[i], new ComparerIgnoringEqual(comparer));
                if (index < i)
                {
                    var item = array[i];
                    int j = i;

                    do
                    {
                        array[j] = array[--j];
                    } while (j > index);

                    array[index] = item;
                }                              
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
            SortWithBinarySearch(array, comparer);
            //SortWithLinearSearch(array, comparer);
        }
    }
}