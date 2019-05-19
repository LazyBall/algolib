using System;
using System.Collections.Generic;
using System.IO;

namespace TaskE
{
    class Program
    {
        static void Main()
        {
            var inputData = ReadFromFile("input.txt");
            WriteToFile("output.txt", BellmanFordAlgorithm.FindNegativeCycle(inputData.Edges,
                inputData.NumberVertices));
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
                data.Edges = new Edge[int.Parse(str[1])];

                for (int i = 0; i < data.Edges.Length; i++)
                {
                    str = inputFile.ReadLine().Split(sep, option);
                    data.Edges[i] = new Edge(int.Parse(str[0]), int.Parse(str[1]),
                        int.Parse(str[2]));
                }

            }

            return data;
        }

        static void WriteToFile(string fileName, List<int> cycle)
        {
            using (var outputFile = new StreamWriter(fileName))
            {
                if (cycle.Count == 0)
                {
                    outputFile.WriteLine("-1");
                }
                else
                {
                    outputFile.WriteLine("1");
                    outputFile.WriteLine(cycle.Count - 1);

                    foreach (var vertex in cycle)
                    {
                        outputFile.Write("{0} ", vertex);
                    }

                }
            }
        }

    }

    class InputData
    {
        public int NumberVertices { get; set; }
        public Edge[] Edges { get; set; }
    }

    struct Edge
    {
        public int Start { get; private set; }
        public int End { get; private set; }
        public int Weight { get; private set; }

        public Edge(int start, int end, int weight)
        {
            this.Start = start;
            this.End = end;
            this.Weight = weight;
        }
    }

    static class BellmanFordAlgorithm
    {
        public static List<int> FindNegativeCycle(Edge[] edges, int numberVertices)
        {
            var distances = new int[numberVertices];
            var previous = new int[numberVertices];
            int changed = -1;

            for (int i = 0; i < distances.Length; i++)
            {
                changed = -1;

                for (int j = 0; j < edges.Length; j++)
                {
                    var newDist = (long)distances[edges[j].Start] + edges[j].Weight;
                    if (newDist < distances[edges[j].End])
                    {
                        distances[edges[j].End] = (int)Math.Max(int.MinValue, newDist);
                        previous[edges[j].End] = edges[j].Start;
                        changed = edges[j].End;
                    }
                }

            }

            var path = new List<int>();
            if (changed != -1)
            {
                for (int i = 0; i < distances.Length; i++)
                {
                    changed = previous[changed];
                }

                int current = changed;

                do
                {
                    path.Add(current);
                    current = previous[current];
                } while (current != changed);

                path.Add(current);
                path.Reverse();
            }
            return path;
        }
    }
}