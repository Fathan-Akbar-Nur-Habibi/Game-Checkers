using System.Collections.Generic;
namespace game_checkers
{
	public class Man : CharPiece
	{
		public Man(int id, Colour colour)
			: base(id, CharType.Man, colour, 1, false)
		{
		}

		public override List<Destination> AvailableMove()
		{
			// Implement man-specific move logic here
			var moves = new List<Destination>();
			// Example move logic for a man
			return moves;
		}
	}
}
