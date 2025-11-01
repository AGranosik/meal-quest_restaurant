using application.Menus.Commands.Interfaces;
using application.Menus.Queries.Interfaces;
using infrastructure.Database.MenuContext.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure.Database.MenuContext;

internal static class MenuContextConfiguration
{
    public static IServiceCollection ConfigureMenuContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .ConfigureRepositories()
            .ConfigureDbContext(configuration);

        return serviceCollection;
    }

    private static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IMenuRepository, MenuRepository>()
            .AddScoped<IReadMenuRepository, ReadMenuRepository>();

        return services;
    }

    private static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        => services.AddDbContext<MenuDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("postgres")));
}