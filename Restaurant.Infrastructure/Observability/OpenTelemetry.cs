using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Npgsql;
using Microsoft.Extensions.Configuration;
using OpenTelemetry.Logs;


namespace infrastructure.Observability
{
    public static class OpenTelemetry
    {
        public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var otlpEndpoint = configuration.GetSection("OTEL_EXPORTER_OTLP_ENDPOINT").Value;

            builder.Services
                .AddOpenTelemetry()
                .ConfigureResource(b => b.AddService(serviceName: InfrastuctureConfiguration.SERVICE_NAME))
                .WithTracing(builder =>
                {
                    builder.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddNpgsql();

                    builder.AddOtlpExporter(o => o.Endpoint = new Uri(otlpEndpoint!));
                })
                .WithMetrics(builder =>
                {
                    builder.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddPrometheusExporter();
                });

            builder.Logging.AddOpenTelemetry(otlp => {
                otlp.IncludeFormattedMessage = true;
                otlp.IncludeScopes = true;
                otlp.ParseStateValues = true;

            });



            return builder;
        }

    }
}
