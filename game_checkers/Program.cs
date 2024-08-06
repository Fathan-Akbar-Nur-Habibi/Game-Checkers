using System;

namespace game_checkers
{
    class Program
    {
        static void Main()
        {
            IPlayer player1 = new Player(1, "White");
            IPlayer player2 = new Player(2, "Red");
            Piece[,] pieces = new Piece[8, 8];

            InitializeBoard(pieces);

            Board board = new ConcreteBoard(pieces);
            GameController gameController = new GameController(player1, player2, board);

            gameController.OnTurnChanged += turn => Console.WriteLine($"{(turn == 0 ? "White" : "Red")}'s turn.");
            gameController.OnPieceMoved += (piece, from, to) => Console.WriteLine($"Piece moved from {PositionToString(from)} to {PositionToString(to)}");
            gameController.OnPieceRemoved += (piece, destination) => Console.WriteLine($"Piece removed from {PositionToString(destination)}");
            gameController.OnGameEnded += player => Console.WriteLine($"Game ended! Winner: {player.Name}");

            Console.WriteLine("Checkers Game");
            PrintBoard(board);

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
                                if (board.GetWinner() != null)
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

        static string PositionToString(Destination destination)
        {
            return $"{(char)(destination.Y + 'a')}{destination.X + 1}";
        }
    }
}
