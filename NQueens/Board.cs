using System.Text;

namespace NQueens
{
    class Board
    {
        private readonly int k;
        private readonly int N;
        private bool hasConflicts;
        private readonly bool printArray;
        private Random random;
        private List<int> queens; // List containing the row index of each queen on a certain board column 
        private List<int> rowQueens; // List containing the number of queens on each board row
        private List<int> mainDiagonalQueens; // List containing the number of queens on every board diagonal "parallel" to the main diagonal
        private List<int> secondaryDiagonalQueens; // List containing the number of queens on every board diagonal "parallel" to the secondary diagonal

        public Board(int N, int k, bool printArray)
        {
            this.k = k;
            this.N = N;
            this.printArray = printArray;
            this.random = new Random();
            this.initialize();
        }

        public bool HasConflicts { get { return this.hasConflicts; } }
        public bool PrintArray { get { return this.printArray; } }
        public int MaxIterations { get { return this.k * this.N; } }
        public int DiagonalsCount { get { return (this.N * 2) - 1; } } // Number of diagonals parallel to the main diagonal

        public void Solve()
        {
            if (this.N == 2 || this.N == 3) return; // No solution

            for (int iteration = 0; iteration < this.MaxIterations; ++iteration)
            {
                int column = this.getColWithQueenWithMaxConf();
                if (!this.HasConflicts) return;
                int row = this.getRowWithMinConf(column);

                this.moveQueen(column, row);
            }

            if (this.HasConflicts)
            {
                this.initialize();
                this.Solve();
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            if (this.HasConflicts)
            {
                result.AppendLine("-1");
            }
            else if (this.PrintArray)
            {
                result.Append('[');
                for (int i = 0; i < this.N; ++i)
                {
                    if (i != this.N - 1)
                        result.Append(string.Format("{0}, ", this.queens[i]));
                    else
                        result.Append(this.queens[i]);
                }
                result.Append(']').AppendLine();

                return result.ToString();
            }
            else
            {
                for (int i = 0; i < this.N; ++i)
                {
                    var row = new StringBuilder();
                    for (int j = 0; j < this.N; ++j)
                    {
                        row.Append(this.queens[j] == i ? '*' : '_').Append(' ');
                    }
                    result.AppendLine(row.ToString().Trim());
                }
            }

            return result.ToString();
        }

        private void initialize()
        {
            this.hasConflicts = true;
            this.queens = new List<int>();
            this.rowQueens = Enumerable.Repeat(0, this.N).ToList();
            this.mainDiagonalQueens = Enumerable.Repeat(0, this.DiagonalsCount).ToList();
            this.secondaryDiagonalQueens = Enumerable.Repeat(0, this.DiagonalsCount).ToList();

            for (int i = 0; i < this.N; ++i)
            {
                this.addQueen(i, this.random.Next(0, this.N));
            }
        }

        private void addQueen(int column, int row)
        {
            this.queens.Add(row);
            this.rowQueens[row]++;
            this.mainDiagonalQueens[this.getMainDiagonalIndex(column, row)]++;
            this.secondaryDiagonalQueens[this.getSecondaryDiagonalIndex(column, row)]++;
        }

        private void moveQueen(int column, int newRow)
        {
            int currentRow = this.queens[column];
            this.rowQueens[currentRow]--;
            this.mainDiagonalQueens[this.getMainDiagonalIndex(column, currentRow)]--;
            this.secondaryDiagonalQueens[this.getSecondaryDiagonalIndex(column, currentRow)]--;

            this.queens[column] = newRow;
            this.rowQueens[newRow]++;
            this.mainDiagonalQueens[this.getMainDiagonalIndex(column, newRow)]++;
            this.secondaryDiagonalQueens[this.getSecondaryDiagonalIndex(column, newRow)]++;
        }

        private int getRowWithMinConf(int column)
        {
            int minRowConflicts = int.MaxValue;
            List<int> rowsWithMinConflicts = new List<int>();

            for (int row = 0; row < this.N; ++row)
            {
                var currentRowConflicts = this.getRowConflicts(column, row);

                if (currentRowConflicts < minRowConflicts)
                {
                    minRowConflicts = currentRowConflicts;
                    rowsWithMinConflicts.Clear();
                    rowsWithMinConflicts.Add(row);
                }
                else if (currentRowConflicts == minRowConflicts)
                {
                    rowsWithMinConflicts.Add(row);
                }
            }

            return rowsWithMinConflicts[this.random.Next(0, rowsWithMinConflicts.Count)];
        }

        private int getColWithQueenWithMaxConf()
        {
            int maxColumnConflicts = int.MinValue;
            List<int> columnsWithMaxConflicts = new List<int>();

            for (int column = 0; column < this.N; ++column)
            {
                var currentColumnConflicts = this.getColumnConflicts(column);

                if (currentColumnConflicts > maxColumnConflicts)
                {
                    maxColumnConflicts = currentColumnConflicts;
                    columnsWithMaxConflicts.Clear();
                    columnsWithMaxConflicts.Add(column);
                }
                else if (currentColumnConflicts == maxColumnConflicts)
                {
                    columnsWithMaxConflicts.Add(column);
                }
            }

            if (maxColumnConflicts == 0) this.hasConflicts = false;

            return columnsWithMaxConflicts[this.random.Next(0, columnsWithMaxConflicts.Count)];
        }

        private int getColumnConflicts(int column)
        {
            int row = this.queens[column];

            // Account for the queen in the current column as it appears once in each list
            int conflicts = this.rowQueens[row] + this.mainDiagonalQueens[this.getMainDiagonalIndex(column, row)] + this.secondaryDiagonalQueens[this.getSecondaryDiagonalIndex(column, row)] - 3;

            return conflicts;
        }

        private int getRowConflicts(int column, int row)
        {
            int conflicts = this.rowQueens[row] + this.mainDiagonalQueens[this.getMainDiagonalIndex(column, row)] + this.secondaryDiagonalQueens[this.getSecondaryDiagonalIndex(column, row)];

            conflicts -= this.queens[column] == row ? 3 : 0; // Account for the queen if checking the row where she resides

            return conflicts;
        }

        private int getMainDiagonalIndex(int column, int row)
        {
            return column - row + this.N - 1;
        }

        private int getSecondaryDiagonalIndex(int column, int row)
        {
            return column + row;
        }
    }
}