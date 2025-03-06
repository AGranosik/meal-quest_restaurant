using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
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
}