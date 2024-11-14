using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure.BusService
{
    internal static class BusServiceConfiguration
    {
        public static IServiceCollection ConfigureBusService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("rabbitmq"));
                });
            });

            return services;
        }
    }
}
