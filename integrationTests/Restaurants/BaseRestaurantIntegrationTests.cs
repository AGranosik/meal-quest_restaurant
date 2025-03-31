using DotNet.Testcontainers.Containers;
using infrastructure.Database.MenuContext;
using infrastructure.Database.MenuContext.Models.Configurations;
using infrastructure.Database.RestaurantContext;
using infrastructure.Database.RestaurantContext.Models.Configurations;
using infrastructure.EventStorage;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Respawn.Graph;

namespace integrationTests.Restaurants;

internal class BaseRestaurantIntegrationTests : BaseContainerIntegrationTests<RestaurantDbContext>
{
    protected MenuDbContext _menuDbContext;
    protected EventDbContext _eventDbContext;

    public BaseRestaurantIntegrationTests(List<IContainer> containers) : base(containers)
    {
    }

    protected override async Task OneTimeSetUp()
    {
        await base.OneTimeSetUp();
        Connection = DbContext.Database.GetDbConnection();
        await Connection.OpenAsync();

        Respawner = await Respawner.CreateAsync(Connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            TablesToInclude = [.. RestaurantTables, new Table(MenuDatabaseConstants.Schema, MenuDatabaseConstants.Restaurants)],
            SchemasToInclude =
            [
                "public",
                RestaurantDatabaseConstants.SCHEMA,
                MenuDatabaseConstants.Schema
            ]
        });

        _menuDbContext = await GetDifferentDbContext<MenuDbContext>();
        _eventDbContext = await GetDifferentDbContext<EventDbContext>();
    }

    public override async Task OneTimeTearDown()
    {
        await base.OneTimeTearDown();
        await Connection!.DisposeAsync();
    }
}