using infrastructure.Database.MenuContext;
using infrastructure.Database.RestaurantContext;
using infrastructure.EventStorage;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Respawn.Graph;

namespace integrationTests.Restaurants
{
    public class BaseRestaurantIntegrationTests : BaseContainerIntegrationTests<RestaurantDbContext>
    {
        protected MenuDbContext _menuDbContext;
        protected EventDbContext _eventDbContext;
        public BaseRestaurantIntegrationTests()
        {
            
        }
        protected override async Task OneTimeSetUp()
        {
            await base.OneTimeSetUp();
            _connection = _dbContext.Database.GetDbConnection();
            await _connection.OpenAsync();
            _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                TablesToInclude =
                [
                    new Table("restaurant", "WorkingDays"),
                    new Table("restaurant", "Restaurants"),
                    new Table("restaurant", "OpeningHours"),
                    new Table("restaurant", "Addresses"),
                    new Table("restaurant", "Owners"),
                    new Table("restaurant", "Restaurants"),
                    new Table("menu", "Restaurants"),
                    new Table("event", "RestaurantEvents"),
                ],
                SchemasToInclude =
                [
                    "public",
                    "restaurant",
                    "menu",
                    "event"
                ]
            });

            _menuDbContext = await GetDifferentDbContext<MenuDbContext>();
            _eventDbContext = await GetDifferentDbContext<EventDbContext>();
        }

        public override async Task OneTimeTearDown()
        {
            await base.OneTimeTearDown();
            _connection!.Dispose();
        }
    }
}
