namespace game_checkers.Class;

public abstract class Board
{
    private Piece[,] _pieces;

    protected Board(Piece[,] pieces)
    {
        _pieces = pieces;
    }

    public Piece[,] GetBoard()
    {
        return _pieces;
    }

    public bool IsOccupied(Destination destination)
    {
        return _pieces[destination.x, destination.y] != null;
    }

    public Destination GetDestination(Piece piece)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (_pieces[i, j] == piece)
                {
                    return new Destination(i, j);
                }
            }
        }
        return null;
    }

    public Piece GetPiece(Destination destination)
    {
        return _pieces[destination.x, destination.y];
    }

    public void SetPlacePiece(Piece piece, Destination destination)
    {
        _pieces[destination.x, destination.y] = piece;
    }

    public abstract IPlayer GetWinner();

    public abstract bool HasPlayer(IPlayer player);
}
