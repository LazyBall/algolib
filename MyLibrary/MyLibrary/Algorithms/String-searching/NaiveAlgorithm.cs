using System;
using System.Collections.Generic;

namespace MyLibrary.Algorithms.StringSearch
{
    /// <summary>
    /// Наивный алгоритм поиска.
    /// </summary>
    public class NaiveAlgorithm : IStringSearchingAlgorithm
    {

        public int GetFirstEntry(string pattern, string text)
        {
            foreach (var entry in GetAllEntries(pattern, text))
            {
                return entry;
            }

            return -1;
        }

        public IEnumerable<int> GetAllEntries(string pattern, string text)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern is null");
            }
            if (text == null)
            {
                throw new ArgumentNullException("text is null");
            }
            if (pattern == string.Empty)
            {
                yield return 0;
                yield break;
            }

            int stop = text.Length - pattern.Length + 1;

            for (int i = 0; i < stop; i++)
            {
                int j = 0;

                while ((j < pattern.Length) && (pattern[j] == text[i + j]))
                {
                    j++;
                }

                if (j == pattern.Length)
                {
                    yield return i;
                }
            }
        }
    }
}