using application;
using infrastructure;
using infrastructure.Observability;
using webapi.Controllers.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.AddOpenTelemetry(builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UserErrorLogging();
app.UseHttpsRedirection();


app.UseOpenTelemetryPrometheusScrapingEndpoint();
//app.UseSerilogRequestLogging();
app.UseAuthorization();

app.MapControllers();

app.Run();
