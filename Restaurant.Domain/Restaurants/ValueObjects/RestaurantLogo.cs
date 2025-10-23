using System.Diagnostics.CodeAnalysis;

namespace domain.Restaurants.ValueObjects;

[ExcludeFromCodeCoverage]
public sealed class RestaurantLogo
{
    public RestaurantLogo(byte[]? data)
    {
        Data = data;
    }
    public byte[]? Data { get; }
}