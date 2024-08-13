using Microsoft.EntityFrameworkCore;
using Respawn;
using Respawn.Graph;

namespace integrationTests.Menus
{
    public class BaseMenuIntegrationTests : BaseContainerIntegrationTests
    {
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
                ],
                SchemasToInclude =
                [
                    "public",
                    "restaurant"
                ]
            });
        }

        public override async Task OneTimeTearDown()
        {
            await base.OneTimeTearDown();
            _connection.Dispose();
        }
    }
}
