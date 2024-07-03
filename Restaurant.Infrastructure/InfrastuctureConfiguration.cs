using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Infrastructure.Database;

namespace Restaurants.Infrastructure
{
    public static class InfrastuctureConfiguration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureDatabase(configuration);
            return services;
        }
    }
}
