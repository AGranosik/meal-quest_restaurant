using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using infrastructure.Database;
using infrastructure.EventStorage;
using infrastructure.Observability;

namespace infrastructure
{
    public static class InfrastuctureConfiguration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureDatabase(configuration)
                .ConfigureEventStorage(configuration)
                .ConfigureLogging(configuration);

            return services;
        }
    }
}
