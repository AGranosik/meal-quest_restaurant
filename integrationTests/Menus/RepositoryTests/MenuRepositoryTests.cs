using domain.Menus.ValueObjects;
using FluentAssertions;
using infrastructure.Database.MenuContext;
using infrastructure.Database.MenuContext.Models.Configurations;
using infrastructure.Database.MenuContext.Repositories;
using integrationTests.Common;
using Microsoft.EntityFrameworkCore;
using Respawn;
using domain.Menus.ValueObjects.Identifiers;
using Npgsql;
using sharedTests.DataFakers;

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
            .FirstOrDefaultAsync(r => r.Id! == new  RestaurantIdMenuId(restaurantId));
        dbRestaurant.Should().NotBeNull();
    }

    [Test]
    public async Task CreateRestaurant_AlreadyExists_Throws()
    {
        const int restaurantId = 1;
        var repo = CreateRepository();
        var restaurant = new MenuRestaurant(new RestaurantIdMenuId(restaurantId));
        await repo.CreateRestaurantAsync(restaurant, TestContext.CurrentContext.CancellationToken);
        
        DbContext.ChangeTracker.Clear();
        var action = () =>
            repo.CreateRestaurantAsync(new MenuRestaurant(new RestaurantIdMenuId(restaurantId)),
                TestContext.CurrentContext.CancellationToken);
        await action.Should().ThrowAsync<DbUpdateException>();
    }

    [Test]
    public async Task CreateMenu_CannotBeNull_ThrowsException()
    {
        var repo = CreateRepository();
        var action = () => repo.CreateMenuAsync(null!, TestContext.CurrentContext.CancellationToken);
        await action.Should().ThrowAsync<Exception>();
    }
    

    
    [Test]
    public async Task CreateMenu_AlreadyActiveExists_Throws()
    {
        var menu = MenuDataFaker.ValidMenu();
        var repo = CreateRepository();
        await CreateRestaurant(repo, menu.Restaurant.Id!.Value);
        
        await repo.CreateMenuAsync(menu, TestContext.CurrentContext.CancellationToken);
        
        DbContext.ChangeTracker.Clear();
        var menu2 = MenuDataFaker.ValidMenu();
        var action = () => repo.CreateMenuAsync(menu2, TestContext.CurrentContext.CancellationToken);
        await action.Should().ThrowAsync<DbUpdateException>();
    }
    
    [Test]
    public async Task CreateInactiveMenu_AlreadyActiveExists_Success()
    {
        var menu = MenuDataFaker.ValidMenu();
        var repo = CreateRepository();
        await CreateRestaurant(repo, menu.Restaurant.Id!.Value);
        
        await repo.CreateMenuAsync(menu, TestContext.CurrentContext.CancellationToken);
        
        DbContext.ChangeTracker.Clear();
        var menu2 = MenuDataFaker.ValidMenu(false, menu.Id!.Value + 1);
        var action = () => repo.CreateMenuAsync(menu2, TestContext.CurrentContext.CancellationToken);
        await action.Should().NotThrowAsync();
    
        var menus = await DbContext.Menus.ToListAsync();
        menus.Should().Contain(menu => menu.Id! == menu2.Id!);
        menus.Count.Should().Be(2);
    }
    
    [Test]
    public async Task CreateMenu_Success()
    {
        var menu = MenuDataFaker.ValidMenu();
        var repo = CreateRepository();
        await CreateRestaurant(repo, menu.Restaurant.Id!.Value);
        
        await repo.CreateMenuAsync(menu, TestContext.CurrentContext.CancellationToken);
        var dbMenu = DbContext.Menus.FirstAsync(m => m.Id == menu.Id);
        dbMenu.Should().NotBeNull();
    }

    protected override async Task OneTimeSetUp()
    {
        await base.OneTimeSetUp();
        var connString = DbContext.Database.GetConnectionString();
        await using var conn = new NpgsqlConnection(connString);
        await conn.OpenAsync();
        Respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude =
            [
                "public",
                MenuDatabaseConstants.Schema
            ]
        });
    }

    private MenuRepository CreateRepository()
        => new(DbContext);

    private static Task CreateRestaurant(MenuRepository repository, int restaurantId)
        => repository.CreateRestaurantAsync(new MenuRestaurant(new RestaurantIdMenuId(restaurantId)),
            TestContext.CurrentContext.CancellationToken);
}