using infrastructure.Database.MenuContext.Models.Configurations;
using infrastructure.Database.RestaurantContext.Models.Configurations;
using infrastructure.Database.RestaurantContext;
using infrastructure.EventStorage;
using Respawn;
using infrastructure.Database.MenuContext;
using Microsoft.EntityFrameworkCore;
using DotNet.Testcontainers.Containers;
using integrationTests.Common;

namespace integrationTests.Menus
{
    [TestFixture]
    internal class CreateMenuEventEmitterFailureTests : BaseContainerIntegrationTests<MenuDbContext>
    {
        protected RestaurantDbContext _restaurantDbContext;
        protected EventDbContext _eventDbContext;

        public CreateMenuEventEmitterFailureTests() : base([ContainersCreator.Postgres])
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
            _connection!.Dispose();
        }
    }
}
