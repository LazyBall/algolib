using System;
using System.Collections.Generic;

namespace MyLibrary.Algorithms.StringSearch
{
    /// <summary>
    /// Алгоритм Рабина-Карпа.
    /// </summary>
    public class RabinKarpAlgorithm : IStringSearchingAlgorithm
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
            if(pattern.Length>text.Length)
            {
                yield break;
            }


            uint factor = 1;
            for (int i = 1; i < pattern.Length; i++)
            {
                factor <<= 1;
            }

            uint hashText, hashPattern;
            hashText = hashPattern = 0;

            for (int i = 0; i < pattern.Length; i++)
            {
                hashText = (hashText << 1) + text[i];
                hashPattern = (hashPattern << 1) + pattern[i];
            }

            int stop = text.Length - pattern.Length;

            for (int i = 0; i < stop; i++)
            {
                if (hashPattern == hashText)
                {
                    //если нужно нечеткое сравнение строк, то это сравнение убираем
                    if (string.CompareOrdinal(pattern, 0, text, i, pattern.Length) == 0)
                    {
                        yield return i;
                    }
                }
                hashText = ((hashText - factor * text[i]) << 1) + text[i + pattern.Length];
            }

            if (hashPattern == hashText)
            {
                if (string.CompareOrdinal(pattern, 0, text, stop, pattern.Length) == 0)
                {
                    yield return stop;
                }
            }
        }

        //Более безопасный хэш, но медленнее в 2 раза
        private static IEnumerable<int> GetAllEntries1(string pattern, string text)
        {
            //int alphabetSize = (new Random().Next(1, char.MaxValue + 2)); //Вики
            int alphabetSize = char.MaxValue + 1; //размер алфавита, как в Кормене
            int prime = int.MaxValue; //простое число, по модулю которго проводим вычисления
            long factor = 1;
            for (int i = 1; i < pattern.Length; i++)
            {
                factor = (factor * alphabetSize) % prime;
            }

            long hashText, hashPattern;
            hashText = hashPattern = 0;

            for (int i = 0; i < pattern.Length; i++)
            {
                hashText = (hashText * alphabetSize + text[i]) % prime;
                hashPattern = (hashPattern * alphabetSize + pattern[i]) % prime;
            }

            int stop = text.Length - pattern.Length;

            for (int i = 0; i < stop; i++)
            {
                if (hashPattern == hashText)
                {
                    if (string.CompareOrdinal(pattern, 0, text, i, pattern.Length) == 0)
                    {
                        yield return i;
                    }
                }
                hashText = ((hashText - factor * text[i]) * alphabetSize + text[i + pattern.Length]) % prime;
                if (hashText < 0)
                {
                    hashText += prime;
                }
            }

            if (hashPattern == hashText)
            {
                if (string.CompareOrdinal(pattern, 0, text, stop, pattern.Length) == 0)
                {
                    yield return stop;
                }
            }
        }
    }
}