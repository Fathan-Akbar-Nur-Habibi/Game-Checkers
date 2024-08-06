using System;
using System.Collections.Generic;
namespace game_checkers 

{
class Program
{
	static void Main()
	{
		IPlayer player1 = new Player(1, "White");
		IPlayer player2 = new Player(2, "Red");
		Piece[,] pieces = new Piece[8, 8];

		// Initialize pieces on the board
		InitializeBoard(pieces);

		Board board = new ConcreteBoard(pieces);
		GameController gameController = new GameController(player1, player2, board);

		gameController.OnTurnChanged += (turn) => Console.WriteLine($"{(turn == 0 ? "White" : "Red")}'s turn. Enter move (e.g., 'a2 a3' to move piece from a2 to a3) or type 'exit' to quit: ");
		gameController.OnPieceMoved += (piece, from, to) => Console.WriteLine($"Piece moved from {from.X + 1},{(char)(from.Y + 'a')} to {to.X + 1},{(char)(to.Y + 'a')}");
		gameController.OnPieceRemoved += (piece, destination) => Console.WriteLine($"Piece removed from {destination.X + 1},{(char)(destination.Y + 'a')}");
		gameController.OnGameEnded += (player) => Console.WriteLine($"Game ended! Winner: {player.Name}");

		Console.WriteLine("Checkers Game");
		PrintBoard(board);

		// Game loop
		bool gameRunning = true;
		while (gameRunning)
		{
			Console.Write($"{(gameController.Turn == 0 ? "White" : "Red")}'s turn. Enter move (e.g., 'a2 a3'): ");
			string input = Console.ReadLine();
			if (input.ToLower() == "exit")
			{
				gameRunning = false;
				break;
			}

			var moveParts = input.Split(' ');

			if (moveParts.Length == 2)
			{
				var from = ParseDestination(moveParts[0]);
				var to = ParseDestination(moveParts[1]);

				if (from != null && to != null)
				{
					var piece = board.GetPiece(from);
					if (piece != null && piece.Colour == (gameController.Turn == 0 ? Colour.White : Colour.Red))
					{
						if (gameController.MakeMove(gameController.GetPlayers()[gameController.Turn], piece, from, to))
						{
							PrintBoard(board);
							var winner = board.GetWinner();
							if (winner != null)
							{
								gameRunning = false;
							}
							else
							{
								gameController.ChangeTurn();
							}
						}
						else
						{
							Console.WriteLine("Invalid move. Try again.");
						}
					}
					else
					{
						Console.WriteLine("No piece at the starting position or not your piece.");
					}
				}
				else
				{
					Console.WriteLine("Invalid input. Use format 'a2 a3'.");
				}
			}
			else
			{
				Console.WriteLine("Invalid input. Use format 'a2 a3'.");
			}
		}
	}

	static void InitializeBoard(Piece[,] pieces)
	{
		for (int i = 0; i < 8; i += 2)
		{
			pieces[0, i + 1] = new Man(1, Colour.Red); // row 1 (index 0), columns 1, 3, 5, 7
			pieces[1, i] = new Man(1, Colour.Red); // row 2 (index 1), columns 0, 2, 4, 6
			pieces[2, i + 1] = new Man(1, Colour.Red); // row 3 (index 2), columns 1, 3, 5, 7

			pieces[5, i] = new Man(1, Colour.White); // row 6 (index 5), columns 0, 2, 4, 6
		    pieces[6, i + 1] = new Man(1, Colour.White); // row 7 (index 6), columns 1, 3, 5, 7
			pieces[7, i] = new Man(1, Colour.White); // row 8 (index 7), columns 0, 2, 4, 6
		}
	}

	static void PrintBoard(Board board)
	{
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				var piece = board.GetPiece(new Destination(i, j));
				if (piece == null)
				{
					Console.Write("- ");
				}
				else if (piece.Colour == Colour.White)
				{
					Console.Write(piece.Type == CharType.King ? "K " : "W ");
				}
				else
				{
					Console.Write(piece.Type == CharType.King ? "K " : "R ");
				}
			}
			Console.WriteLine();
		}
	}

	static Destination ParseDestination(string pos)
	{
		if (pos.Length == 2 && pos[0] >= 'a' && pos[0] <= 'h' && pos[1] >= '1' && pos[1] <= '8')
		{
			int x = pos[1] - '1';
			int y = pos[0] - 'a';
			return new Destination(x, y);
		}
		return null;
	}
}

}