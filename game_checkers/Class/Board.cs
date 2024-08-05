using System;
using System.Collections.Generic;
namespace game_checkers

{
    public class Board
    {
        protected Piece[,] _pieces = new Piece[8, 8];

        public Piece[,] GetBoard() => _pieces;

        public Board(Piece[,] pieces)
        {
            _pieces = pieces;
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

        public void SetPlacePiece(Piece piece, Destination destination)
        {
            _pieces[destination.X, destination.Y] = piece;
        }

        public IPlayer GetWinner()
        {
            // Check if either King is captured
            bool whiteKingAlive = false;
            bool redKingAlive = false;

            foreach (var piece in _pieces)
            {
                if (piece != null)
                {
                    if (piece.Type == CharType.King)
                    {
                        if (piece.Colour == Colour.White)
                        {
                            whiteKingAlive = true;
                        }
                        else if (piece.Colour == Colour.Red)
                        {
                            redKingAlive = true;
                        }
                    }
                }
            }

            if (!whiteKingAlive)
            {
                return new Player(2, "Red");
            }
            else if (!redKingAlive)
            {
                return new Player(1, "White");
            }

            return null;
        }

        public bool HasPlayer(IPlayer player)
        {
            // Check if the player's pieces are still on the board
            foreach (var piece in _pieces)
            {
                if (piece != null && (player.Id == 1 && piece.Colour == Colour.White || player.Id == 2 && piece.Colour == Colour.Red))
                {
                    return true;
                }
            }
            return false;
        }
    }

}