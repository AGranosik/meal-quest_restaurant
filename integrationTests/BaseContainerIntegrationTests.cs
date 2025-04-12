using System.Data;
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
        await SetUpDbContext();
    }

    [SetUp]
    public async Task SetUp()
    {
        _scope = _factory.Services.CreateScope();
        // await SetUpDbContext();
        var connection = DbContext.Database.GetDbConnection();
        if(connection.State != ConnectionState.Open)
            await connection.OpenAsync();
        await Respawner!.ResetAsync(connection);

        foreach (var entity in DbContext.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }
    }

    [TearDown]
    public void TearDown()
    {
        // DbContext.Dispose();
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

    private async Task SetUpDbContext()
    {
        DbContext = _scope.ServiceProvider.GetRequiredService<TDbContext>();
        var migrations = await DbContext.Database.GetAppliedMigrationsAsync();
        //for some reasone sometimes they are nto applied 
        if (!migrations.Any())
        {
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
    }

    protected async Task<TDiffDbContext> GetDifferentDbContext<TDiffDbContext>()
        where TDiffDbContext : DbContext
    {
        var dbContext = _scope.ServiceProvider.GetRequiredService<TDiffDbContext>();
        // var migrations = await dbContext.Database.GetAppliedMigrationsAsync();
        // //for some reasone sometimes they are nto applied 
        // if (migrations.Any()) return dbContext;
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