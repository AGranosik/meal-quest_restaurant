using System.Data.Common;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;

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
    public BaseContainerIntegrationTests(List<IContainer> containers)
    {
        _containers = containers;
        _factory = new ApiWebApplicationFactory();
    }

    [OneTimeSetUp]
    protected virtual async Task OneTimeSetUp()
    {
        // await StartContainersAsync();
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
        // await StopContainersAsync();

        await _factory.DisposeAsync();
        Client.Dispose();
        await DbContext.DisposeAsync();
        _scope.Dispose();
    }

    private void SetUpDbContext()
    {
        DbContext = _scope.ServiceProvider.GetRequiredService<TDbContext>();
    }

    protected async Task<TDiffDbContext> GetDifferentDbContext<TDiffDbContext>()
        where TDiffDbContext : DbContext
    {
        var dbContext = _scope.ServiceProvider.GetRequiredService<TDiffDbContext>();
        var migrations = await dbContext.Database.GetAppliedMigrationsAsync();
        if(!migrations.Any())
            await dbContext.Database.MigrateAsync();
        return dbContext;
    }

}