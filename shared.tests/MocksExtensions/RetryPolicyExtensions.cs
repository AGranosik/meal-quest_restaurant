using core.FallbackPolicies;
using Moq;

namespace sharedTests.MocksExtensions;

public static class RetryPolicyExtensions
{
    public static Times NumberOfAppRetries()
        => Times.Exactly(FallbackRetryPolicies.NUMBER_OF_RETRIES + 1);
}