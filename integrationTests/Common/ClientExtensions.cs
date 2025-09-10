using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using FluentAssertions;

namespace integrationTests.Common;

internal static class ClientExtensions
{
    public static async Task<TResponse?> TestPostAsync<TRequest, TResponse>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, TRequest value, CancellationToken cancellationToken)
    {
        var response = await client.PostAsJsonAsync(requestUri, value, cancellationToken).ConfigureAwait(false);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var resultString = await response.Content.ReadAsStringAsync(cancellationToken);

        return ApiResponseDeserializator.Deserialize<TResponse>(resultString);
    }
    public static async Task<TResponse?> TestPostMultipartForm<TRequest, TResponse>(this HttpClient client, [StringSyntax("Uri")] string? requestUri, TRequest value, CancellationToken cancellationToken)
    {
        var formDataDict = value!.ToDictionary();

        using var content = new MultipartFormDataContent();

        foreach (var kv in formDataDict)
        {
            if (kv.Value is null) continue;

            if (kv.Value is byte[] fileBytes) // raw file
            {
                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(fileContent, kv.Key, "upload.bin");
            }
            else if (kv.Value is Stream stream) // file as stream
            {
                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(streamContent, kv.Key, "upload.bin");
            }
            else // normal properties (string, int, etc.)
            {
                content.Add(new StringContent(kv.Value.ToString() ?? ""), kv.Key);
            }
        }

        var response = await client.PostAsync(requestUri, content, cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var resultString = await response.Content.ReadAsStringAsync(cancellationToken);
        return ApiResponseDeserializator.Deserialize<TResponse>(resultString);
    }
    
    public static Dictionary<string, object?> ToDictionary(this object obj, string? prefix = null)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        var dict = new Dictionary<string, object?>();

        foreach (var prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var value = prop.GetValue(obj);
            if (value == null) continue;

            var key = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";

            if (IsSimple(value.GetType()) || value is byte[] || value is Stream)
            {
                dict[key] = value;
            }
            else if (value is IEnumerable<object> list) // Handle collections
            {
                int index = 0;
                foreach (var item in list)
                {
                    if (IsSimple(item.GetType()))
                        dict[$"{key}[{index}]"] = item;
                    else
                        foreach (var kv in item.ToDictionary($"{key}[{index}]"))
                            dict[kv.Key] = kv.Value;
                    index++;
                }
            }
            else // Nested object
            {
                foreach (var kv in value.ToDictionary(key))
                    dict[kv.Key] = kv.Value;
            }
        }

        return dict;
    }

    private static bool IsSimple(Type type)
    {
        return type.IsPrimitive
               || type.IsEnum
               || type == typeof(string)
               || type == typeof(decimal)
               || type == typeof(DateTime)
               || type == typeof(Guid);
    }
}