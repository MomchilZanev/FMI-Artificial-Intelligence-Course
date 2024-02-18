namespace KnapsackProblem
{
    public class Program
    {
        static void Main(string[] args)
        {
            int M, N;
            List<int> tokens = (Console.ReadLine() ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
            M = tokens[0];
            N = tokens[1];

            List<KnapsackItem> items = new List<KnapsackItem>();
            for (int i = 0; i < N; ++i)
            {
                tokens = (Console.ReadLine() ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
                int mi = tokens[0];
                int ci = tokens[1];

                KnapsackItem item = new KnapsackItem(mi, ci);
                items.Add(item);
            }

            int poolSize = 10000;
            int selectionSize = 2000;
            int generations = 25;
            double mutationChance = 0.01;

            var solver = new GeneticKnapsackSolver(items, M, poolSize, selectionSize, mutationChance);
            solver.Solve(generations);
        }
    }
}