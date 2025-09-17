namespace Restaurant.DataSeed.Seed;

public static class GeoUtils
{
    private const double EarthRadiusMeters = 6371000; // average Earth radius

    public static (double lat, double lng) GetRandomNearby(Random rng, double baseLat, double baseLng, double maxMeters = 200)
    {
        // convert from degrees to radians
        var latRad = baseLat * Math.PI / 180.0;
        var lngRad = baseLng * Math.PI / 180.0;

        // pick a random distance (0..maxMeters) and bearing (0..360°)
        var distance = rng.NextDouble() * maxMeters;
        var bearing = rng.NextDouble() * 2 * Math.PI;

        // angular distance
        var angularDistance = distance / EarthRadiusMeters;

        // new latitude
        var newLatRad = Math.Asin(
            Math.Sin(latRad) * Math.Cos(angularDistance) +
            Math.Cos(latRad) * Math.Sin(angularDistance) * Math.Cos(bearing)
        );

        // new longitude
        var newLngRad = lngRad + Math.Atan2(
            Math.Sin(bearing) * Math.Sin(angularDistance) * Math.Cos(latRad),
            Math.Cos(angularDistance) - Math.Sin(latRad) * Math.Sin(newLatRad)
        );

        // convert back to degrees
        var newLat = newLatRad * 180.0 / Math.PI;
        var newLng = newLngRad * 180.0 / Math.PI;

        return (newLat, newLng);
    }
}