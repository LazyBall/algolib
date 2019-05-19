using System;
using System.Collections.Generic;
using System.IO;

namespace TaskB
{
    class Program
    {
        static void Main()
        {
            int start;
            int[] cities;
            var graph = ReadFromFile("input.txt", out start, out cities);
            var distances = DijkstrasAlgorithm.FindShortestPaths(graph, start);
            var cityTimePairs = new KeyValuePair<int, int>[cities.Length];

            for (int i = 0; i < cities.Length; i++)
            {
                var city = cities[i];
                cityTimePairs[i] = new KeyValuePair<int, int>(city, distances[city]);
            }

            Array.Sort(cityTimePairs, delegate (KeyValuePair<int, int> cityTimePair1,
                 KeyValuePair<int, int> cityTimePair2)
            {
                var comparisonResult = cityTimePair1.Value.CompareTo(cityTimePair2.Value);
                if (comparisonResult == 0)
                {
                    comparisonResult = cityTimePair1.Key.CompareTo(cityTimePair2.Key);
                }
                return comparisonResult;
            });
            WriteToFile("output.txt", cityTimePairs);
        }


        static Graph<int> ReadFromFile(string fileName,
            out int start, out int[] cities)
        {
            using (var inputFile = new StreamReader(fileName))
            {
                var sep = new char[] { ' ' };
                var option = StringSplitOptions.RemoveEmptyEntries;
                var str = inputFile.ReadLine().Split(sep, option);
                var graph = new Graph<int>(int.Parse(str[0]));
                int edgesCount = int.Parse(str[1]);
                cities = new int[int.Parse(str[2])];
                start = int.Parse(str[3]) - 1;
                str = inputFile.ReadLine().Split(sep, option);

                for (int i = 0; i < cities.Length; i++)
                {
                    cities[i] = int.Parse(str[i]) - 1;
                }

                for (int i = 0; i < edgesCount; i++)
                {
                    str = inputFile.ReadLine().Split(sep, option);
                    int leftVertex = int.Parse(str[0]) - 1,
                        rightVertex = int.Parse(str[1]) - 1;
                    int weight = int.Parse(str[2]);

                    graph[leftVertex].Add(new KeyValuePair<int, int>(rightVertex, weight));
                    graph[rightVertex].Add(new KeyValuePair<int, int>(leftVertex, weight));
                }

                return graph;
            }
        }

        static void WriteToFile(string fileName, IEnumerable<KeyValuePair<int, int>> cityTimePairs)
        {
            using (var outputFile = new StreamWriter(fileName))
            {
                foreach (var cityTimePair in cityTimePairs)
                {
                    outputFile.WriteLine("{0} {1}", cityTimePair.Key + 1, cityTimePair.Value);
                }
            }
        }
    }

    class Graph<TWeight>
    {
        private List<KeyValuePair<int, TWeight>>[] _adjacencyList;
        public int NumberVertices { get; private set; }
        public List<KeyValuePair<int, TWeight>> this[int vertex]
        {
            get
            {
                return _adjacencyList[vertex];
            }
        }

        public Graph(int numberVertices)
        {
            this.NumberVertices = numberVertices;
            _adjacencyList = new List<KeyValuePair<int, TWeight>>[this.NumberVertices];

            for (int i = 0; i < _adjacencyList.Length; i++)
            {
                _adjacencyList[i] = new List<KeyValuePair<int, TWeight>>();
            }
        }
    }

    static class DijkstrasAlgorithm
    {
        public static int[] FindShortestPaths(Graph<int> graph, int start)
        {
            var distances = new int[graph.NumberVertices];

            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = int.MaxValue;
            }

            distances[start] = 0;

            foreach (var edge in graph[start])
            {
                var newDistance = edge.Value;
                if (newDistance < distances[edge.Key])
                {
                    distances[edge.Key] = newDistance;
                }
            }

            var visited = new HashSet<int>
            {
                start
            };

            while (visited.Count < graph.NumberVertices)
            {
                int currentVertex = 0;
                int minDistance = int.MaxValue;

                for (int i = 0; i < distances.Length; i++)
                {
                    if (!visited.Contains(i))
                    {
                        if (distances[i] < minDistance)
                        {
                            currentVertex = i;
                            minDistance = distances[i];
                        }
                    }
                }

                if (minDistance < int.MaxValue)
                {
                    visited.Add(currentVertex);

                    foreach (var edge in graph[currentVertex])
                    {
                        if (!visited.Contains(edge.Key))
                        {
                            int newDistance = distances[currentVertex] + edge.Value;
                            if (newDistance < distances[edge.Key])
                            {
                                distances[edge.Key] = newDistance;
                            }
                        }
                    }

                }
                else
                {
                    break;
                }
            }

            return distances;
        }
    }
}