using infrastructure.Database.MenuContext;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Respawn.Graph;

namespace integrationTests.Menus
{
    public class BaseMenuIntegrationTests : BaseContainerIntegrationTests<MenuDbContext>
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
                    new Table("menu", "Groups"),
                    new Table("menu", "Ingredients"),
                    new Table("menu", "Meals"),
                    new Table("menu", "Menus")
                ],
                SchemasToInclude =
                [
                    "public",
                    "menu"
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
