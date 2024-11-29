using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure.BusService
{
    //TODO: publish endpoint
    //TODO: Tests
    //TODO: observability & metrics for grafana

    // interface in application layer
    // configuration here
    // retry on application side
    // do not block thread on events publication
    internal static class BusServiceConfiguration
    {
        public static IServiceCollection ConfigureBusService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((_, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("rabbitmq"));
                    cfg.UseTimeout(cfg => cfg.Timeout =  TimeSpan.FromMilliseconds(100));
                });
            });

            return services;
        }
    }
}
