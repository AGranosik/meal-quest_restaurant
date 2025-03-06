using application.Restaurants.Commands.Interfaces;
using application.Restaurants.Queries.Interfaces;
using infrastructure.Database.RestaurantContext.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure.Database.RestaurantContext;

internal static class RestaurantContextConfiguration
{
    public static IServiceCollection ConfigureRestaurantContext(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureRepositories()
            .ConfigureDbContext(configuration);

        return services;
    }

    private static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IRestaurantRepository, RestaurantReposiotry>()
            .AddScoped<IRestaurantQueryRepository, RestaurantQueryRepository>();


        return services;
    }

    private static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        => services.AddDbContext<RestaurantDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("postgres")));
}