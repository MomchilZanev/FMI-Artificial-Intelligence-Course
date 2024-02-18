namespace KnapsackProblem
{
    public class KnapsackItem
    {
        public KnapsackItem(int mi, int ci)
        {
            this.Weight = mi;
            this.Value = ci;
        }

        public int Weight { get; set; }
        public int Value { get; set; }
    }
}