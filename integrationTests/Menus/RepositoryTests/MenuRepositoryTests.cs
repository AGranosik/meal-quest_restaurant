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

    //TODO: TESTS CHECKING IF CATEGORIES ARE CREATED
    [Test]
    public async Task CreateRestaurant_Success()
    {
        var restuarantId = 1;
        var repo = CreateRepository();
        var action = () =>
            repo.CreateRestaurantAsync(new MenuRestaurant(new RestaurantIdMenuId(restuarantId)),
                CancellationToken.None);
        await action.Should().NotThrowAsync();

        var dbRestuarant = await _dbContext.Restaurants.FirstOrDefaultAsync(r => r.Id!.Value == restuarantId);
        dbRestuarant.Should().NotBeNull();
    }

    [Test]
    public async Task CreateRestaurant_AlreadyExists_NotThrow()
    {
        var restuarantId = 1;
        var repo = CreateRepository();
        await repo.CreateRestaurantAsync(new MenuRestaurant(new RestaurantIdMenuId(restuarantId)),
            CancellationToken.None);

        var action = () =>
            repo.CreateRestaurantAsync(new MenuRestaurant(new RestaurantIdMenuId(restuarantId)),
                CancellationToken.None);
        await action.Should().NotThrowAsync();
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
        _connection = _dbContext.Database.GetDbConnection();
        await _connection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            TablesToInclude = _MenuTables,
            SchemasToInclude =
            [
                "public",
                MenuDatabaseConstants.Schema
            ]
        });
    }

    private MenuRepository CreateRepository()
        => new(_dbContext);
}