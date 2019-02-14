using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
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
        static void Main(string[] args)
        {
            var words = DoWords(ReadFile("WarAndWorld.txt"));
            var watch = new Stopwatch();
            watch.Start();
            UseSortedSet(words);
            watch.Stop();
            Console.WriteLine("Time: {0}", watch.ElapsedMilliseconds);
            watch.Reset();
            watch.Start();
            UseSortedDictionary(words);
            watch.Stop();
            Console.WriteLine("Time: {0}", watch.ElapsedMilliseconds);
            watch.Reset();
            watch.Start();
            UseDictionary(words);
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
            var strBuid = new StringBuilder();
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

        static void UseSortedSet(IEnumerable<string> words)
        {
            var sortList = new SortedList<string, int>();
            foreach (var word in words)
            {
                if (sortList.ContainsKey(word))
                {
                    sortList[word]++;
                }
                else
                {
                    sortList.Add(word, 1);
                }
            }

            var p = sortList.OrderByDescending(x => x.Value);
            Console.WriteLine("---------SortedList--------");
            Console.WriteLine("Count: {0}", sortList.Count);
            int i = 0;
            foreach (var word in p)
            {
                if (i < 11)
                    Console.WriteLine("Word: {0} \t Count: {1}", word.Key, word.Value);
                else
                {
                    break;
                }
                i++;
            }
        }


        static void UseSortedDictionary(IEnumerable<string> words)
        {
            var sortDictionary = new SortedDictionary<string, int>();
            foreach (var word in words)
            {
                if (sortDictionary.ContainsKey(word))
                {
                    sortDictionary[word]++;
                }
                else
                {
                    sortDictionary.Add(word, 1);
                }
            }

            var p = sortDictionary.OrderByDescending(x => x.Value);
            Console.WriteLine("---------SortedDictionary--------");
            Console.WriteLine("Count: {0}", sortDictionary.Count);
            int i = 0;
            foreach (var word in p)
            {
                if (i < 11)
                    Console.WriteLine("Word: {0} \t Count: {1}", word.Key, word.Value);
                else
                {
                    break;
                }
                i++;
            }
        }

        static void UseDictionary(IEnumerable<string> words)
        {
            var dictionary = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (dictionary.ContainsKey(word))
                {
                    dictionary[word]++;
                }
                else
                {
                    dictionary.Add(word, 1);
                }
            }

            var p = dictionary.OrderByDescending(x => x.Value);
            Console.WriteLine("---------Dictionary--------");
            Console.WriteLine("Count: {0}", dictionary.Count);
            int i = 0;
            foreach (var word in p)
            {
                if (i < 11)
                    Console.WriteLine("Word: {0} \t Count: {1}", word.Key, word.Value);
                else
                {
                    break;
                }
                i++;
            }
        }


        static void UseList(IEnumerable<string> words)
        {
            var list = new List<Pair<string, int>>();
            foreach (var word in words)
            {
                var pair = new Pair<string, int>(word, 1);
                int index = list.IndexOf(pair);
                if (index != -1)
                {
                    list[index].Value++;
                }
                else
                {
                    list.Add(pair);
                }
            }

            var p = list.OrderByDescending(x => x.Value);
            Console.WriteLine("---------List--------");
            Console.WriteLine("Count: {0}", list.Count);
            int i = 0;
            foreach (var word in p)
            {
                if (i < 11)
                    Console.WriteLine("Word: {0} \t Count: {1}", word.Key, word.Value);
                else
                {
                    break;
                }
                i++;
            }
        }


        class Pair<Tkey, TValue> : IComparable<Pair<Tkey, TValue>>
            where Tkey : IComparable<Tkey>
        {
            public Tkey Key;
            public TValue Value;

            public Pair(Tkey key, TValue value)
            {
                this.Key = key;
                this.Value = value;
            }
            public int CompareTo(Pair<Tkey, TValue> other)
            {
                return this.Key.CompareTo(other.Key);
            }
        }
    }
}