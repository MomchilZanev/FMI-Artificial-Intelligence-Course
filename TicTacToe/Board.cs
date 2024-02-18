using System.Text;

namespace TicTacToe
{
    public class Board : ICloneable
    {
        public Board(char empptyCellChar, char computerChar, char playerChar)
        {
            this.EmpptyCellChar = empptyCellChar;
            this.ComputerChar = computerChar;
            this.PlayerChar = playerChar;
            this.Cells = new List<List<char>>
            {
                new List<char> { this.EmpptyCellChar, this.EmpptyCellChar, this.EmpptyCellChar },
                new List<char> { this.EmpptyCellChar, this.EmpptyCellChar, this.EmpptyCellChar },
                new List<char> { this.EmpptyCellChar, this.EmpptyCellChar, this.EmpptyCellChar }
            };
            this.Winner = this.EmpptyCellChar;
        }

        public char EmpptyCellChar { get; private set; }
        public char ComputerChar { get; private set; }
        public char PlayerChar { get; private set; }
        public List<List<char>> Cells { get; private set; }
        public char Winner { get; private set; }

        public bool HasTurns { get => this.Cells.Any(row => row.Any(cell => cell == '-')); }
        public bool HasWinner { get => this.Winner != this.EmpptyCellChar; }

        public void MakeMove(short row, short col, bool playerTurn)
        {
            if (this.IsValidMove(row, col))
            {
                this.Cells[row][col] = playerTurn ? this.PlayerChar : this.ComputerChar;
                this.checkForWinner(row, col);
            }
        }

        public bool IsValidMove(short row, short col)
        {
            return 0 <= row && row < 3 && 0 <= col && col < 3 && this.Cells[row][col] == this.EmpptyCellChar;
        }

        private void checkForWinner(short row, short col)
        {
            // Check row
            if (this.Cells[row].All(c => c == this.Cells[row][col]))
            {
                this.Winner = this.Cells[row][col];
            }

            // Check column
            if (this.Cells.All(r => r[col] == this.Cells[row][col]))
            {
                this.Winner = this.Cells[row][col];
            }

            // Check diagonals
            if (row - col == 0 || row + col == 2)
            {
                // Primary diagonal
                if (this.Cells[0][0] == this.Cells[1][1] && this.Cells[1][1] == this.Cells[2][2])
                {
                    this.Winner = this.Cells[1][1];
                }

                // Secondary diagonal
                if (this.Cells[0][2] == this.Cells[1][1] && this.Cells[1][1] == this.Cells[2][0])
                {
                    this.Winner = this.Cells[1][1];
                }
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("Board:");
            foreach (var row in this.Cells)
            {
                result.AppendLine(string.Join(' ', row));
            }

            return result.ToString().Trim();
        }

        public object Clone()
        {
            Board board = new Board(this.EmpptyCellChar, this.ComputerChar, this.PlayerChar);

            for (int i = 0; i < this.Cells.Count; ++i)
            {
                for (int j = 0; j < this.Cells[i].Count; ++j)
                {
                    board.Cells[i][j] = this.Cells[i][j];
                }
            }
            board.Winner = this.Winner;

            return board;
        }
    }
}