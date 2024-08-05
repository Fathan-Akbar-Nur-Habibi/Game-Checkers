using System;
using System.Collections.Generic;
namespace game_checkers

{

	public class King : CharPiece
	{
		public King(int id, Colour colour)
			: base(id, CharType.King, colour, 1, true)
		{
		}

		public override List<Destination> AvailableMove()
		{
			// Implement King-specific move logic here
			var moves = new List<Destination>();
			// Example move logic for a king
			return moves;
		}
	}

}
