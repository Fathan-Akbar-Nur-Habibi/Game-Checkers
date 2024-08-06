using System;
using System.Collections.Generic;

namespace game_checkers
{
    public class GameController
    {
        public int Turn { get; private set; }
        private readonly IPlayer[] players;
        private readonly Board board;

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

            OnPieceMoved?.Invoke(piece, from, to);

            CheckGameEnd();

            return true;
        }

        public void ChangeTurn()
        {
            Turn = (Turn + 1) % 2;
            OnTurnChanged?.Invoke(Turn);
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
