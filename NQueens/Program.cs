using System.Diagnostics;
using System.Globalization;

namespace NQueens
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = int.Parse(Console.ReadLine() ?? "0");
            bool printArray = true;
            Board board = new Board(N, 1, printArray);

            if (N > 100)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                board.Solve();
                stopwatch.Stop();
                Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0:0.00}", stopwatch.ElapsedMilliseconds / 1000.0));
            }
            else
            {
                board.Solve();
                Console.Write(board.ToString());
            }
        }
    }
}