namespace game_checkers
{
    public class Destination
    {
        public int X { get; }
        public int Y { get; }

        public Destination(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is Destination destination &&
                   X == destination.X &&
                   Y == destination.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}