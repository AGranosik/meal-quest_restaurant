using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System.Diagnostics;
using System.Reflection;

namespace infrastructure.Observability
{
    public static class Logging
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
                //.Enrich.WithMachineName()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Error)
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment!))
                //.WriteTo.
                .Enrich.WithProperty("Environment", environment));
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
        }
    }

    public class TraceEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var traceId = Activity.Current?.TraceId.ToString() ?? "NoTraceId";
            var spanId = Activity.Current?.SpanId.ToString() ?? "NoSpanId";

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId", traceId));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SpanId", spanId));
        }
    }
}
