using application.EventHandlers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure.EventStorage
{
    public static class EventStorageConfiguration
    {
        public static IServiceCollection ConfigureEventStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IEventInfoStorage<,>), typeof(EventInfoStorage<,>));

            services
                .ConfigureDbContext(configuration);

            return services;
        }

        private static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
            => services.AddDbContext<EventDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("postgres")));
    }
}
