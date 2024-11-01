using System.Data.Common;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using infrastructure.Database.MenuContext.Models.Configurations;
using infrastructure.Database.RestaurantContext.Models.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Respawn.Graph;
using Testcontainers.PostgreSql;

namespace integrationTests
{
    public class BaseContainerIntegrationTests<TDbContext>
        where TDbContext : DbContext
    {
        protected IContainer _postgresContainer;
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

        public BaseContainerIntegrationTests()
        {
            _postgresContainer = new PostgreSqlBuilder()
                .WithImage("postgres:14-alpine")
                .WithUsername("admin")
                .WithPassword("S3cret")
                .WithPortBinding("5431", "5432")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
                .Build();

            _factory = new ApiWebApplicationFactory();
        }

        [OneTimeSetUp]
        protected virtual async Task OneTimeSetUp()
        {
            if (_postgresContainer.State != TestcontainersStates.Running)
                await _postgresContainer.StartAsync();

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
            await _postgresContainer.StopAsync();
            await _postgresContainer.DisposeAsync();
            await _factory.DisposeAsync();
            _client.Dispose();
            await _dbContext.DisposeAsync();
            _scope.Dispose();
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
