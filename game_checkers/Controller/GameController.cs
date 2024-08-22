using System;
using System.Collections.Generic;

namespace GameCheckers
{
	public class GameController
	{
		public int Turn { get; private set; }
		private readonly IPlayer[] players;
		private readonly Board board;
		private readonly Dictionary<IPlayer, Colour> playerColours = new Dictionary<IPlayer, Colour>();
		private readonly Dictionary<IPlayer, Destination> playerPieceLocations = new Dictionary<IPlayer, Destination>();

		public event Action<int> OnTurnChanged;
		public event Action<Piece, Destination, Destination> OnPieceMoved;
		public event Action<Piece, Destination> OnPieceRemoved;
		public event Action<IPlayer> OnGameEnded;

		public GameController(IPlayer player1, IPlayer player2, Board board)
		{
			players = new IPlayer[] { player1, player2 };
			this.board = board;
			Turn = 0;
		}

		public List<IPlayer> GetPlayers() => new List<IPlayer>(players);

		public Piece[,] GetBoard() => board.Pieces;

		public IPlayer GetWinner()
		{
			bool whitePieces = false;
			bool redPieces = false;

			foreach (var piece in GetBoard())
			{
				if (piece != null)
				{
					if (piece.Colour == Colour.White)
						whitePieces = true;
					else if (piece.Colour == Colour.Red)
						redPieces = true;
				}
			}

			if (!whitePieces)
				return players[1];
			if (!redPieces)
				return players[0];

			return null;
		}

		public bool MakeMove(IPlayer player, Piece piece, Destination from, Destination to)
		{
			if (!piece.AvailableMove(from).Contains(to))
			{
				return false;
			}

			// Handle piece capture
			int dx = to.X - from.X;
			int dy = to.Y - from.Y;
			if (Math.Abs(dx) == 2 && Math.Abs(dy) == 2)
			{
				Destination mid = new Destination(from.X + dx / 2, from.Y + dy / 2);
				Piece capturedPiece = board.GetPiece(mid);
				if (capturedPiece != null && capturedPiece.Colour != piece.Colour)
				{
					board.PlacePiece(null, mid);
					OnPieceRemoved?.Invoke(capturedPiece, mid);
				}
			}

			// Promote to King if reaching the opposite end
			if (piece is Man && (to.X == 0 || to.X == 7))
			{
				var king = new King(piece.Id, piece.Colour, board);
				board.PlacePiece(king, to);
			}
			else
			{
				board.PlacePiece(piece, to);
			}

			board.PlacePiece(null, from);
			playerPieceLocations[player] = to;

			OnPieceMoved?.Invoke(piece, from, to);

			// Check for additional jumps
			List<Destination> furtherMoves = piece.AvailableMove(to);
			if (furtherMoves.Count > 0)
			{
				Console.WriteLine($"{player.Name}, you can make another jump. Enter your next move: ");
				string input = Console.ReadLine();
				if (!string.IsNullOrEmpty(input))
				{
					string[] move = input.Split(' ');
					if (move.Length == 2)
					{
						Destination nextTo = new Destination(ConvertCoordinate(move[1][0]), int.Parse(move[1][1].ToString()) - 1);
						if (MakeMove(player, piece, to, nextTo))
						{
							ChangeTurn();
						}
					}
				}
			}

			CheckGameEnd();

			return true;
		}

		private int ConvertCoordinate(char coordinate)
		{
			return coordinate - 'A';
		}

		public void ChangeTurn()
		{
			Turn = (Turn + 1) % 2;
			OnTurnChanged?.Invoke(Turn);
		}

		private void CheckGameEnd()
		{
			IPlayer winner = GetWinner();
			if (winner != null)
			{
				OnGameEnded?.Invoke(winner);
			}
			else
			{
				ChangeTurn();
			}
		}
	}
}
