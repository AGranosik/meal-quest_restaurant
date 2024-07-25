using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Testcontainers.PostgreSql;

namespace integrationTests
{
    public class BaseContainerIntegratioNTests
    {
        public static IContainer _postgresContainer;

        public BaseContainerIntegrationTests()
        {
            _postgresContainer = new PostgreSqlBuilder()
                .WithImage("postgres:14-alpine")
                .WithUsername("admin")
                .WithPassword("S3cret")
                .WithPortBinding(5432)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
                .Build();
        }
    }
}
