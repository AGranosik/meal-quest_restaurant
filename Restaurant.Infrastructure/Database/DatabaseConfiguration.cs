using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using infrastructure.Database.RestaurantContext;
using infrastructure.Database.MenuContext;

namespace infrastructure.Database
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureRestaurantContext(configuration)
                .ConfigureMenuContext(configuration);

            return services;
        }


    }
}
