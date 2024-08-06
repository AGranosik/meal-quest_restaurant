using System.Text.Json;

namespace unitTests
{
    internal static class ApiResponseDeserializator
    {
        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static T Deserialize<T>(string json)
            => JsonSerializer.Deserialize<T>(json, _options);
    }
}
