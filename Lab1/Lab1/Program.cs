using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO; 
using System.Diagnostics;

namespace Lab1
{
    /*Лабораторная работа №1. "Методы программирования". 6 семестр.
Напишите  метод, который читает весь текст из файла WarAndWorld.txt,  разбивает его на слова.  
Далее вычисляет сколько уникальных слов содержится в тексте и вычисляет первые 10 часто встречающихся слов.  
При этом  используется контейнер SortedList.
Напишите еще три модификации этого же метода с использованием в качестве контейнера для данных SortedDictionary, 
List, Dictionary.  
Для двух последних надо вспомнить лямбда-выражения или LINQ-запросы для выполнения задания.
Напишите консольное приложение которое вызывает написанные Вами методы и замеряет время работы.
*/

    class Program
    {
        static void Main()
        {
            var words = new List<string>(DoWords(ReadFile("WarAndWorld.txt")));
            var watch = new Stopwatch();
            watch.Start();
            UseIDictionary<SortedList<string, int>>(words, "SortedList");
            watch.Stop();
            Console.WriteLine("Time: {0}", watch.ElapsedMilliseconds);
            watch.Reset();

            watch.Start();
            UseIDictionary<SortedDictionary<string, int>>(words, "SortedDictionary");
            watch.Stop();
            Console.WriteLine("Time: {0}", watch.ElapsedMilliseconds);
            watch.Reset();

            watch.Start();
            UseIDictionary<Dictionary<string, int>>(words, "Dictionary");
            watch.Stop();
            Console.WriteLine("Time: {0}", watch.ElapsedMilliseconds);
            watch.Reset();

            watch.Start();
            UseList(words);
            watch.Stop();
            Console.WriteLine("Time: {0}", watch.ElapsedMilliseconds);
        }

        static IEnumerable<char> ReadFile(string fileName)
        {
            using (var inputFile = new StreamReader(fileName))
            {
                while (inputFile.Peek() >= 0)
                {
                    yield return (char)inputFile.Read();
                }
            }
        }

        static IEnumerable<string> DoWords(IEnumerable<char> text)
        {
            var strBuid = new StringBuilder(8); // средняя длина русского слова
            foreach (var symbol in text)
            {
                var symbolLower = char.ToLower(symbol);
                if (char.IsLetter(symbolLower))
                {
                    strBuid.Append(symbolLower);
                }
                else
                {
                    if (strBuid.Length > 0)
                    {
                        yield return strBuid.ToString();
                        strBuid.Clear();
                    }
                }
            }
        }

        static void UseIDictionary<T>(IEnumerable<string> words, string nameDataStruct)
            where T : IDictionary<string, int>, new()
        {
            var iDict = new T();
            foreach (var word in words)
            {
                if (iDict.ContainsKey(word))
                {
                    iDict[word]++;
                }
                else
                {
                    iDict.Add(word, 1);
                }
            }
            var p = iDict.OrderByDescending(x => x.Value);
            Console.WriteLine();
            Console.WriteLine($"-----------------{nameDataStruct}-----------------------");
            Console.WriteLine("Count: {0}", iDict.Count);
            int i = 1;
            foreach (var pair in p)
            {
                Console.WriteLine("{0}. Word: {1} \t Count: {2}", i, pair.Key, pair.Value);
                i++;
                if (i > 10)
                {
                    break;
                }
            }
        }

        static void UseList(IEnumerable<string> words)
        {
            var list = new List<Pair<string, int>>();
            foreach (var word in words)
            {
                var pair = list.Find(x => x.Key == word);
                if (pair != null)
                {
                    pair.Value++;
                }
                else
                {
                    list.Add(new Pair<string, int>(word, 1));
                }
            }

            var p = list.OrderByDescending(x => x.Value);
            Console.WriteLine();
            Console.WriteLine("-------------List-------------");
            Console.WriteLine("Count: {0}", list.Count);
            int i = 1;
            foreach (var pair in p)
            {
                Console.WriteLine("{0}. Word: {1} \t Count: {2}", i, pair.Key, pair.Value);
                i++;
                if (i > 10)
                {
                    break;
                }
            }
        }


        class Pair<TKey, TValue> : IComparable<Pair<TKey, TValue>>
            where TKey : IComparable<TKey>
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }

            public Pair(TKey key, TValue value)
            {
                this.Key = key;
                this.Value = value;
            }
            public int CompareTo(Pair<TKey, TValue> other)
            {
                return this.Key.CompareTo(other.Key);
            }
        }
    }
}