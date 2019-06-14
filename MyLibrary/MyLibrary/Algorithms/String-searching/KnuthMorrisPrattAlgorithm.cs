using System;
using System.Collections.Generic;

namespace MyLibrary.Algorithms.StringSearch
{
    /// <summary>
    /// Алгоритм Кнута — Морриса — Пратта (КМП-алгоритм).
    /// </summary>
    public class KnuthMorrisPrattAlgorithm : IStringSearchingAlgorithm
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

            var pi = PrefixFunction.Compute(pattern);
            int j = 0;

            for (int i = 0; i < text.Length; i++)
            {
                while ((j > 0) && (pattern[j] != text[i]))
                {
                    j = pi[j - 1];
                }

                if (pattern[j] == text[i])
                {
                    j++;
                }
                if (j == pattern.Length)
                {
                    yield return (i - j + 1);
                    j = pi[j - 1];
                }
            }

        }
    }
}