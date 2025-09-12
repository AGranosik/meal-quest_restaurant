namespace Restaurant.DataSeed.Seed;

public static class GeoUtils
{
    private const double EarthRadiusMeters = 6371000; // average Earth radius

    public static (double lat, double lng) GetRandomNearby(Random rng, double baseLat, double baseLng, double maxMeters = 200)
    {
        // Convert meters to degrees
        var lat = baseLat;
        var lng = baseLng;

        // random distance and angle
        var distance = rng.NextDouble() * maxMeters;
        var angle = rng.NextDouble() * 2 * Math.PI;

        // offset in radians
        var dLat = (distance * Math.Cos(angle)) / EarthRadiusMeters;
        var dLng = (distance * Math.Sin(angle)) / (EarthRadiusMeters * Math.Cos(baseLat * Math.PI / 180));

        // apply offsets (convert back to degrees)
        var newLat = lat + dLat * (180 / Math.PI);
        var newLng = lng + dLng * (180 / Math.PI);

        return (newLat, newLng);
    }
}