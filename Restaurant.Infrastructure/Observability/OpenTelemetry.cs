using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Npgsql;
using Microsoft.Extensions.Configuration;
using OpenTelemetry.Logs;
using OpenTelemetry.Exporter;
using Serilog;
using Microsoft.Extensions.Options;
using System;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;


namespace infrastructure.Observability
{
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


    public static class OpenTelemetry
    {
        //refactor
        // check if console exporter is needed
        // add db queries to traces
        // how 
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


            //builder.Host.UseSerilog();
            //builder.Logging.ClearProviders();
            builder.Logging.AddOpenTelemetry(otlp => {
                otlp.AddConsoleExporter();
                otlp.IncludeFormattedMessage = true;
                otlp.IncludeScopes = true;
                otlp.ParseStateValues = true;

            });

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //var logger = new LoggerConfiguration()
            //    .Enrich.FromLogContext()
            //    .Enrich.WithProperty("TraceId", () => Activity.Current?.TraceId.ToString())
            //    .Enrich.WithProperty("SpanId", () => Activity.Current?.SpanId.ToString())
            //    //.Enrich.WithMachineName()
            //    .WriteTo.Console()
            //    .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment!))
            //            //.WriteTo.
            //    .Enrich.WithProperty("Environment", environment)
            //    //.ReadFrom.Configuration(configuration)
            //    .CreateLogger();

            builder.Services.AddSerilog(l => l.Enrich.FromLogContext()
            .Enrich.With(new TraceEnricher())
                //.Enrich.WithMachineName()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment!))
                //.WriteTo.
                .Enrich.WithProperty("Environment", environment));

            return builder;
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
}
