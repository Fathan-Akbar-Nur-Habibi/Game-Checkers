namespace GameCheckers
{
	public interface IPlayer
	{
		int Id { get; set; }
		string Name { get; set; }
		 Colour Colour { get; set; }
	}
}
