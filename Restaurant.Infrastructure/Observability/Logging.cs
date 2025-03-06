using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System.Diagnostics;

namespace infrastructure.Observability;

internal static class Logging
{
    public static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureLogger(configuration);
        return services;
    }

    private static void ConfigureLogger(this IServiceCollection services, IConfiguration configuration)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        services.AddSerilog(l => l.Enrich.FromLogContext()
            .Enrich.With(new TraceEnricher())
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Error)
            .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment!))
            .Enrich.WithProperty("Environment", environment));
    }

    private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment)
    {
        return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]!))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"{InfrastuctureConfiguration.SERVICE_NAME}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
        };
    }
}

internal class TraceEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var traceId = Activity.Current?.TraceId.ToString() ?? "NoTraceId";
        var spanId = Activity.Current?.SpanId.ToString() ?? "NoSpanId";

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId", traceId));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SpanId", spanId));
    }
}