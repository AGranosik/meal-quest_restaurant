using System.Text.Json;

namespace integrationTests;

internal static class ApiResponseDeserializator
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static T? Deserialize<T>(this string json)
        => JsonSerializer.Deserialize<T>(json, _options);
}