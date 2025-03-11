using Polly;
using Polly.Retry;

namespace core.Operations.FallbackPolicies;

public static class FallbackRetryPolicies
{
    public const int NUMBER_OF_RETRIES = 2;
    public static AsyncRetryPolicy AsyncRetry
        => Policy.Handle<Exception>()
            .WaitAndRetryAsync(NUMBER_OF_RETRIES, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            );
}