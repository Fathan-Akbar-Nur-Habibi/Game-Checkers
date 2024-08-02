using game_checkers;

public class Display { 

private GameController _gameController;
private Board _board; 
public Display(GameController gameController, Board
 board)
 {
 	_gameController = gameController; 
	_board = board;  
	SubscribeToEvent();
 }

private void SubscribeToEvent()
{
	_gameController.OnTurnChanged += ShowTurnChanged;
	_gameController.OnPieceMoved += ShowPieceMoved;
	_gameController.OnPieceRemoved += ShowPieceRemoved; 
	_gameController.OnPieceUpgrade += ShowpPieceUpgrade;
	_gameController.OnGameEnd += ShowGameEnd;
}
public void ShowBoard()
{
	var pieces = _board.GetBoard();
	for (int i=0; i<8; i++)
	{
		for (int j = 0; i<8; j++)
		{
			if (pieces[i,j]!= null)
			{
				Console.Write($"{pieces[i,j].Colour.ToString()[0]}{(pieces[i,j].Type == CharType.King ? 'K' :'M')}");
			}
			else { 
				Console.Write(". ");
			}
			}
			Console.WriteLine();
		}
		
	}
}

public void ShowTurnChanged(){}

public void ShowPieceMoved(int turn)
{
	Console.WriteLine($"Turn  change to player{turn}");
}

public void ShowPieceRemoved(Piece piece, Destination from, Destination to)
{
	Console.WriteLine($"{piece.Colour} piece move from ({from.x},{from.y})to ({to.x},{to.y})");
}

public void ShowpPieceUpgrade(IPlayer player, Piece piece)
{ 
	Console.WriteLine($"{piece.Colour} piece upgrade to King at {piece.GetType().Name}");
}

public void ShowGameEnd(IPlayer player)
{
	Console.WriteLine($"Game End! Winner : {player.Name}");
}
public void ShowPossibleMoves()
{ 
	var possibleMoves = _gameController.PossibbleMove(piece);
	Console.WriteLine($"Possible Move for {piece.Colour} {piece.Type}:");
		Console.WriteLine($"Possible moves for {piece.Colour} {piece.Type}:");
		foreach (var move in pissibleMoves) 
		{
			Console.WriteLine($"({move.x},{move.y})");
		}
	
}

public void StartGame(){
	ShowBoard();}
}
