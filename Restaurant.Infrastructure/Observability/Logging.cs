using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.Elasticsearch;
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

            //services.AddSerilog


            //Log.Logger = new LoggerConfiguration()
            //    .Enrich.FromLogContext()
            //    //.Enrich.WithMachineName()
            //    .WriteTo.Console()
            //    .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
            //    //.WriteTo.
            //    .Enrich.WithProperty("Environment", environment)
            //    //.ReadFrom.Configuration(configuration)
            //    .CreateLogger();
        }

    }
}
