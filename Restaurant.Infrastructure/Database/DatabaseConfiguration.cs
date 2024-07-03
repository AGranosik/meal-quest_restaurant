using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using infrastructure.Database.RestaurantContext;

namespace infrastructure.Database
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.SetupDbConnection(configuration);
            return services;
        }


        private static IServiceCollection SetupDbConnection(this IServiceCollection services, IConfiguration configuration)
             => services.AddDbContext<RestaurantDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("postgres")));

    }
}
