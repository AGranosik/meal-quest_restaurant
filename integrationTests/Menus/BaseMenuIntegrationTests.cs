using DotNet.Testcontainers.Containers;
using infrastructure.Database.MenuContext;
using infrastructure.Database.MenuContext.Models.Configurations;
using infrastructure.Database.RestaurantContext;
using infrastructure.Database.RestaurantContext.Models.Configurations;
using infrastructure.EventStorage;
using Microsoft.EntityFrameworkCore;
using Respawn;

namespace integrationTests.Menus
{
    internal class BaseMenuIntegrationTests : BaseContainerIntegrationTests<MenuDbContext>
    {
        protected RestaurantDbContext _restaurantDbContext;
        protected EventDbContext _eventDbContext;

        public BaseMenuIntegrationTests(List<IContainer> containers) : base(containers)
        {
        }

        protected override async Task OneTimeSetUp()
        {
            await base.OneTimeSetUp();
            _connection = _dbContext.Database.GetDbConnection();
            await _connection.OpenAsync();
            var tables = _restaurantTables.Concat(_MenuTables);
            _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                TablesToInclude = tables.ToArray(),
                SchemasToInclude =
                [
                    "public",
                    RestaurantDatabaseConstants.SCHEMA,
                    MenuDatabaseConstants.SCHEMA
                ]
            });

            _eventDbContext = await GetDifferentDbContext<EventDbContext>();
            _restaurantDbContext = await GetDifferentDbContext<RestaurantDbContext>();
        }

        public override async Task OneTimeTearDown()
        {
            await base.OneTimeTearDown();
            await _connection!.DisposeAsync();
        }
    }
}
