using DotNet.Testcontainers.Containers;
using integrationTests.Common;

namespace integrationTests;

[SetUpFixture]
internal sealed class IntegrationSetup
{
    internal static readonly List<IContainer> Containers = [ContainersCreator.Postgres, ContainersCreator.RabbitMq];

    
    [OneTimeSetUp]
    public async Task StartContainersAsync()
    {
        var tasks = new List<Task>(Containers.Count);
        foreach (var container in Containers)
        {
            if (container.State != TestcontainersStates.Running)
                tasks.Add(container.StartAsync());
        }

        await Task.WhenAll(tasks);
    }

    [OneTimeTearDown]
    public async Task StopContainersAsync()
    {
        var tasks = new List<Task>(Containers.Count);
        tasks.AddRange(Containers.Select(container => container.StopAsync()));

        await Task.WhenAll(tasks);

        foreach (var container in Containers)
        {
            await container.DisposeAsync();
        }
    }
}