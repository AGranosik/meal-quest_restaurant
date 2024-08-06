using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using infrastructure.Database.RestaurantContext;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace integrationTests
{
    public class BaseContainerIntegrationTests
    {
        protected IContainer _postgresContainer;
        private ApiWebApplicationFactory _factory;

        protected HttpClient _client;
        protected RestaurantDbContext _dbContext;
        protected IServiceScope _scope;

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
        public async Task OneTimeSetUp()
        {
            if (_postgresContainer.State != TestcontainersStates.Running)
                await _postgresContainer.StartAsync();

            _client = _factory.CreateClient();
            _scope = _factory.Services.CreateScope();
            SetUpDbContext();

            // create once just purge
            await _dbContext.Database.EnsureCreatedAsync();
        }

        [SetUp]
        public void SetUp()
        {
            _scope = _factory.Services.CreateScope();
            SetUpDbContext();
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
            _scope.Dispose();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await _postgresContainer.StopAsync();
            await _postgresContainer.DisposeAsync();
            _factory.Dispose();
            _client.Dispose();
            _dbContext.Dispose();
            _scope.Dispose();
        }


        private void SetUpDbContext()
        {
            _dbContext = _scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();
        }

    }
}
