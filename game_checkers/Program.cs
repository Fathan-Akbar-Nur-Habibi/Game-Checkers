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

            gameController.OnTurnChanged += (turn) => Console.WriteLine($"{(turn == 0 ? "White" : "Red")}'s turn. Enter move (e.g., '2 3 3 4' to move piece from (2, 3) to (3, 4)) or type 'exit' to quit: ");
            gameController.OnPieceMoved += (piece, from, to) => Console.WriteLine($"Piece moved from {from.X},{from.Y} to {to.X},{to.Y}");
            gameController.OnPieceRemoved += (piece, destination) => Console.WriteLine($"Piece removed from {destination.X},{destination.Y}");
            gameController.OnGameEnded += (player) => Console.WriteLine($"Game ended! Winner: {player.Name}");

            Console.WriteLine("Checkers Game");
            PrintBoard(board);

            // Game loop
            bool gameRunning = true;
            while (gameRunning)
            {
                Console.Write($"{(gameController.Turn == 0 ? "White" : "Red")}'s turn. Enter move (e.g., '2 3 3 4'): ");
                string input = Console.ReadLine();
                if (input.ToLower() == "exit")
                {
                    gameRunning = false;
                    break;
                }

                var moveParts = input.Split(' ');

                if (moveParts.Length == 4 && int.TryParse(moveParts[0], out int fromX) && int.TryParse(moveParts[1], out int fromY) && int.TryParse(moveParts[2], out int toX) && int.TryParse(moveParts[3], out int toY))
                {
                    var from = new Destination(fromX, fromY);
                    var to = new Destination(toX, toY);

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
                    Console.WriteLine("Invalid input. Use format '2 3 3 4'.");
                }
            }
        }

        static void InitializeBoard(Piece[,] pieces)
        {
            for (int i = 0; i < 8; i += 2)
            {
                pieces[0, i + 1] = new Man(1, Colour.Red);
                pieces[1, i] = new Man(1, Colour.Red);
                pieces[2, i + 1] = new Man(1, Colour.Red);

                pieces[5, i] = new Man(1, Colour.White);
                pieces[6, i + 1] = new Man(1, Colour.White);
                pieces[7, i] = new Man(1, Colour.White);
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
                        Console.Write("W ");
                    }
                    else
                    {
                        Console.Write("R ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}