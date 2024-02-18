namespace TicTacToe
{
    public class Program
    {
        static void Main(string[] args)
        {
            const char empptyCellChar = '-';
            const char computerChar = 'O';
            const char playerChar = 'X';

            bool play = true;
            while (play)
            {
                Console.Clear();

                Console.WriteLine(string.Format("Computer: {0}", computerChar));
                Console.WriteLine(string.Format("Player: {0}", playerChar));
                Board board = new Board(empptyCellChar, computerChar, playerChar);

                Console.WriteLine(board);

                Console.WriteLine("Would you like to start first? [Y/N]");
                bool playerTurn = (Console.ReadLine() ?? "n").ToLower().Trim() == "y";

                while (board.HasTurns && !board.HasWinner)
                {
                    while (playerTurn)
                    {
                        Console.WriteLine("Your move? [row col]");
                        List<short> tokens = (Console.ReadLine() ?? "0 0").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(t => short.Parse(t)).ToList();
                        short row = tokens.FirstOrDefault();
                        short col = tokens.LastOrDefault();
                        if (!board.IsValidMove(row, col))
                        {
                            Console.WriteLine("Invalid move!");
                            continue;
                        }
                        board.MakeMove(row, col, playerTurn);
                        playerTurn = false;
                    }

                    if (!board.HasTurns || board.HasWinner)
                        break;
                    KeyValuePair<short, short> computerMove = Computer.GetBestTurn(board);
                    board.MakeMove(computerMove.Key, computerMove.Value, playerTurn);

                    Console.WriteLine(board);
                    playerTurn = true;
                }

                if (board.HasWinner)
                {
                    Console.WriteLine(string.Format("{0} wins!", board.Winner));
                }
                else
                {
                    Console.WriteLine("Tie.");
                }

                Console.WriteLine("Play again? [Y/N]");
                play = (Console.ReadLine() ?? "n").ToLower().Trim() == "y";
            }
        }
    }
}