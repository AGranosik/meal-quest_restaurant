using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(ApplicationConfiguration)));
        return services;
    }
}