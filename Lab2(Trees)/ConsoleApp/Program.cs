

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            int length = 100000, startDel = 50000, stopDel = 70000, numberOfTests = 50;
            var worker = new Worker();
            worker.ShowInfo(length, startDel, stopDel, numberOfTests);
        }
    }
}