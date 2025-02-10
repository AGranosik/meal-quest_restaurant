using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using infrastructure.Database.RestaurantContext;
using infrastructure.Database.MenuContext;

namespace infrastructure.Database
{
    internal static class DatabaseConfiguration
    {
        internal static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureRestaurantContext(configuration)
                .ConfigureMenuContext(configuration);

            return services;
        }


    }
}
