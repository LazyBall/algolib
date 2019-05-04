using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using MyLibrary.DataStructures;

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
           
        class BenchmarkResult
        {
            public long AddTimeInMs { get; set; }
            public long RemoveTimeInMs { get; set; }
            public long FindTimeInMs { get; set; }

            public void Add(BenchmarkResult result)
            {
                this.AddTimeInMs += result.AddTimeInMs;
                this.RemoveTimeInMs += result.RemoveTimeInMs;
                this.FindTimeInMs += result.FindTimeInMs;
            }

            public override string ToString()
            {
                return
                $"Add: {AddTimeInMs} ms\t" +
                $"Remove: {RemoveTimeInMs} ms\t" +
                $"Find: {FindTimeInMs} ms\t" +
                $"Total: {AddTimeInMs + RemoveTimeInMs + FindTimeInMs} ms";
            }
        }

        void AddElements(IDictionary<string, int> dict, IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                if (!dict.ContainsKey(word)) dict.Add(word, 1);
                else dict[word]++;
            }

            Console.WriteLine($"Elements after adding: {dict.Count}");
        }

        void RemoveElements(IDictionary<string, int> dict, IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                dict.Remove(word);
            }

            Console.WriteLine($"Elements after removing: {dict.Count}");
        }

        void FindElements(IDictionary<string, int> dict, IEnumerable<string> array)
        {
            foreach (var element in array)
            {
                dict.ContainsKey(element);
            }
        }

        BenchmarkResult RunBenchmark<T>(IEnumerable<string> array, IEnumerable<string> delete) 
            where T : IDictionary<string, int>, new()
        {
            var dict = new T();
            var watch = new Stopwatch();
            var result = new BenchmarkResult();

            watch.Start();
            AddElements(dict, array);
            watch.Stop();
            result.AddTimeInMs = watch.ElapsedMilliseconds;

            watch.Restart();
            RemoveElements(dict, delete);
            watch.Stop();
            result.RemoveTimeInMs = watch.ElapsedMilliseconds;

            watch.Restart();
            FindElements(dict, array);
            watch.Stop();
            result.FindTimeInMs = watch.ElapsedMilliseconds;

            return result;
        }

        void RunTest(IEnumerable<string> array, IEnumerable<string> delete, BenchmarkResult totalResultDict = null,
            BenchmarkResult totalResultHash = null)
        {
            Console.WriteLine("Dictionary:\t");
            var result = RunBenchmark<Dictionary<string, int>>(array, delete);
            totalResultDict?.Add(result);
            Console.WriteLine(result.ToString());

            Console.WriteLine();

            Console.WriteLine("HashTable:\t\t");
            result = RunBenchmark<HashTable<string, int>>(array, delete);
            totalResultHash?.Add(result);
            Console.WriteLine(result.ToString());

            Console.WriteLine();
        }

        BenchmarkResult CreateAverageValue(BenchmarkResult totalResult, int numberOfTests)
        {
            return new BenchmarkResult()
            {
                AddTimeInMs = totalResult.AddTimeInMs / numberOfTests,
                RemoveTimeInMs = totalResult.RemoveTimeInMs / numberOfTests,
                FindTimeInMs = totalResult.FindTimeInMs / numberOfTests
            };
        }

        public void ShowInfo(int numberOfTests)
        {
            var totalResultDict = new BenchmarkResult();
            var totalResultHash = new BenchmarkResult();
            var words = DoWords(ReadFileByCharacter("WarAndWorld.txt")).ToArray();
            var delete = (from t in words where t.Length == 7 select t).ToArray();

            for (int i = 0; i < numberOfTests; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Test: {0}", i + 1);
                Console.ForegroundColor = ConsoleColor.White;
                RunTest(words, delete, totalResultDict, totalResultHash);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Average: ");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Dictionary: \t");
            Console.WriteLine(CreateAverageValue(totalResultDict, numberOfTests).ToString());
            Console.WriteLine();

            Console.WriteLine("HashTable: \t");
            Console.WriteLine(CreateAverageValue(totalResultHash, numberOfTests).ToString());
            
        }
    }
}