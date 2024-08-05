using System;
using System.Collections.Generic;
namespace game_checkers

{
	public class CharPiece : Piece
	{
		public int BaseAbility { get; }
		public bool WhichAbility { get; }

		public CharPiece(CharType type, Colour colour, int baseAbility, bool whichAbility)
			: base(0, type, colour)
		{
			BaseAbility = baseAbility;
			WhichAbility = whichAbility;
		}

		public CharPiece(int id, CharType type, Colour colour, int baseAbility, bool whichAbility)
			: base(id, type, colour)
		{
			BaseAbility = baseAbility;
			WhichAbility = whichAbility;
		}

		public int GetCurrentAbility() => BaseAbility;

		public int GetUpgrade() => BaseAbility + 1; // Simplified upgrade logic

		public override List<Destination> AvailableMove()
		{
			// Basic implementation for example purposes
			// You should replace this with actual chess or checkers movement logic
			var moves = new List<Destination>();

			// Example: Add some dummy moves for demonstration
			moves.Add(new Destination(1, 1));
			moves.Add(new Destination(2, 2));

			return moves;
		}
	}

}