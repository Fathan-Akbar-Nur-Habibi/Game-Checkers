using System.Collections.Generic;

namespace game_checkers
{
    public class Man : CharPiece
    {
        private readonly Board _board;

        public Man(int id, Colour colour, Board board)
            : base(id, CharType.Man, colour, 1, false)
        {
            _board = board;
        }

        public override List<Destination> AvailableMove(Destination currentDestination)
        {
            var moves = new List<Destination>();

            int direction = Colour == Colour.White ? -1 : 1;
            int startX = currentDestination.X;
            int startY = currentDestination.Y;

            AddMoveIfValid(moves, startX + direction, startY + 1);
            AddMoveIfValid(moves, startX + direction, startY - 1);

            return moves;
        }

        private void AddMoveIfValid(List<Destination> moves, int x, int y)
        {
            if (x >= 0 && x < 8 && y >= 0 && y < 8 && !_board.IsOccupied(new Destination(x, y)))
            {
                moves.Add(new Destination(x, y));
            }
        }
    }
}
