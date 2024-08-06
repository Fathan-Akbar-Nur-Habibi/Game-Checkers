using System.Collections.Generic;

namespace game_checkers
{
    public class King : CharPiece
    {
        private readonly Board _board;

        public King(int id, Colour colour, Board board)
            : base(id, CharType.King, colour, 1, true)
        {
            _board = board;
        }

        public override List<Destination> AvailableMove(Destination currentDestination)
        {
            var moves = new List<Destination>();

            int[] directions = { -1, 1 };
            foreach (int dx in directions)
            {
                foreach (int dy in directions)
                {
                    int x = currentDestination.X + dx;
                    int y = currentDestination.Y + dy;
                    while (IsValidMove(x, y))
                    {
                        moves.Add(new Destination(x, y));
                        if (_board.IsOccupied(new Destination(x, y)))
                        {
                            break;
                        }
                        x += dx;
                        y += dy;
                    }
                }
            }
            return moves;
        }

        private bool IsValidMove(int x, int y)
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }
    }
}
