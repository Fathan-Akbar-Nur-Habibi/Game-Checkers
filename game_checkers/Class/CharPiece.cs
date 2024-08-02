namespace game_checkers.Class;

public class CharPiece : Piece
{
    public int BaseAbility { get; }
    public bool WhichAbility { get; }

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