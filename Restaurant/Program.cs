using application;
using infrastructure;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using webapi.Controllers.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services
    .AddOpenTelemetry()
    .ConfigureResource(b => b.AddService(serviceName: "restaurant-menu-service"))
    .WithTracing(builder =>
    {
        builder.AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation();
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UserErrorLogging();
app.UseHttpsRedirection();


app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.UseAuthorization();

app.MapControllers();

app.Run();
