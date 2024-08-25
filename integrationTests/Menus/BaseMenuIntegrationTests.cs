using infrastructure.Database.MenuContext;
using infrastructure.Database.RestaurantContext;
using infrastructure.EventStorage;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Respawn.Graph;

namespace integrationTests.Menus
{
    public class BaseMenuIntegrationTests : BaseContainerIntegrationTests<MenuDbContext>
    {
        protected RestaurantDbContext _restaurantDbContext;
        protected EventDbContext _eventDbContext;

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
                    new Table("menu", "Groups"),
                    new Table("menu", "Ingredients"),
                    new Table("menu", "Meals"),
                    new Table("menu", "Menus"),
                    new Table("menu", "Restaurants"),
                    // move to some configuration class
                    new Table("restaurant", "WorkingDays"),
                    new Table("restaurant", "OpeningHours"),
                    new Table("restaurant", "Addresses"),
                    new Table("restaurant", "Owners"),
                    new Table("restaurant", "Restaurants"),
                ],
                SchemasToInclude =
                [
                    "public",
                    "menu",
                    "restaurant",
                    "event"
                ]
            });

            _eventDbContext = await GetDifferentDbContext<EventDbContext>();
            _restaurantDbContext = await GetDifferentDbContext<RestaurantDbContext>();
        }

        public override async Task OneTimeTearDown()
        {
            await base.OneTimeTearDown();
            _connection.Dispose();
        }
    }
}
