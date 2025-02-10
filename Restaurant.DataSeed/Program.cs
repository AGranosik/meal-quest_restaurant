using application;
using infrastructure;
using infrastructure.Database;
using infrastructure.Database.RestaurantContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.DataSeed.Seed;

var servicesCollection = new ServiceCollection();
var builder = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddUserSecrets<Program>(); // Load user secrets

IConfiguration configuration = builder.Build();
servicesCollection.ConfigureDatabase(configuration)
    .AddApplication();
var serviceProvider = servicesCollection.BuildServiceProvider();

var mediator = serviceProvider.GetRequiredService<IMediator>();

var restaurantDbContext = serviceProvider.GetRequiredService<RestaurantDbContext>();
await restaurantDbContext.Database.MigrateAsync();

var seed = new DataSeed(mediator);

await seed.Seed();

Console.WriteLine("Hello, World!");
