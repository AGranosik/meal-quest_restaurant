using DotNet.Testcontainers.Containers;
using infrastructure.Database.MenuContext;
using infrastructure.Database.MenuContext.Models.Configurations;
using infrastructure.Database.RestaurantContext;
using infrastructure.Database.RestaurantContext.Models.Configurations;
using infrastructure.EventStorage;
using Microsoft.EntityFrameworkCore;
using Respawn;

namespace integrationTests.Menus;

internal class BaseMenuIntegrationTests : BaseContainerIntegrationTests<MenuDbContext>
{
    protected RestaurantDbContext RestaurantDbContext;
    protected EventDbContext EventDbContext;

    protected BaseMenuIntegrationTests(List<IContainer> containers) : base(containers)
    {
    }

    protected override async Task OneTimeSetUp()
    {
        await base.OneTimeSetUp();
        Connection = DbContext.Database.GetDbConnection();
        await Connection.OpenAsync();
        var tables = RestaurantTables.Concat(MenuTables);
        Respawner = await Respawner.CreateAsync(Connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            TablesToInclude = tables.ToArray(),
            SchemasToInclude =
            [
                "public",
                RestaurantDatabaseConstants.SCHEMA,
                MenuDatabaseConstants.Schema
            ]
        });

        EventDbContext = await GetDifferentDbContext<EventDbContext>();
        RestaurantDbContext = await GetDifferentDbContext<RestaurantDbContext>();
    }

    public override async Task OneTimeTearDown()
    {
        await base.OneTimeTearDown();
        await Connection!.DisposeAsync();
    }
}