using System;
using System.Collections.Generic;
using System.IO;

namespace TaskC
{
    class Program
    {
        static void Main()
        {
            var inputData = ReadFromFile("input.txt");
            var pair = FloydWarshallAlgorithm.FindShortestPaths(inputData.Edges,
                inputData.NumberVertices);
            inputData.Edges = null;
            var distances = pair.Key;
            var next = pair.Value;

            int start = 0, end = 0, maxDist = -1;
            for (int i = 1; i < inputData.NumberVertices; i++)
            {
                for (int j = i + 1; j < inputData.NumberVertices; j++)
                {
                    if (distances[i, j] != int.MaxValue && distances[i, j] > maxDist)
                    {
                        start = i;
                        end = j;
                        maxDist = distances[i, j];
                    }
                }
            }

            var path = new List<int>();
            if (maxDist == -1)
            {
                maxDist = 0;
            }
            else
            {
                path = FloydWarshallAlgorithm.GetShortestPath(start, end, next);
            }
            WriteToFile("output.txt", maxDist, path);

        }

        static InputData ReadFromFile(string fileName)
        {
            var data = new InputData();
            using (var inputFile = new StreamReader(fileName))
            {
                var sep = new char[] { ' ' };
                var option = StringSplitOptions.RemoveEmptyEntries;
                var str = inputFile.ReadLine().Split(sep, option);
                data.NumberVertices = int.Parse(str[0]) + 1;
                int countEdges = int.Parse(str[1]) * 2;
                var edges = new int[countEdges][];

                for (int i = 0; i < countEdges; i++)
                {
                    str = inputFile.ReadLine().Split(sep, option);
                    edges[i] = new int[3];
                    int left = int.Parse(str[0]), right = int.Parse(str[1]),
                        weight = int.Parse(str[2]);
                    edges[i][0] = left;
                    edges[i][1] = right;
                    edges[i][2] = weight;
                    i++;
                    edges[i] = new int[3];
                    edges[i][0] = right;
                    edges[i][1] = left;
                    edges[i][2] = weight;
                }

                data.Edges = edges;
            }

            return data;
        }

        static void WriteToFile(string fileName, int distance, List<int> path)
        {
            using (var outputFile = new StreamWriter(fileName))
            {
                outputFile.WriteLine(distance);
                foreach (var item in path)
                {
                    outputFile.Write("{0} ", item);
                }
            }
        }

    }

    class InputData
    {
        public int NumberVertices { get; set; }
        public int[][] Edges { get; set; }
    }

    static class FloydWarshallAlgorithm
    {
        private static KeyValuePair<int[,], int[,]> FindShortestPaths(int[,] adjacencyMatrix)
        {
            int n = adjacencyMatrix.GetLength(0);
            var distance = adjacencyMatrix;
            var next = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (distance[i, j] == int.MaxValue)
                    {
                        next[i, j] = -1;
                    }
                    else
                    {
                        next[i, j] = j;
                    }
                }
            }

            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        var newDist = (long)distance[i, k] + distance[k, j];
                        if (newDist < distance[i, j])
                        {
                            distance[i, j] = (int)newDist;
                            next[i, j] = k;
                        }
                    }
                }
            }

            return new KeyValuePair<int[,], int[,]>(distance, next);
        }

        public static KeyValuePair<int[,], int[,]> FindShortestPaths(int[][] edges, int numberVertices)
        {
            return FindShortestPaths(CreateAdjacencyMatrix(edges, numberVertices));
        }

        private static int[,] CreateAdjacencyMatrix(int[][] edges, int numberVertices)
        {
            var matrix = new int[numberVertices, numberVertices];

            for (int i = 0; i < numberVertices; i++)
            {
                for (int j = 0; j < numberVertices; j++)
                {
                    matrix[i, j] = int.MaxValue;
                }
                matrix[i, i] = 0;
            }

            for (int i = 0; i < edges.Length; i++)
            {
                int row = edges[i][0], column = edges[i][1];
                matrix[row, column] = edges[i][2];
            }

            return matrix;
        }

        public static List<int> GetShortestPath(int start, int end, int[,] next)
        {
            var path = new List<int>();
            if (next[start, end] != -1)
            {
                var current = start;
                while (current != end)
                {
                    path.Add(current);
                    current = next[current, end];
                }
                path.Add(end);
            }
            return path;
        }
    }
}