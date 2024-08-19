using System;

namespace GameCheckers
{
	class Program
	{
		static void Main()
		{
			Console.Write("Player 1 ID: ");
			int player1Id = int.Parse(Console.ReadLine());
			Console.Write("Player 1 Name: ");
			string player1Name = Console.ReadLine();
			IPlayer player1 = new Player(player1Id, player1Name);

			Console.Write("Player 2 ID: ");
			int player2Id = int.Parse(Console.ReadLine());
			Console.Write("Player 2 Name: ");
			string player2Name = Console.ReadLine();
			IPlayer player2 = new Player(player2Id, player2Name);

			var boardSetup = new Piece[8, 8];
			var board = new Board(boardSetup);
			var gameController = new GameController(player1, player2, board);

			ChoosePlayerColours(gameController, player1, player2);

			gameController.OnPieceMoved += (piece, from, to) => Console.WriteLine($"Moved {piece.Colour} piece from {from.X + 1},{(char)(from.Y + 'a')} to {to.X + 1},{(char)(to.Y + 'a')}");
			gameController.OnTurnChanged += turn => Console.WriteLine($"Player {(turn + 1)}'s turn.");
			gameController.OnGameEnded += winner => Console.WriteLine($"Game over! Player {winner.Name} wins!");

			InitializeBoard(board, player1, player2, gameController);

			DisplayBoard(gameController);

			while (true)
			{
				Console.Write("Enter move (format: x1,y1 x2,y2) or type 'exit' to quit: ");
				string input = Console.ReadLine();

				if (input.Trim().ToLower() == "exit")
				{
					Console.WriteLine("Game ended by player.");
					break;
				}

				try
				{
					var move = ParseMove(input);
					var piece = board.GetPiece(move.Item1);
					if (piece != null && gameController.MakeMove(gameController.GetPlayers()[gameController.Turn], piece, move.Item1, move.Item2))
					{
						DisplayBoard(gameController);
						gameController.ChangeTurn();
					}
					else
					{
						Console.WriteLine("Invalid move. Try again.");
					}
				}
				catch (FormatException ex)
				{
					Console.WriteLine($"Error: {ex.Message}. Please use the format x1,y1 x2,y2.");
				}
			}
		}

		static void ChoosePlayerColours(GameController gameController, IPlayer player1, IPlayer player2)
		{
			Console.Write("Player 1, choose your color (Red/White): ");
			Colour player1Colour = ParseColour(Console.ReadLine());
			Colour player2Colour = player1Colour == Colour.Red ? Colour.White : Colour.Red;

			gameController.SetPlayerColour(player1, player1Colour);
			gameController.SetPlayerColour(player2, player2Colour);

			Console.WriteLine($"{player1.Name} chose {player1Colour}. {player2.Name} will be {player2Colour}.");
		}

		static Colour ParseColour(string input)
		{
			input = input.Trim().ToLower();
			return input switch
			{
				"red" => Colour.Red,
				"white" => Colour.White,
			     _ => throw new FormatException("Invalid colour. Please enter 'Red' or 'White'.")
					// use "_" discard pattern
			};
		}

		static void InitializeBoard(Board board, IPlayer player1, IPlayer player2, GameController gameController)
		{
			// Initialize Red pieces
			for (int x = 0; x < 3; x++)
			{
				for (int y = 0; y < 8; y++)
				{
					if ((x + y) % 2 == 1)
					{
						var piece = new Man(2, Colour.Red, board);
						board.PlacePiece(piece, new Destination(x, y));
					}
				}
			}

			// Initialize White pieces
			for (int x = 5; x < 8; x++)
			{
				for (int y = 0; y < 8; y++)
				{
					if ((x + y) % 2 == 1)
					{
						var piece = new Man(1, Colour.White, board);
						board.PlacePiece(piece, new Destination(x, y));
					}
				}
			}
		}

		static void DisplayBoard(GameController gameController)
		{
			var board = gameController.GetBoard();

			for (int x = 0; x < 8; x++)
			{
				for (int y = 0; y < 8; y++)
				{
					var piece = board[x, y];
					if (piece == null)
					{
						Console.Write("- ");
					}
					else
					{
						Console.Write($"{(piece.Colour == Colour.Red ? 'R' : 'W')} ");
					}
				}
				Console.WriteLine();
			}
			Console.WriteLine();
		}

		static Tuple<Destination, Destination> ParseMove(string input)
		{
			var parts = input.Split(' ');
			if (parts.Length != 2)
			{
				throw new FormatException("Invalid input. Please provide two coordinates.");
			}

			var from = ParseCoordinate(parts[0]);
			var to = ParseCoordinate(parts[1]);

			return Tuple.Create(from, to);
		}

		static Destination ParseCoordinate(string input)
		{
			if (input.Length != 2)
			{
				throw new FormatException("Invalid coordinate format. Use 'x,y'.");
			}

			int x = int.Parse(input[0].ToString()) - 1;
			int y = input[1] - 'a';

			if (x < 0 || x > 7 || y < 0 || y > 7)
			{
				throw new FormatException("Coordinate out of bounds.");
			}

			return new Destination(x, y);
		}
	}
}
