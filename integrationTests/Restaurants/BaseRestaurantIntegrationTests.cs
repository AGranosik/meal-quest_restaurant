using domain.Restaurants.Aggregates;
using DotNet.Testcontainers.Containers;
using infrastructure.Database.MenuContext;
using infrastructure.Database.MenuContext.Models.Configurations;
using infrastructure.Database.RestaurantContext;
using infrastructure.Database.RestaurantContext.Models.Configurations;
using infrastructure.EventStorage;
using Microsoft.EntityFrameworkCore;
using Respawn;
using infrastructure.EventStorage.DatabaseModels.Configurations;
namespace integrationTests.Restaurants;

internal class BaseRestaurantIntegrationTests : BaseContainerIntegrationTests<RestaurantDbContext>
{
    protected MenuDbContext MenuDbContext;
    protected EventDbContext EventDbContext;

    public BaseRestaurantIntegrationTests(List<IContainer> containers) : base(containers)
    {
    }

    protected override async Task OneTimeSetUp()
    {
        await base.OneTimeSetUp();
        Connection = DbContext.Database.GetDbConnection();
        await Connection.OpenAsync();
        MenuDbContext = await GetDifferentDbContext<MenuDbContext>();
        EventDbContext = await GetDifferentDbContext<EventDbContext>();
        
        Respawner = await Respawner.CreateAsync(Connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude =
            [
                "public",
                RestaurantDatabaseConstants.SCHEMA,
                MenuDatabaseConstants.Schema,
                Constants.SCHEMA
            ]
        });

    }

    public override async Task OneTimeTearDown()
    {
        await base.OneTimeTearDown();
        await Connection!.DisposeAsync();
    }
}