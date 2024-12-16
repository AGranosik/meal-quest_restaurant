using System.Data.Common;
using DotNet.Testcontainers.Containers;
using infrastructure.Database.MenuContext.Models.Configurations;
using infrastructure.Database.RestaurantContext.Models.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Respawn.Graph;

namespace integrationTests
{
    internal class BaseContainerIntegrationTests<TDbContext>
        where TDbContext : DbContext
    {
        private readonly List<IContainer> _containers;
        private readonly ApiWebApplicationFactory _factory;

        protected HttpClient _client;
        protected TDbContext _dbContext;
        protected IServiceScope _scope;
        protected Respawner? _respawner;
        protected DbConnection? _connection;

        protected Table[] _restaurantTables =
        [
            new Table(RestaurantDatabaseConstants.SCHEMA, RestaurantDatabaseConstants.WORKINGDAYS),
            new Table(RestaurantDatabaseConstants.SCHEMA, RestaurantDatabaseConstants.OPENINGHOURS),
            new Table(RestaurantDatabaseConstants.SCHEMA, RestaurantDatabaseConstants.ADDRESSES),
            new Table(RestaurantDatabaseConstants.SCHEMA, RestaurantDatabaseConstants.OWNERS),
            new Table(RestaurantDatabaseConstants.SCHEMA, RestaurantDatabaseConstants.RESTAURANTS)
        ];

        protected Table[] _MenuTables =
        [
            new Table(MenuDatabaseConstants.SCHEMA, MenuDatabaseConstants.GROUPS),
            new Table(MenuDatabaseConstants.SCHEMA, MenuDatabaseConstants.INGREDIENTS),
            new Table(MenuDatabaseConstants.SCHEMA, MenuDatabaseConstants.MEALS),
            new Table(MenuDatabaseConstants.SCHEMA, MenuDatabaseConstants.MENUS),
            new Table(MenuDatabaseConstants.SCHEMA, MenuDatabaseConstants.RESTAURANTS)
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
            _client = _factory.CreateClient();
            _scope = _factory.Services.CreateScope();
            SetUpDbContext();
            await _dbContext.Database.EnsureCreatedAsync();
        }

        [SetUp]
        public async Task SetUp()
        {
            _scope = _factory.Services.CreateScope();
            SetUpDbContext();
            var connection = _dbContext.Database.GetDbConnection();
            await connection.OpenAsync();
            await _respawner!.ResetAsync(connection);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
            _scope.Dispose();
        }

        [OneTimeTearDown]
        public virtual async Task OneTimeTearDown()
        {
            await StopContainersAsync();

            await _factory.DisposeAsync();
            _client.Dispose();
            await _dbContext.DisposeAsync();
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
            _dbContext = _scope.ServiceProvider.GetRequiredService<TDbContext>();
        }

        protected async Task<DiffDbContext> GetDifferentDbContext<DiffDbContext>()
            where DiffDbContext : DbContext
        {
            var dbContext = _scope.ServiceProvider.GetRequiredService<DiffDbContext>();
            await dbContext.Database.MigrateAsync();
            return dbContext;
        }

    }
}
