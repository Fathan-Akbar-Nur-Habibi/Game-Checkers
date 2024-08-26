using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace GameCheckers
{
	class Program
	{
		static void Main()
		{
			IPlayer player1 = CreatePlayer(1);
			IPlayer player2;
			
			while (true)
			{
				player2 = CreatePlayer(2);
				if (player1.Id == player2.Id)
				{
					Console.WriteLine("This ID is already in use by another player. Please enter a different ID.");
				}
				else 
				{
					break;
				}
			}
			
			var boardSetup = new Piece[8, 8];
			var board = new Board(boardSetup);
			var gameController = new GameController(player1, player2, board);

			// Set player colors directly on player objects
			ChoosePlayerColours(player1, player2);

			gameController.OnPieceMoved += (piece, from, to) => Console.WriteLine($"Moved {piece.Colour} piece from {from.X + 1},{(char)(from.Y + 'a')} to {to.X + 1},{(char)(to.Y + 'a')}");
			gameController.OnTurnChanged += turn => Console.WriteLine($"Player, {(turn )}");
			gameController.OnGameEnded += winner => Console.WriteLine($"Game over! Player {winner.Name} wins!");

			InitializeBoard(board, player1, player2);

			DisplayBoard(gameController);

			while (true)
			{
				Console.Write("Enter move (format: x1y1 x2y2) or type 'exit' to quit: ");
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
					Console.WriteLine($"Error: {ex.Message}. Please use the format x1y1 x2y2.");
				}
			}
		}
		
		// Method to save the game state
public static void SaveGameState(GameController gameController)
{
	var options = new JsonSerializerOptions
	{
		WriteIndented = true,
		IncludeFields = true // Enable this to include non-public fields in serialization
	};

	string jsonString = JsonSerializer.Serialize(gameController, options);
	
	// Save the JSON string to a file
	File.WriteAllText("game_state.json", jsonString);
	
	Logger.Instance.Log("Game state saved.");
}

// Method to load the game state
public static GameController LoadGameState()
{
	if (!File.Exists("game_state.json"))
	{
		throw new FileNotFoundException("Saved game state file not found.");
	}

	string jsonString = File.ReadAllText("game_state.json");
	
	var options = new JsonSerializerOptions
	{
		IncludeFields = true // Enable this to include non-public fields in serialization
	};

	return JsonSerializer.Deserialize<GameController>(jsonString, options);
}


		static IPlayer CreatePlayer(int playerNumber)
		{
			Console.Write($"Player {playerNumber} ID: ");
			int playerId = int.Parse(Console.ReadLine());
			Console.Write($"Player {playerNumber} Name: ");
			string playerName = Console.ReadLine();
			return new Player(playerId, playerName);
		}

		static void ChoosePlayerColours(IPlayer player1, IPlayer player2)
		{
			Console.Write("Player 1, choose your color (Red/White): ");
			Colour player1Colour = ParseColour(Console.ReadLine());
			Colour player2Colour = player1Colour == Colour.Red ? Colour.White : Colour.Red;

			player1.Colour = player1Colour;
			player2.Colour = player2Colour;

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
			};
		}

		static void InitializeBoard(Board board, IPlayer player1, IPlayer player2)
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
			Console.WriteLine("  a b c d e f g h");
			var board = gameController.GetBoard();
	
			for (int x = 0; x < 8; x++)
			{
				Console.Write($"{x + 1} ");
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