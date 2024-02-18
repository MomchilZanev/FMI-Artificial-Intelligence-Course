namespace NPuzzle
{
    class Solver
    {
        private Board board;

        public Solver(Board board)
        {
            this.Solved = false;
            this.Path = new Stack<Direction>();
            this.Path.Push(Direction.none);
            this.board = board;
        }

        public bool Solved { get; private set; }
        public Stack<Direction> Path { get; private set; }

        // Use the IDA* search algorithm to find the shortest path to the solved board state
        public void Solve()
        {
            if (!this.board.IsSolvable)
            {
                Console.WriteLine(-1);
                return;
            }

            int threshold = this.board.Manhattan;
            while (!this.Solved)
            {
                threshold = this.exploreNeighbors(0, threshold);
            }
        }

        private void exploreDirection(Direction direction, ref int curCost, ref int threshold, ref int minCost)
        {
            if (direction == Direction.none
                || !this.board.CanMove(direction)
                || this.Path.Peek() == GetOppositeDirection(direction)
                ) return;
            this.board.Move(direction);
            this.Path.Push(direction);

            int val = this.exploreNeighbors(curCost + 1, threshold);
            minCost = val < minCost ? val : minCost;

            this.board.Move(GetOppositeDirection(direction));
            this.Path.Pop();
        }

        private int exploreNeighbors(int curCost, int threshold)
        {
            int estimatedCost = curCost + this.board.Manhattan;

            // Prune search node if the estimated cost is greater than threshold.
            if (estimatedCost > threshold) return estimatedCost;

            if (this.board.IsGoal)
            {
                this.Solved = true;
                this.printResult();
                return -1;
            }

            // Explore neighbors without backtracking and return the minimum pruned value
            int minCost = int.MaxValue;
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                this.exploreDirection(direction, ref curCost, ref threshold, ref minCost);
                if (this.Solved) return -1;
            }

            return minCost;
        }

        private void printResult()
        {
            Console.WriteLine(this.Path.Count - 1);
            foreach (Direction step in this.Path.Where(s => s != Direction.none).Reverse())
            {
                Console.WriteLine(DirectionToString(step));
            }
        }

        private static string DirectionToString(Direction direction)
        {
            switch (direction)
            {
                case Direction.left:
                    return "left";
                case Direction.right:
                    return "right";
                case Direction.up:
                    return "up";
                case Direction.down:
                    return "down";
                default:
                    return String.Empty;
            }
        }

        private static Direction GetOppositeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.left:
                    return Direction.right;
                case Direction.right:
                    return Direction.left;
                case Direction.up:
                    return Direction.down;
                case Direction.down:
                    return Direction.up;
                default:
                    return Direction.none;
            }
        }
    }
}