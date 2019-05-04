
namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            var worker = new Worker();
            int length = 10000, startDel = 5000, stopDel = 7000, numberTest = 10;
            worker.ShowInfo(length, startDel, stopDel, numberTest);
        }
    }
}