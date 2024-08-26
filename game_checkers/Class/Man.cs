using System.Collections.Generic;

namespace GameCheckers
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
			AddCaptureMoves(moves, startX, startY, direction);

			return moves;
		}

		private void AddMoveIfValid(List<Destination> moves, int x, int y)
		{
			if (IsWithinBounds(x, y) && !_board.IsOccupied(new Destination(x, y)))
			{
				moves.Add(new Destination(x, y));
			}
		}

		private void AddCaptureMoves(List<Destination> moves, int startX, int startY, int direction)
		{
			AddCaptureMove(moves, startX, startY, direction, 1); // Capture diagonally right
			AddCaptureMove(moves, startX, startY, direction, -1); // Capture diagonally left

		}

		private void AddCaptureMove(List<Destination> moves, int startX, int startY, int direction, int side)
		{


			int midX = startX + direction;
			int midY = startY + side;
			int destX = startX + 2 * direction;
			int destY = startY + 2 * side;

			if (IsWithinBounds(destX, destY) && _board.IsOccupied(new Destination(midX, midY)))

			{
				Piece midPiece = _board.GetPiece(new Destination(midX, midY));
				if (midPiece != null && midPiece.Colour != Colour && !_board.IsOccupied(new Destination(destX, destY)))
		
				{
					moves.Add(new Destination(destX, destY));
				}
			}
		}

		private bool IsWithinBounds(int x, int y)
		{
			return x >= 0 && x < 8 && y >= 0 && y < 8;
		}
	}
}
