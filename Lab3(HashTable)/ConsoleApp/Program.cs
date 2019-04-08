using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            var worker = new Worker();
            int i;
            for ( i = 1; i < 11; i++)
            {
                Console.WriteLine("Test {0}:", i);
                worker.RunBenchmark();
                Console.WriteLine();
            }
        }
    }
}
