namespace KnapsackProblem
{
    public class PotentialSolution
    {
        public PotentialSolution(int itemsCount)
        {
            Items = Enumerable.Repeat(false, itemsCount).ToList();
            this.Weight = 0;
            this.Fitness = 0;
        }

        public List<bool> Items { get; set; }
        public int Fitness { get; set; }
        public int Weight { get; set; }

        public void SetItem(int index, bool value, KnapsackItem item)
        {
            if (this.Items[index] == value) return;

            if (value)
            {
                this.Items[index] = value;
                this.Fitness += item.Value;
                this.Weight += item.Weight;
            }
            else
            {
                this.Items[index] = value;
                this.Fitness -= item.Value;
                this.Weight -= item.Weight;
            }
        }
    }
}