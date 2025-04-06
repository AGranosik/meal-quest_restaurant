using infrastructure.Database.RestaurantContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace integrationTests;

internal sealed class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private IConfiguration? Configuration { get; set; }
    private IServiceCollection _services { get; set; }
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("integrationsettings.json")
                .Build();

            config.AddConfiguration(Configuration, false);
        });
        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            RemoveUnnecessaryServicesForTests(services);
            _services = services;
            services.AddDbContext<RestaurantDbContext>(opt => opt.UseNpgsql(Configuration!.GetConnectionString("postgres")));
        });

        base.ConfigureWebHost(builder);
    }

    private static void RemoveUnnecessaryServicesForTests(IServiceCollection services)
    {
        //trace
        var descriptor = services.FirstOrDefault(
            d => d.ServiceType == typeof(OpenTelemetry.Trace.TracerProvider));

        if (descriptor != null)
        {
            services.Remove(descriptor);
        }

        //logs
        services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
    }
}