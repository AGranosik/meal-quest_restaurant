using domain.Common.DomainImplementationTypes;

namespace domain.Restaurants.ValueObjects;

public class Coordinates : ValueObject<Coordinates>
{
    public Coordinates(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double X { get; }
    public double Y { get; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;

        if (obj is not Coordinates other) return false;
        return X == other.X && Y == other.Y;
    }
}