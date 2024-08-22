namespace GameCheckers
{
	public class Player : IPlayer
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Colour Colour { get; set; }

		public Player(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}