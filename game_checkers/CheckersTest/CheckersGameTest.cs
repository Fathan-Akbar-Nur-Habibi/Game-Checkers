using System;
using NUnit.Framework;

namespace GameCheckers.Tests
{
    [TestFixture]
    public class GameControllerTests
    {
        private GameController _gameController;
        private Player _player1;
        private Player _player2;
        private Board _board;

        [SetUp]
        public void SetUp()
        {
            _player1 = new Player(1, "Alice");
            _player2 = new Player(2, "Bob");
            _board = InitializeBoard();
            _gameController = new GameController(_player1, _player2, _board);
        }

        private Board InitializeBoard()
        {
            var pieces = new Piece[8, 8];

            // Initialize Red pieces
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if ((x + y) % 2 != 0)
                    {
                        pieces[x, y] = new Man(x * 8 + y, Colour.Red, null);
                    }
                }
            }

            // Initialize White pieces
            for (int x = 5; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if ((x + y) % 2 != 0)
                    {
                        pieces[x, y] = new Man(x * 8 + y, Colour.White, null);
                    }
                }
            }

            return new Board(pieces);
        }

        [Test]
        public void MakeMove_ValidMove_UpdatesBoardAndChangesTurn()
        {
            // Arrange
            var from = new Destination(2, 1);
            var to = new Destination(3, 2);
            var piece = _board.GetPiece(from);

            // Act
            var result = _gameController.MakeMove(_player1, piece, from, to);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(_board.GetPiece(from), Is.Null);
            Assert.That(_board.GetPiece(to), Is.EqualTo(piece));
            Assert.That(_gameController.Turn, Is.EqualTo(1));
        }

        [Test]
        public void MakeMove_InvalidMove_DoesNotUpdateBoardOrChangeTurn()
        {
            // Arrange
            var from = new Destination(2, 1);
            var to = new Destination(5, 2); // Invalid move
            var piece = _board.GetPiece(from);

            // Act
            var result = _gameController.MakeMove(_player1, piece, from, to);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(_board.GetPiece(from), Is.EqualTo(piece));
            Assert.That(_board.GetPiece(to), Is.Null);
            Assert.That(_gameController.Turn, Is.EqualTo(0));
        }

        [Test]
        public void MakeMove_CaptureMove_RemovesCapturedPieceAndUpdatesBoard()
        {
            // Arrange
            var from = new Destination(2, 1);
            var mid = new Destination(3, 2);
            var to = new Destination(4, 3);

            // Place opponent's piece in the middle
            var opponentPiece = new Man(99, Colour.Red, null);
            _board.PlacePiece(opponentPiece, mid);
            var piece = _board.GetPiece(from);

            // Act
            var result = _gameController.MakeMove(_player1, piece, from, to);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(_board.GetPiece(from), Is.Null);
            Assert.That(_board.GetPiece(to), Is.EqualTo(piece));
            Assert.That(_board.GetPiece(mid), Is.Null); // Captured piece is removed
            Assert.That(_gameController.Turn, Is.EqualTo(1));
        }

        [Test]
        public void GetWinner_ReturnsPlayer_WhenOpponentHasNoPieces()
        {
            // Arrange
            // Remove all of Player 2's pieces from the board
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var piece = _board.GetPiece(new Destination(x, y));
                    if (piece != null && piece.Colour == Colour.Red)
                    {
                        _board.RemovePiece(new Destination(x, y));
                    }
                }
            }

            // Act
            var winner = _gameController.GetWinner();

            // Assert
            Assert.That(winner, Is.EqualTo(_player1));
        }

        [Test]
        public void GetWinner_ReturnsNull_WhenBothPlayersHavePieces()
        {
            // Act
            var winner = _gameController.GetWinner();

            // Assert
            Assert.That(winner, Is.Null);
        }

        [Test]
        public void PromotionToKing_WhenPieceReachesEndOfBoard()
        {
            // Arrange
            var from = new Destination(1, 0);
            var to = new Destination(0, 1); // Destination for promotion
            var piece = _board.GetPiece(from);

            // Act
            var result = _gameController.MakeMove(_player1, piece, from, to);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(_board.GetPiece(to), Is.InstanceOf<King>());
        }

        [Test]
        public void OnGameEnded_EventTriggered_WhenWinnerExists()
        {
            // Arrange
            bool eventTriggered = false;
            _gameController.OnGameEnded += (winner) => eventTriggered = true;

            // Remove all of Player 2's pieces
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var piece = _board.GetPiece(new Destination(x, y));
                    if (piece != null && piece.Colour == Colour.Red)
                    {
                        _board.RemovePiece(new Destination(x, y));
                    }
                }
            }

            // Act
            _gameController.CheckGameEnd();

            // Assert
            Assert.That(eventTriggered, Is.True);
        }
    }
}
