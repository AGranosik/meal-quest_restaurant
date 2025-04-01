using domain.Menus.ValueObjects;
using FluentAssertions;
using infrastructure.Database.MenuContext;
using infrastructure.Database.MenuContext.Models.Configurations;
using infrastructure.Database.MenuContext.Repositories;
using integrationTests.Common;
using Microsoft.EntityFrameworkCore;
using Respawn;
using domain.Menus.ValueObjects.Identifiers;

namespace integrationTests.Menus.RepositoryTests;

[TestFixture]
internal class MenuRepositoryTests : BaseContainerIntegrationTests<MenuDbContext>
{
    public MenuRepositoryTests() : base([ContainersCreator.Postgres])
    {
    }

    [Test]
    public async Task CreateRestaurant_Success()
    {
        const int restaurantId = 1;
        var repo = CreateRepository();
        var action = () =>
            repo.CreateRestaurantAsync(new MenuRestaurant(new RestaurantIdMenuId(restaurantId)),
                TestContext.CurrentContext.CancellationToken);
        await action.Should().NotThrowAsync();

        var dbRestaurant = await DbContext.Restaurants
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == new  RestaurantIdMenuId(restaurantId));
        dbRestaurant.Should().NotBeNull();
    }

    [Test]
    public async Task CreateRestaurant_AlreadyExists_Throws()
    {
        const int restaurantId = 1;
        var repo = CreateRepository();
        var restaurant = new MenuRestaurant(new RestaurantIdMenuId(restaurantId));
        await repo.CreateRestaurantAsync(restaurant, TestContext.CurrentContext.CancellationToken);
    DbContext.Restaurants.Entry(restaurant).State = EntityState.Detached;
        var action = () =>
            repo.CreateRestaurantAsync(new MenuRestaurant(new RestaurantIdMenuId(restaurantId)),
                TestContext.CurrentContext.CancellationToken);
        await action.Should().ThrowAsync<DbUpdateException>();
    }

    [Test]
    public async Task CreateMenu_CannotBeNull_ThrowsException()
    {
        var repo = CreateRepository();
        var action = () => repo.CreateMenuAsync(null!, CancellationToken.None);
        await action.Should().ThrowAsync<Exception>();
    }

    protected override async Task OneTimeSetUp()
    {
        await base.OneTimeSetUp();
        Connection = DbContext.Database.GetDbConnection();
        await Connection.OpenAsync();
        Respawner = await Respawner.CreateAsync(Connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            TablesToInclude = MenuTables,
            SchemasToInclude =
            [
                "public",
                MenuDatabaseConstants.Schema
            ]
        });
    }

    private MenuRepository CreateRepository()
        => new(DbContext);
}