using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Testcontainers.PostgreSql;

namespace integrationTests
{
    public class BaseContainerIntegrationTests
    {
        protected IContainer _postgresContainer;
        private ApiWebApplicationFactory _factory;

        protected HttpClient _client;

        public BaseContainerIntegrationTests()
        {
            _postgresContainer = new PostgreSqlBuilder()
                .WithImage("postgres:14-alpine")
                .WithUsername("admin")
                .WithPassword("S3cret")
                .WithPortBinding(5432)
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
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await _postgresContainer.StopAsync();
            await _postgresContainer.DisposeAsync();
            _factory.Dispose();
            _client.Dispose();
        }


    }
}
