using application;
using infrastructure;
using infrastructure.Database.MenuContext;
using infrastructure.Database.RestaurantContext;
using infrastructure.EventStorage;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Restaurant.DataSeed.Seed;
using webapi.Controllers.Restaurants;

var servicesCollection = new ServiceCollection();
var builder = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddUserSecrets<Program>(); // Load user secrets

IConfiguration configuration = builder.Build();
servicesCollection.AddInfrastructure(configuration)
    .AddApplication();

servicesCollection.AddScoped(typeof(ILogger<>), typeof(NullLogger<>));


var serviceProvider = servicesCollection.BuildServiceProvider();

var mediator = serviceProvider.GetRequiredService<IMediator>();
var restaurantDbContext = serviceProvider.GetRequiredService<RestaurantDbContext>();
var eventDbContext = serviceProvider.GetRequiredService<EventDbContext>();
var menuDbContext = serviceProvider.GetRequiredService<MenuDbContext>();

await restaurantDbContext.Database.MigrateAsync();
await eventDbContext.Database.MigrateAsync();
await menuDbContext.Database.MigrateAsync();

var seed = new DataSeed(mediator, NullLoggerFactory.Instance.CreateLogger<RestaurantController>());

await seed.SeedRestaurants();
