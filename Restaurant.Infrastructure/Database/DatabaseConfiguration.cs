using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using infrastructure.Database.RestaurantContext;

namespace infrastructure.Database
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureRestaurantContext(configuration);
            return services;
        }


    }
}
