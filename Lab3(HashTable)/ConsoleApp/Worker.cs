using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using HashTable;

namespace ConsoleApp
{
    class Worker
    {

        IEnumerable<char> ReadFileByCharacter(string fileName)
        {
            using (var inputFile = new StreamReader(fileName))
            {
                while (inputFile.Peek() >= 0)
                {
                    yield return (char)inputFile.Read();
                }
            }
        }

        IEnumerable<string> DoWords(IEnumerable<char> text)
        {
            var strBuilder = new StringBuilder(8); // средняя длина русского слова
            foreach (var symbol in text)
            {
                var symbolLower = char.ToLower(symbol);
                if (char.IsLetter(symbolLower))
                {
                    strBuilder.Append(symbolLower);
                }
                else
                {
                    if (strBuilder.Length > 0)
                    {
                        yield return strBuilder.ToString();
                        strBuilder.Clear();
                    }
                }
            }
        }

        void InsertWords(IDictionary<string, int> hashTable, IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                if (!hashTable.ContainsKey(word)) hashTable.Add(word, 1);
                else hashTable[word]++;
            }
        }

        void DeleteWords(IDictionary<string, int> hashTable, IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                hashTable.Remove(word);
            }
        }

        void RunTest<T>(IEnumerable<string> insert, IEnumerable<string> delete)
            where T : IDictionary<string, int>, new()
        {
            var hashTable = new T();
            InsertWords(hashTable, insert);
            Console.WriteLine("Number of elements after insertion: {0} elemets", hashTable.Count);
            DeleteWords(hashTable, delete);
            Console.WriteLine("Number of elements after deletion: {0} elemets", hashTable.Count);
        }

        public void RunBenchmark()
        {
            var array = DoWords(ReadFileByCharacter("WarAndWorld.txt")).ToArray();
            var del = (from w in array where w.Length == 7 select w).ToArray();
            var watch = new Stopwatch();

            Console.WriteLine("Dictionary");
            watch.Start();
            RunTest<Dictionary<string, int>>(array, del);
            watch.Stop();
            Console.WriteLine("Time: {0} ms.", watch.ElapsedMilliseconds);
            Console.WriteLine();

            Console.WriteLine("HashTable");
            watch.Restart();
            RunTest<HashTable<string, int>>(array, del);
            watch.Stop();
            Console.WriteLine("Time: {0} ms.", watch.ElapsedMilliseconds);
        }
    }
}
