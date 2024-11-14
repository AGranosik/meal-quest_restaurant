using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using infrastructure.Database;
using infrastructure.EventStorage;
using infrastructure.Observability;
using infrastructure.BusService;

namespace infrastructure
{
    public static class InfrastuctureConfiguration
    {
        public const string SERVICE_NAME = "restaurant-menu-service";
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureDatabase(configuration)
                .ConfigureEventStorage(configuration)
                .ConfigureBusService(configuration)
                .ConfigureLogging(configuration);

            return services;
        }
    }
}
