using System.Data;
using System.Data.Common;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
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
        var tasks = new List<Task>();
        foreach (var container in IntegrationSetup.Containers)
        {
            if(container.State != TestcontainersStates.Running && _containers.Contains(container))
                tasks.Add(container.StartAsync());
            else if (container.State is TestcontainersStates.Running or TestcontainersStates.Restarting && !_containers.Contains(container))
            {
                tasks.Add(container.StopAsync());
            }
        }

        await Task.WhenAll(tasks);
        Client = _factory.CreateClient();
        _scope = _factory.Services.CreateScope();
        await SetUpDbContext();
    }

    [SetUp]
    public async Task SetUp()
    {
        _scope = _factory.Services.CreateScope();
        var connString = DbContext.Database.GetConnectionString();
        await using var conn = new NpgsqlConnection(connString);
        await conn.OpenAsync();
        if(conn.State != ConnectionState.Open)
            await conn.OpenAsync();
        await Respawner!.ResetAsync(conn);

        foreach (var entity in DbContext.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }
    }

    [TearDown]
    public void TearDown()
    {
        _scope.Dispose();
    }

    [OneTimeTearDown]
    public virtual async Task OneTimeTearDown()
    {
        await _factory.DisposeAsync();
        Client.Dispose();
        await DbContext.DisposeAsync();
        _scope.Dispose();
    }

    private async Task SetUpDbContext()
    {
        DbContext = _scope.ServiceProvider.GetRequiredService<TDbContext>();
        try
        {
            await DbContext.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // throw;
        }
    }

    protected async Task<TDiffDbContext> GetDifferentDbContext<TDiffDbContext>()
        where TDiffDbContext : DbContext
    {
        var dbContext = _scope.ServiceProvider.GetRequiredService<TDiffDbContext>();
        try
        {
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // throw;
        }
        return dbContext;
    }

}