using System.Text.Json;

namespace integrationTests
{
    internal static class ApiResponseDeserializator
    {
        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static T? Deserialize<T>(this string json)
            => JsonSerializer.Deserialize<T>(json, _options);
    }
}
