
namespace Lab5
{
    class Program
    {
        static void Main()
        {
            int length = 100000, numberOfTests = 20;
            var worker = new Worker();
            worker.ShowInfo(length, numberOfTests);
        }
    }
}