using System;
using System.Collections.Generic;
using System.IO;

namespace TaskA
{
    class Program
    {
        static void Main()
        {
            var inputData = ReadFromFile("input.txt");
            byte[] parentCity;
            var times = DijkstrasAlgorithm.GetMinTime(inputData.Map, inputData.Start, out parentCity);
            inputData.Map = null;
            var time = times[inputData.End];
            var path = new List<int>();
            if (time != int.MaxValue)
            {
                var city = inputData.End;
                while (city != inputData.Start)
                {
                    path.Add(city);
                    city = parentCity[city];
                }
                path.Add(inputData.Start);
            }
            else
            {
                time = -1;
            }

            WriteToFile("output.txt", time, path);
        }

        static InputData ReadFromFile(string fileName)
        {
            var data = new InputData();
            using (var inputFile = new StreamReader(fileName))
            {
                var sep = new char[] { ' ' };
                var option = StringSplitOptions.RemoveEmptyEntries;
                var str = inputFile.ReadLine().Split(sep, option);
                data.Map = new CountryMap(int.Parse(str[0]) + 1);
                str = inputFile.ReadLine().Split(sep, option);
                data.Start = byte.Parse(str[0]);
                if (data.Map.NumberCities == 2)
                {
                    data.End = 0;
                }
                else
                {
                    data.End = byte.Parse(str[1]);
                }
                str = inputFile.ReadLine().Split(sep, option);
                int trainCount = int.Parse(str[0]);

                for (int i = 0; i < trainCount; i++)
                {
                    str = inputFile.ReadLine().Split(sep, option);
                    data.Map.AddTrain(byte.Parse(str[0]), byte.Parse(str[1]),
                        int.Parse(str[2]), int.Parse(str[3]));
                }

            }
            return data;
        }

        static void WriteToFile(string fileName, int time, List<int> reversePath)
        {
            using (var outputFile = new StreamWriter(fileName))
            {
                outputFile.WriteLine(time);

                for (int i = reversePath.Count - 1; i >= 0; i--)
                {
                    outputFile.Write("{0} ", reversePath[i]);
                }

            }
        }
    }

    class InputData
    {
        public CountryMap Map { get; set; }
        public byte Start { get; set; }
        public byte End { get; set; }
    }

    class CountryMap
    {
        private List<Train>[] _adjacencyList;
        public int NumberCities => _adjacencyList.Length;
        public List<Train> this[int city]
        {
            get
            {
                return _adjacencyList[city];
            }
        }

        public CountryMap(int numberCities)
        {
            _adjacencyList = new List<Train>[numberCities];

            for (int i = 0; i < _adjacencyList.Length; i++)
            {
                _adjacencyList[i] = new List<Train>();
            }
        }

        public void AddTrain(byte start, byte destination, int departureTime, int arrivalTime)
        {
            _adjacencyList[start].Add(new Train(destination, departureTime, arrivalTime));
        }

    }

    class Train
    {
        public byte Destination { get; private set; }
        public int DepartureTime { get; private set; }
        public int ArrivalTime { get; private set; }
        public Train(byte destination, int departureTime, int arrivalTime)
        {
            this.Destination = destination;
            this.DepartureTime = departureTime;
            this.ArrivalTime = arrivalTime;
        }
    }

    static class DijkstrasAlgorithm
    {
        public static int[] GetMinTime(CountryMap map, byte start, out byte[] parentCity)
        {
            var times = new int[map.NumberCities];
            parentCity = new byte[map.NumberCities];
            var visited = new bool[map.NumberCities];

            for (int i = 0; i < times.Length; i++)
            {
                times[i] = int.MaxValue;
            }

            times[start] = 0;

            while (true)
            {
                int minTime = int.MaxValue;
                byte city = 0;

                for (byte i = 0; i < times.Length; i++)
                {
                    if (!visited[i])
                    {
                        if (times[i] < minTime)
                        {
                            minTime = times[i];
                            city = i;
                        }
                    }
                }

                if (minTime < int.MaxValue)
                {
                    visited[city] = true;
                    foreach (var train in map[city])
                    {
                        if (times[city] <= train.DepartureTime)
                        {
                            if (train.ArrivalTime < times[train.Destination])
                            {
                                times[train.Destination] = train.ArrivalTime;
                                parentCity[train.Destination] = city;
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            return times;
        }
    }
}