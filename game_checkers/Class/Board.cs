using System;
using System.Collections.Generic;

namespace game_checkers
{
    public class Board
    {
        protected readonly Piece[,] _pieces = new Piece[8, 8];

        public Piece[,] GetBoard() => _pieces;

        public Board(Piece[,] pieces)
        {
            if (pieces.GetLength(0) != 8 || pieces.GetLength(1) != 8)
                throw new ArgumentException("Board must be 8x8.", nameof(pieces));
            Array.Copy(pieces, _pieces, pieces.Length);
        }

        public bool IsOccupied(Destination destination)
        {
            return _pieces[destination.X, destination.Y] != null;
        }

        public Destination GetDestination(Piece piece)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (_pieces[x, y] == piece)
                    {
                        return new Destination(x, y);
                    }
                }
            }
            return null;
        }

        public Piece GetPiece(Destination destination)
        {
            return _pieces[destination.X, destination.Y];
        }

        public void PlacePiece(Piece piece, Destination destination)
        {
            _pieces[destination.X, destination.Y] = piece;
        }

        public IPlayer GetWinner()
        {
            bool whitePieces = false;
            bool redPieces = false;

            foreach (var piece in _pieces)
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
                return new Player(2, "Red");
            if (!redPieces)
                return new Player(1, "White");

            return null;
        }

        public bool HasPlayerPieces(IPlayer player)
        {
            foreach (var piece in _pieces)
            {
                if (piece != null && (player.Id == 1 && piece.Colour == Colour.White || player.Id == 2 && piece.Colour == Colour.Red))
                    return true;
            }
            return false;
        }
    }
}
