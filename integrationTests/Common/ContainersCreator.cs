using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace integrationTests.Common
{
    internal static class ContainersCreator
    {
        public static IContainer Postgres
            => new PostgreSqlBuilder()
                .WithImage("postgres:14-alpine")
                .WithUsername("admin")
                .WithPassword("S3cret")
                .WithPortBinding("5431", "5432")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
                .Build();

        public static IContainer RabbitMq
            => new RabbitMqBuilder()
                .WithImage("rabbitmq:3")
                .WithUsername("guest")
                .WithPassword("guest")
                .WithPortBinding("5673", "5672")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
                .Build();
    }
}
