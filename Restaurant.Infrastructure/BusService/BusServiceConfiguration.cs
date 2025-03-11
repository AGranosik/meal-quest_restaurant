using application.EventHandlers.Interfaces;
using domain.Menus.Aggregates;
using domain.Restaurants.Aggregates;
using infrastructure.BusService.Emitters;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure.BusService;

//TODO: observability & metrics for grafana of rabbitmq
internal static class BusServiceConfiguration
{
    public const int TIMEOUT_LIMIT = 2;
    public static IServiceCollection ConfigureBusService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((bus, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("rabbitmq"));
            });
        });

        services.AddScoped<IEventEmitter<Menu>, MenuEventEmitter>();
        services.AddScoped<IEventEmitter<Restaurant>, RestaurantEventEmitter>();

        return services;
    }
}