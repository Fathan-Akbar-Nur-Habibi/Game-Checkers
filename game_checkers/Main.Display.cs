using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography.X509Certificates;

namespace game_checkers;

class Program 
{
	static void Main ()
	{
		var player1 = new Player {Id = 1, Name = "Player1" };
		var player2 = new Player {Id = 2, Name = "Player2"}; 
		
		Piece[,] pieces = new Piece[8,8];
		InitializePieces(pieces, player1, player2);
		
		var board = new ConcreteBoard(pieces);
		var gameController = new GameController(player1, player2, board);
		var display = new Display(gameController, board);
		
		display.StartGame();
		
		static void InitializePieces(Piece[,]pieces,IPlayer player1,IPlayer player2)
		{
			for (int i=0; i<8;i++)
			{
				for(int j =0; j<8;j++)
				{
					if ((i+j) %2 ==1) 
					{
						if (i < 3)
						{
							pieces[i,j] = new CharPiece(i*8 + j, CharType.Man, Colour.White, 1, false);
					
						}
						else if (i>4)
						{
							pieces[i,j] = new CharPiece(i*8 +j, CharType.King, Colour.Red,1, false);
						}
					}
				}
			}
		}
	}
	public class Player : IPlayer { 
		public int Id{ get;}
		public string Name {get; set;}
	}
	public class ConcreteBoard :Board
	{ 
		public ConcreteBoard (Piece[,] pieces) : base (pieces) { }
		public override IPlayer GetWinner()
		{
			return null;
		}
        public override bool HasPlayer(IPlayer player)
        {
            return false;
        }
    }
}