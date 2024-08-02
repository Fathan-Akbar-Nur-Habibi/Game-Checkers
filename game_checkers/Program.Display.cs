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
public void ShowBoard(){}

public void ShowTurnChanged(){}

public void ShowPieceMoved(){}

public void ShowPieceRemoved(){}

public void ShowpPieceUpgrade(){}

public void ShowGameEnd(){}

public void ShowPossibleMoves(){}

public void StartGame(){}
}
