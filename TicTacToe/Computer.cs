namespace TicTacToe
{
    public static class Computer
    {
        public static KeyValuePair<short, short> GetBestTurn(Board currentState)
        {
            Board startState = (Board)currentState.Clone();

            // Use alpha-beta search to find the best neighboring state
            Board? nextState = Computer.MinMaxValue(currentState, false, short.MinValue, short.MaxValue, 0, true).Value;

            if (nextState is null)
            {
                throw new Exception("No valid moves available!");
            }

            // Return the move that produces <nextState> from <startState>
            for (short i = 0; i < startState.Cells.Count; ++i)
            {
                for (short j = 0; j < startState.Cells[i].Count; ++j)
                {
                    if (startState.Cells[i][j] != nextState.Cells[i][j])
                        return new KeyValuePair<short, short>(i, j);
                }
            }

            throw new Exception("No valid moves available!");
        }

        // <minMaxToggle>: true => Max; false => Min
        private static KeyValuePair<short, Board?> MinMaxValue(Board boardState, bool playerTurn, short alpha, short beta, short depth, bool minMaxToggle)
        {
            // Terminal test
            if (boardState.HasWinner || !boardState.HasTurns)
            {
                // Return a neutral value if a tie is reached
                if (!boardState.HasWinner)
                    return new KeyValuePair<short, Board?>(0, boardState);

                // Return a positive value if the computer wins and negative value if the player wins
                // Use depth to encourage finishing the game earlier against a sub-optimal opponent
                short terminalValue = boardState.Winner == boardState.ComputerChar ? (short)(10 - depth) : (short)(depth - 10);

                return new KeyValuePair<short, Board?>(terminalValue, boardState);
            }

            // Explore neighbors and get Min/Max value
            KeyValuePair<short, Board?> value = new KeyValuePair<short, Board?>(minMaxToggle ? short.MinValue : short.MaxValue, null);
            for (short i = 0; i < boardState.Cells.Count; ++i)
            {
                for (short j = 0; j < boardState.Cells[i].Count; ++j)
                {
                    if (boardState.IsValidMove(i, j))
                    {
                        Board nextState = (Board)boardState.Clone();
                        nextState.MakeMove(i, j, playerTurn);

                        KeyValuePair<short, Board?> minMax = Computer.MinMaxValue(nextState, !playerTurn, alpha, beta, (short)(depth + 1), !minMaxToggle);
                        if ((minMaxToggle && minMax.Key > value.Key) || (!minMaxToggle && minMax.Key < value.Key))
                            value = new KeyValuePair<short, Board?>(minMax.Key, nextState);

                        // Prune next neighbors
                        if ((minMaxToggle && value.Key >= beta) || (!minMaxToggle && value.Key <= alpha))
                            return value;

                        alpha = minMaxToggle ? Math.Max(alpha, value.Key) : alpha;
                        beta = !minMaxToggle ? Math.Min(beta, value.Key) : beta;
                    }
                }
            }

            return value;
        }
    }
}