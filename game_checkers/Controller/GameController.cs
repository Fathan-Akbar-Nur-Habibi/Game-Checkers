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

        public bool MakeMove(IPlayer player, Piece piece, Destination from, Destination to)
        {
            if (board.IsOccupied(to) || !piece.AvailableMove(from).Contains(to))
            {
                return false;
            }

            board.PlacePiece(null, from);
            board.PlacePiece(piece, to);
            playerPieceLocations[player] = to; // Update player's piece location

            OnPieceMoved?.Invoke(piece, from, to);

            CheckGameEnd();

            return true;
        }

        public void ChangeTurn()
        {
            Turn = (Turn + 1) % 2;
            OnTurnChanged?.Invoke(Turn);
        }

        public void SetPlayerColour(IPlayer player, Colour colour)
        {
            playerColours[player] = colour;
        }

        public Colour GetPlayerColour(IPlayer player)
        {
            return playerColours[player];
        }

        public Destination GetPlayerPieceLocation(IPlayer player)
        {
            return playerPieceLocations.ContainsKey(player) ? playerPieceLocations[player] : null;
        }

        private void CheckGameEnd()
        {
            var winner = board.GetWinner();
            if (winner != null)
            {
                OnGameEnded?.Invoke(winner);
            }
        }
    }
}
