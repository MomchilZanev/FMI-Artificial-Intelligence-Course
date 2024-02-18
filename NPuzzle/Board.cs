using System.Data;

namespace NPuzzle
{
    public enum Direction
    {
        left,
        right,
        up,
        down,
        none
    }

    class Board
    {
        public Board(int size, int zeroIndex, List<int> tiles)
        {
            this.TilesCount = size + 1;
            this.N = (int)Math.Sqrt(this.TilesCount);
            this.ZeroIndex = zeroIndex == -1 ? size : zeroIndex;
            this.Tiles = tiles;
            findCurIndex();
        }

        public int TilesCount { get; private set; }
        public int N { get; private set; }
        public int ZeroIndex { get; private set; } // The final position of the empty tile
        public int CurIndex { get; private set; } // The current position of the empty tile in the tiles list
        public int CurRow { get => this.CurIndex / this.N; } // The current row of the empty tile
        public int CurCol { get => this.CurIndex % this.N; } // The current column of the empty tile
        public List<int> Tiles { get; private set; } // The tiles in row-major order
        public int Manhattan // The Manhattan distance of the current board to the goal board 
        {
            get
            {
                int result = 0;
                for (int i = 0; i < this.TilesCount; ++i)
                {
                    int curTile = this.Tiles[i];
                    if (curTile == 0) continue;

                    int currentRow = i / this.N;
                    int currentCol = i % this.N;

                    int targetIndex = curTile > this.ZeroIndex ? curTile : curTile - 1;
                    int targetRow = targetIndex / this.N;
                    int targetCol = targetIndex % this.N;

                    result += Math.Abs(currentRow - targetRow) + Math.Abs(currentCol - targetCol);
                }

                return result;
            }
        }
        public bool IsGoal { get => this.Manhattan == 0; }
        public bool IsSolvable
        {
            get
            {
                int inversionCount = Board.getInversionCount(this.Tiles.Where(t => t != 0).ToList());
                if (this.N % 2 == 1) return inversionCount % 2 == 0;

                int targetBlankRow = this.ZeroIndex / this.N;

                return ((inversionCount + this.CurRow) % 2) == (targetBlankRow % 2);
            }
        }

        public bool CanMove(Direction direction)
        {
            switch (direction)
            {
                case Direction.left:
                    return this.CurCol != this.N - 1;
                case Direction.right:
                    return this.CurCol != 0;
                case Direction.up:
                    return this.CurRow != this.N - 1;
                case Direction.down:
                    return this.CurRow != 0;
                default:
                    return false;
            }
        }

        public void Move(Direction direction)
        {
            if (!this.CanMove(direction)) return;

            switch (direction)
            {
                case Direction.left:
                    this.swapTiles(this.CurIndex, this.CurIndex += 1);
                    break;
                case Direction.right:
                    this.swapTiles(this.CurIndex, this.CurIndex -= 1);
                    break;
                case Direction.up:
                    this.swapTiles(this.CurIndex, this.CurIndex += this.N);
                    break;
                case Direction.down:
                    this.swapTiles(this.CurIndex, this.CurIndex -= this.N);
                    break;
            }
        }

        private void swapTiles(int idA, int idB)
        {
            int temp = this.Tiles[idA];
            this.Tiles[idA] = this.Tiles[idB];
            this.Tiles[idB] = temp;
        }

        private void findCurIndex()
        {
            for (int i = 0; i < this.TilesCount; ++i)
            {
                if (this.Tiles[i] == 0)
                {
                    this.CurIndex = i;
                    break;
                }
            }
        }

        private static int inversionsAfterMerge(List<int> arr, List<int> left, List<int> right)
        {
            int leftIndex = 0, rightIndex = 0;
            int inversionsAfterMerge = 0;

            while (leftIndex < left.Count && rightIndex < right.Count)
            {
                if (left[leftIndex] > right[rightIndex])
                {
                    arr[leftIndex + rightIndex] = right[rightIndex];
                    inversionsAfterMerge += left.Count - leftIndex;
                    ++rightIndex;
                }
                else
                {
                    arr[leftIndex + rightIndex] = left[leftIndex];
                    ++leftIndex;
                }
            }

            while (rightIndex < right.Count)
            {
                arr[leftIndex + rightIndex] = right[rightIndex];
                ++rightIndex;
            }
            while (leftIndex < left.Count)
            {
                arr[leftIndex + rightIndex] = left[leftIndex];
                ++leftIndex;
            }

            return inversionsAfterMerge;
        }

        private static int getInversionCount(List<int> list)
        {
            if (list.Count < 2)
                return 0;

            int middle = (list.Count + 1) / 2;
            List<int> left = list.Take(middle).ToList();
            List<int> right = list.Skip(middle).ToList();

            return getInversionCount(left) + getInversionCount(right) + inversionsAfterMerge(list, left, right);
        }
    }
}