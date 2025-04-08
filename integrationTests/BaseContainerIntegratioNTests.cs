using System.Data.Common;
using DotNet.Testcontainers.Containers;
using infrastructure.Database.MenuContext.Models.Configurations;
using infrastructure.Database.RestaurantContext.Models.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Respawn.Graph;

namespace integrationTests;

internal class BaseContainerIntegrationTests<TDbContext>
    where TDbContext : DbContext
{
    private readonly List<IContainer> _containers;
    private readonly ApiWebApplicationFactory _factory;

    protected HttpClient Client;
    protected TDbContext DbContext;
    private IServiceScope _scope;
    protected Respawner? Respawner;
    protected DbConnection? Connection;

    protected readonly Table[] RestaurantTables =
    [
        new Table(RestaurantDatabaseConstants.SCHEMA, RestaurantDatabaseConstants.WORKINGDAYS),
        new Table(RestaurantDatabaseConstants.SCHEMA, RestaurantDatabaseConstants.OPENINGHOURS),
        new Table(RestaurantDatabaseConstants.SCHEMA, RestaurantDatabaseConstants.ADDRESSES),
        new Table(RestaurantDatabaseConstants.SCHEMA, RestaurantDatabaseConstants.OWNERS),
        new Table(RestaurantDatabaseConstants.SCHEMA, RestaurantDatabaseConstants.RESTAURANTS)
    ];

    protected readonly Table[] MenuTables =
    [
        new Table(MenuDatabaseConstants.Schema, MenuDatabaseConstants.Groups),
        new Table(MenuDatabaseConstants.Schema, MenuDatabaseConstants.Ingredients),
        new Table(MenuDatabaseConstants.Schema, MenuDatabaseConstants.Meals),
        new Table(MenuDatabaseConstants.Schema, MenuDatabaseConstants.Menus),
        new Table(MenuDatabaseConstants.Schema, MenuDatabaseConstants.Restaurants),
        new Table(MenuDatabaseConstants.Schema, MenuDatabaseConstants.Categories)
    ];

    public BaseContainerIntegrationTests(List<IContainer> containers)
    {
        _containers = containers;
        _factory = new ApiWebApplicationFactory();
    }

    [OneTimeSetUp]
    protected virtual async Task OneTimeSetUp()
    {
        await StartContainersAsync();
        Client = _factory.CreateClient();
        _scope = _factory.Services.CreateScope();
        SetUpDbContext();
        await DbContext.Database.EnsureCreatedAsync();
    }

    [SetUp]
    public async Task SetUp()
    {
        _scope = _factory.Services.CreateScope();
        SetUpDbContext();
        var connection = DbContext.Database.GetDbConnection();
        await connection.OpenAsync();
        await Respawner!.ResetAsync(connection);
    }

    [TearDown]
    public void TearDown()
    {
        DbContext.Dispose();
        _scope.Dispose();
    }

    [OneTimeTearDown]
    public virtual async Task OneTimeTearDown()
    {
        await StopContainersAsync();

        await _factory.DisposeAsync();
        Client.Dispose();
        await DbContext.DisposeAsync();
        _scope.Dispose();
    }

    private async Task StartContainersAsync()
    {
        var tasks = new List<Task>(_containers.Count);
        foreach(var container in _containers)
        {
            if(container.State != TestcontainersStates.Running)
                tasks.Add(container.StartAsync());
        }

        await Task.WhenAll(tasks);
    }

    private async Task StopContainersAsync()
    {
        var tasks = new List<Task>(_containers.Count);
        foreach(var container in _containers)
        {
            tasks.Add(container.StopAsync());
        }

        await Task.WhenAll(tasks);

        foreach(var container in _containers)
        {
            await container.DisposeAsync();
        }
    }

    private void SetUpDbContext()
    {
        DbContext = _scope.ServiceProvider.GetRequiredService<TDbContext>();
    }

    protected async Task<TDiffDbContext> GetDifferentDbContext<TDiffDbContext>()
        where TDiffDbContext : DbContext
    {
        var dbContext = _scope.ServiceProvider.GetRequiredService<TDiffDbContext>();
        await dbContext.Database.MigrateAsync();
        return dbContext;
    }

}