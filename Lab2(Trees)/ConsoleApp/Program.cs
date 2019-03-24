

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            int length = 10000, startDel = 5000, stopDel = 7000, numberOfTests = 50;
            var worker = new Worker();
            worker.ShowInfo(length, startDel, stopDel, numberOfTests);
        }
    }
}