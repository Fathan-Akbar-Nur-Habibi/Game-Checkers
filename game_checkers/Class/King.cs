using System.Collections.Generic;

namespace GameCheckers
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
                        if (_board.IsOccupied(new Destination(x, y)))
                        {
                            Piece midPiece = _board.GetPiece(new Destination(x, y));
                            if (midPiece != null && midPiece.Colour != Colour)
                            {
                                int captureX = x + dx;
                                int captureY = y + dy;
                                if (IsValidMove(captureX, captureY) && !_board.IsOccupied(new Destination(captureX, captureY)))
                                {
                                    moves.Add(new Destination(captureX, captureY));
                                }
                            }
                            break;
                        }
                        moves.Add(new Destination(x, y));
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
