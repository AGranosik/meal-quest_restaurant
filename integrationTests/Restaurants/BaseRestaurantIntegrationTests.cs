using DotNet.Testcontainers.Containers;
using infrastructure.Database.MenuContext;
using infrastructure.Database.MenuContext.Models.Configurations;
using infrastructure.Database.RestaurantContext;
using infrastructure.Database.RestaurantContext.Models.Configurations;
using infrastructure.EventStorage;
using infrastructure.EventStorage.DatabaseModels.Configurations;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Respawn.Graph;

namespace integrationTests.Restaurants;

internal class BaseRestaurantIntegrationTests : BaseContainerIntegrationTests<RestaurantDbContext>
{
    protected MenuDbContext MenuDbContext;
    protected EventDbContext EventDbContext;

    protected BaseRestaurantIntegrationTests(List<IContainer> containers) : base(containers)
    {
    }

    [OneTimeSetUp]
    protected override async Task OneTimeSetUp()
    {
        await base.OneTimeSetUp();
        Connection = DbContext.Database.GetDbConnection();
        MenuDbContext = await GetDifferentDbContext<MenuDbContext>();
        EventDbContext = await GetDifferentDbContext<EventDbContext>();
        await Connection.OpenAsync();
        Respawner = await Respawner.CreateAsync(Connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            // TablesToInclude = [.. RestaurantTables, new Table(MenuDatabaseConstants.Schema, MenuDatabaseConstants.Restaurants)],
            SchemasToInclude =
            [
                "public",
                RestaurantDatabaseConstants.SCHEMA,
                MenuDatabaseConstants.Schema,
                Constants.SCHEMA
            ]
        });

    }

    [OneTimeTearDown]
    public override async Task OneTimeTearDown()
    {
        await base.OneTimeTearDown();
        await Connection!.DisposeAsync();
    }
}