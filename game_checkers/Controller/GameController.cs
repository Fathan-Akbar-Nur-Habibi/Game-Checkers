namespace game_checkers;
public class GameController { 
	
	public int PieceOnBoard;
	public int Turn;
	private Dictionary<IPlayer, List<CharPiece>> _pieceOnBoard;
	private Dictionary<IPlayer, List<CharPiece>> _pieceRemoved;
	public event Action <int> OnTurnChanged;
	public event Action<IPlayer, Piece> OnPieceUpgrade;
	public event Action <Piece, Destination, Destination> OnPieceMoved;
	public event Action<Piece, Destination> OnPieceRemoved;
	public event Action <IPlayer> OnGameEnded;
	
	private IPlayer _player1;
	private IPlayer _player2;
	private Board _board;
	
	public GameController( IPlayer player, Piece piece, Board board)
	{
		_player1 = _player1;
		_player2 = _player2;
		_board = board;
		
		_pieceOnBoard = new Dictionary<IPlayer, List<CharPiece>>;
		_pieceRemoved = new Dictionary<IPlayer, List<CharPiece>>; 
		
		Turn =1;
		PieceOnBoard =24;
		_pieceOnBoard[_player1] = new List<CharPiece>;
		_pieceOnBoard[_player2] = new List<CharPiece>;
		
		_pieceRemoved[_player1] = new List<CharPiece>>; 
		_pieceRemoved[_player2] = new List<CharPiece>>; 
	}  
	public bool ChangeTurn()
	{
		Turn = Turn == 1? 2:1;
		OnTurnChanged?.Invoke(Turn);
	}
	public List<IPlayer> GetPlayers()
	{
		return new List<IPlayer> {_player1, _player2};
	}
	public bool CheckPlayer(IPlayer player)
	{
		return player == _player1 || player == _player2 ;
	}
	public bool HasPieceOnBoard(IPlayer player, Piece piece)
	{
		return _pieceOnBoard[player].Contains(piece as CharPiece);
	}
	public List<Destination> PossibbleMove(IPlayer player, Piece piece)
	{
		if (!CheckPlayer(player) || !HasPieceOnBoard(player, piece))
		{
			return new List<Destination>();
		}
		return piece.AvailableMove();
	}
	public bool MakeMove(IPlayer player, Piece piece, Destination from, Destination to)
	{
		if (!CheckPlayer(player) || !HashPieceOnBoard(player, piece))
		{
			return false;
		}
		
	}
	public bool RemovePiece(IPlayer player, Piece piece) 
	{
		if (!CheckPlayer(player) || !HasPieceOnBoard(player, piece))
		{
			return false;
		}
		_pieceOnBoard[player].Remove(piece as CharPiece);
		_pieceRemoved[player].Add(piece as CharPiece);
		PieceOnBoard--;
		OnPieceRemoved?.Invoke(piece, _board.GetDistanation(piece));
		
	}
	public bool ChangeCharType(IPlayer player, Piece piece)
	{
		if (!CheckPlayer(player) || !HasPieceOnBoard(player, piece))
		{
			return false;
		}
		charPiece Type = CharType.King;
		OnPieceUpgrade?.Invoke(player, piece);
		
	}
	public void NotifyCharTypeChanged(IPlayer player,Piece target) 
	{ 
		OnPieceUpgrade?.Invoke(player, target);
	}
}