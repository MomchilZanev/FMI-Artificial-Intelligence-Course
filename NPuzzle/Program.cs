using System.Diagnostics;

namespace NPuzzle
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = int.Parse(Console.ReadLine() ?? "");
            int zeroIndex = int.Parse(Console.ReadLine() ?? "");
            List<int> tiles = new List<int>();
            int N = (int)Math.Sqrt(size + 1);
            for (int i = 0; i < N; ++i)
            {
                tiles.AddRange((Console.ReadLine() ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)));
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Board board = new Board(size, zeroIndex, tiles);
            Solver solver = new Solver(board);
            solver.Solve();

            stopwatch.Stop();
            TimeSpan timeToSolve = stopwatch.Elapsed;
        }
    }
}