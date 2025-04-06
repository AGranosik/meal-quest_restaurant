using DotNet.Testcontainers.Containers;
using infrastructure.Database.MenuContext;
using integrationTests.Common;

namespace integrationTests;

[SetUpFixture]
internal sealed class TestIntegrationSetUp
{
    private readonly List<IContainer> _containers =
    [
        ContainersCreator.Postgres,
        ContainersCreator.RabbitMq
    ];
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await StartContainersAsync(_containers);
        
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        var tasks = new List<Task>(_containers.Count);
        tasks.AddRange(_containers.Select(container => container.StopAsync()));

        await Task.WhenAll(tasks);

        foreach(var container in _containers)
        {
            await container.DisposeAsync();
        }
    }
    
    
    private static async Task StartContainersAsync(List<IContainer> containers)
    {
        var tasks = new List<Task>(containers.Count);
        tasks.AddRange(from container in containers where container.State != TestcontainersStates.Running select container.StartAsync());

        await Task.WhenAll(tasks);
    }
}