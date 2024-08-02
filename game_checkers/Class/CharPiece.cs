using System.Text.Json;

namespace game_checkers;

public class CharPiece : Piece
{
	public int BaseAbility { get; }
	public bool WhichAbility { get; }
	public static bool TrueKing {get; private set;}
	static CharPiece()
	{
		LoadConfiguration();
	}
	
	private static void LoadConfiguration()
	{
		var configText = System.IO.File.ReadAllText("config.json");
		var config = JsonSerializer.Deserialize<Dictionary<string,bool>>(configText);
		if (config !=null && config.ContainsKey("trueKing"))
		{
			TrueKing = config["trueKing"];
		}
	}

	public CharPiece(int id, CharType type, Colour colour, int baseAbility, bool whichAbility)
		: base(id, type, colour)
	{
		BaseAbility = baseAbility;
		WhichAbility = whichAbility;
	}

	public int GetCurrentAbility()
	{
		return BaseAbility + (WhichAbility ? 1 : 0);
	}

	public int GetUpgrade()
	{
		return BaseAbility + 1;
	}

	public override List<Destination> AvailableMove()
	{
		
		return new List<Destination>();
	}
}