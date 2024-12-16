using System.Text.Json;

namespace infrastructure.Common
{
    internal static class Serializer
    {
        private static readonly JsonSerializerOptions _serializingOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static string Serialize<T>(T data)
            => JsonSerializer.Serialize<T>(data, _serializingOptions);

        public static T? Deserialize<T>(string data, Type type)
            => (T?)JsonSerializer.Deserialize(data, type, _serializingOptions);
    }
}
