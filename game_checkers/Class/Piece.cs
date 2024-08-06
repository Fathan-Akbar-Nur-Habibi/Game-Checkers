using System.Collections.Generic;
namespace game_checkers

{
	public abstract class Piece
	{
		public int Id { get; }
		public CharType Type { get; }
		public Colour Colour { get; }
		//public bool IsChangeable { get; set; }

		protected Piece(int id, CharType type, Colour colour)
		{
			Id = id;
			Type = type;
			Colour = colour;
		}

		public abstract List<Destination> AvailableMove();
	}
}