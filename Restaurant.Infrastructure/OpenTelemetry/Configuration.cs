using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Npgsql;
using Microsoft.Extensions.Configuration;


namespace infrastructure.OpenTelemetry
{
    public static class Configuration
    {
        public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var otlpEndpoint = configuration.GetSection("OTEL_EXPORTER_OTLP_ENDPOINT").Value;

            builder.Services
                .AddOpenTelemetry()
                .ConfigureResource(b => b.AddService(serviceName: "restaurant-menu-service"))
                .WithTracing(builder =>
                {
                    builder.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddNpgsql();

                    builder.AddOtlpExporter(o => o.Endpoint = new Uri(otlpEndpoint));
                })
                .WithMetrics(builder =>
                {
                    builder.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddPrometheusExporter();
                });

            builder.Logging.ClearProviders();
            builder.Logging.AddOpenTelemetry(opt =>
            {
                //opt.addc
            });

            return builder;
        }
    }
}
