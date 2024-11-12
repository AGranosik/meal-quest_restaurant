using Polly.Retry;
using Polly;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace core.FallbackPolicies
{
    public static class FallbackRetryPolicies
    {
        public const int NUMBER_OF_RETRIES = 2;
        public const string RETRY_TYPE = "RETRY_TYPE";

        public static IServiceCollection RegisterResiliencePipelines(this IServiceCollection services)
        {
            services.AddResiliencePipeline(RETRY_TYPE, builder =>
            {
                builder.AddRetry(new RetryStrategyOptions
                {
                    DelayGenerator = retryAttempt => new ValueTask<TimeSpan?>(TimeSpan.FromSeconds(Math.Pow(2, retryAttempt.AttemptNumber))),
                    MaxRetryAttempts = NUMBER_OF_RETRIES,
                    
                });
            });

            return services;
        }

        //public static AsyncRetryPolicy AsyncRetry
        //    => Policy.Handle<Exception>()
        //        .WaitAndRetryAsync(NUMBER_OF_RETRIES, retryAttempt =>
        //            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
        //        );
    }
}
