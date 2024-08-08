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
            var board = new ConcreteBoard(boardSetup);
            var gameController = new GameController(player1, player2, board);

            ChoosePlayerColours(gameController, player1, player2);

            gameController.OnPieceMoved += (piece, from, to) => Console.WriteLine($"Moved {piece.Colour} piece from {from.X + 1},{(char)(from.Y + 'a')} to {to.X + 1},{(char)(to.Y + 'a')}");
            gameController.OnTurnChanged += turn => Console.WriteLine($"Player {(turn + 1)}'s turn.");
            gameController.OnGameEnded += winner => Console.WriteLine($"Game over! Player {winner.Name} wins!");

            InitializeBoard(board, player1, player2, gameController);

            DisplayBoard(board);

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
                        DisplayBoard(board);
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

            Console.WriteLine($"{player1.Name} chose {player1Colour}. {player2.Name} will play as {player2Colour}.");
        }

        static Colour ParseColour(string input)
        {
            return input.Trim().ToLower() switch
            {
                "red" => Colour.Red,
                "white" => Colour.White,
                _ => throw new FormatException("Invalid color. Choose either 'Red' or 'White'.")
            };
        }

        static void InitializeBoard(Board board, IPlayer player1, IPlayer player2, GameController gameController)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if ((x + y) % 2 != 0)
                    {
                        board.PlacePiece(new Man(player2.Id, gameController.GetPlayerColour(player2), board), new Destination(x, y));
                    }
                }
                for (int x = 5; x < 8; x++)
                {
                    if ((x + y) % 2 != 0)
                    {
                        board.PlacePiece(new Man(player1.Id, gameController.GetPlayerColour(player1), board), new Destination(x, y));
                    }
                }
            }
        }

        static (Destination, Destination) ParseMove(string input)
        {
            var parts = input.Split(' ');
            if (parts.Length != 2) throw new FormatException("Invalid format.");

            var from = ParseCoordinate(parts[0]);
            var to = ParseCoordinate(parts[1]);

            return (from, to);
        }

        static Destination ParseCoordinate(string coordinate)
        {
            if (coordinate.Length != 3 || !char.IsDigit(coordinate[0]) || coordinate[1] != ',' || !char.IsLetter(coordinate[2]))
                throw new FormatException("Invalid coordinate format.");

            int x = int.Parse(coordinate[0].ToString()) - 1;
            int y = char.ToLower(coordinate[2]) - 'a';

            if (x < 0 || x >= 8 || y < 0 || y >= 8) throw new FormatException("Coordinate out of bounds.");

            return new Destination(x, y);
        }

        static void DisplayBoard(Board board)
        {
            Console.WriteLine("  a b c d e f g h");
            var pieces = board.GetBoard();
            for (int x = 0; x < 8; x++)
            {
                Console.Write($"{x + 1} ");
                for (int y = 0; y < 8; y++)
                {
                    var piece = pieces[x, y];
                    if (piece == null)
                        Console.Write("- ");
                    else
                        Console.Write($"{(piece.Colour == Colour.White ? 'W' : 'R')} ");
                }
                Console.WriteLine();
            }
        }
    }
}