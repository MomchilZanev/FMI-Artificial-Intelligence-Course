namespace KnapsackProblem
{
    public class GeneticKnapsackSolver
    {
        private readonly List<KnapsackItem> items;
        private readonly int maxKnapsackWeight;

        private readonly int poolSize;
        private readonly int selectionSize;
        private readonly double mutationChance;
        private List<PotentialSolution> pool;

        private readonly Random random;

        public GeneticKnapsackSolver(List<KnapsackItem> items, int M, int poolSize, int selectionSize, double mutationChance)
        {
            this.items = items;
            this.maxKnapsackWeight = M;

            this.poolSize = poolSize;
            this.selectionSize = selectionSize;
            this.mutationChance = mutationChance;
            this.pool = new List<PotentialSolution>();

            this.random = new Random();

            this.generateInitialPool();
        }

        private void generateInitialPool()
        {
            for (int i = 0; i < poolSize; ++i)
            {
                this.pool.Add(this.generatePotentialSolution());
            }
        }

        private PotentialSolution generatePotentialSolution()
        {
            PotentialSolution solution = new PotentialSolution(this.items.Count);

            for (int i = 0; i < this.items.Count; ++i)
            {
                int index = this.random.Next(this.items.Count);

                if (random.NextDouble() < 0.5 && solution.Weight + this.items[index].Weight <= this.maxKnapsackWeight)
                {
                    solution.SetItem(index, true, this.items[index]);
                }
            }

            return solution;
        }

        private PotentialSolution UniformCrossover(PotentialSolution parent1, PotentialSolution parent2)
        {
            PotentialSolution child = new PotentialSolution(this.items.Count);

            for (int i = 0; i < parent1.Items.Count; i++)
            {
                if (random.NextDouble() < 0.5)
                {
                    child.SetItem(i, parent1.Items[i], this.items[i]);
                }
                else
                {
                    child.SetItem(i, parent2.Items[i], this.items[i]);
                }
            }

            // Remove random items until solution does not exceed max weight
            while (child.Weight > this.maxKnapsackWeight)
            {
                int indexToRemove = random.Next(child.Items.Count);
                child.SetItem(indexToRemove, false, this.items[indexToRemove]);
            }

            return child;
        }

        private void Mutate(PotentialSolution solution)
        {
            for (int i = 0; i < solution.Items.Count; ++i)
            {
                if (random.NextDouble() < this.mutationChance)
                {
                    solution.SetItem(i, !solution.Items[i], this.items[i]);

                    // Revert mutation if solution was made invalid
                    if (solution.Weight > this.maxKnapsackWeight)
                        solution.SetItem(i, !solution.Items[i], this.items[i]);
                }
            }
        }

        public void Solve(int maxGenerations)
        {
            for (int t = 0; t < maxGenerations; ++t)
            {
                // Sort the pool by fitness
                this.pool = this.pool.OrderByDescending(s => s.Fitness).ToList();

                // Remove <selectionSize> weakest
                this.pool = this.pool.Take(this.poolSize - this.selectionSize).ToList();

                // Take <selectionSize> best and pair them randomly
                var temp = this.pool.Take(this.selectionSize).ToList();
                List<KeyValuePair<PotentialSolution, PotentialSolution>> selection = new List<KeyValuePair<PotentialSolution, PotentialSolution>>();
                while (temp.Count > 0)
                {
                    int randomIndex = this.random.Next(temp.Count);
                    var firstParent = temp[randomIndex];
                    temp.RemoveAt(randomIndex);

                    randomIndex = this.random.Next(temp.Count);
                    var secondParent = temp[randomIndex];
                    temp.RemoveAt(randomIndex);

                    selection.Add(new KeyValuePair<PotentialSolution, PotentialSolution>(firstParent, secondParent));
                }

                // Crossover and Mutation
                for (int i = 0; i < this.selectionSize / 2; ++i)
                {
                    PotentialSolution child1 = UniformCrossover(selection[i].Key, selection[i].Value);
                    PotentialSolution child2 = UniformCrossover(selection[i].Value, selection[i].Key);

                    this.Mutate(child1);
                    this.Mutate(child2);

                    this.pool.Add(child1);
                    this.pool.Add(child2);
                }

                // Print even generations
                if (t % 2 == 0)
                    Console.WriteLine(this.pool.Max(s => s.Fitness));
            }

            // If max generations are odd, the last generation was printed in the for loop
            if (maxGenerations % 2 == 0)
                Console.WriteLine(this.pool.Max(s => s.Fitness));
        }
    }
}