using domain.Common.BaseTypes;

namespace domain.Restaurants.ValueObjects
{
    public class Coordinates(double x, double y) : ValueObject<Coordinates>
    {
        public double X { get; } = x;
        public double Y { get; } = y;

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            if (obj is not Coordinates other) return false;
            return X == other.X && Y == other.Y;
        }
    }
}
