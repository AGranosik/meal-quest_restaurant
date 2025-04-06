using domain.Common.DomainImplementationTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
using FluentAssertions;
using infrastructure.EventStorage;
using infrastructure.EventStorage.DatabaseModels;
using infrastructure.EventStorage.DatabaseModels.Configurations;
using integrationTests.Common;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Respawn.Graph;

namespace integrationTests.EventsStorage;

[TestFixture]
internal abstract class BaseEventInfoStorageTests<TAggregate, TKey> : BaseContainerIntegrationTests<EventDbContext>
    where TAggregate : Aggregate<TKey>
    where TKey : SimpleValueType<int, TKey>
{
    protected BaseEventInfoStorageTests() : base([ContainersCreator.Postgres, ContainersCreator.RabbitMq])
    {
    }

    protected override async Task OneTimeSetUp()
    {
        await base.OneTimeSetUp();

        Connection = DbContext.Database.GetDbConnection();
        await Connection.OpenAsync();

        Respawner = await Respawner.CreateAsync(Connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            TablesToInclude = [new Table(Constants.SCHEMA, typeof(TAggregate).Name + "s")],
            SchemasToInclude = [
                "public",
                Constants.SCHEMA
            ]
        });
    }

    [Test]
    public async Task Pending_NoException()
    {
        var handler = CreateHandler();
        var action = () => handler.StorePendingEventAsync(CreateAggregate(), CancellationToken.None);
        await action.Should().NotThrowAsync();
    }

    [Test]
    public async Task Pending_StoredSuccessfully()
    {
        var handler = CreateHandler();
        await StorePendingEvent(handler);
        var storedEvent = await DbContext.GetDbSet<TAggregate, TKey>().ToListAsync();
        storedEvent.Should().NotBeEmpty();
        storedEvent.Should().HaveCount(1);
    }

    [Test]
    public async Task StoreFailure_NoException()
    {
        var handler = CreateHandler();
        await StorePendingEvent(handler);

        var storedEvent = await GetStoredEventAsync();

        var action = () => handler.StoreFailureAsync(storedEvent.EventId, CancellationToken.None);
        await action.Should().NotThrowAsync();
    }

    [Test]
    public async Task StoreFailure_SavedSuccessfully()
    {
        var handler = CreateHandler();
        await StorePendingEvent(handler);

        var storedEvent = await GetStoredEventAsync();

        await handler.StoreFailureAsync(storedEvent.EventId, CancellationToken.None);
        var faieldEvent = await GetStoredEventAsync();
        faieldEvent.HandlingStatus.Should().Be(HandlingStatus.Failed);
    }

    [Test]
    public async Task StoreSuccess_NoException()
    {
        var handler = CreateHandler();
        await StorePendingEvent(handler);

        var storedEvent = await GetStoredEventAsync();

        var action = () => handler.StoreSuccessAsyncAsync(storedEvent.EventId, CancellationToken.None);
        await action.Should().NotThrowAsync();
    }

    [Test]
    public async Task StoreSuccess_SavedSuccessfully()
    {
        var handler = CreateHandler();
        await StorePendingEvent(handler);

        var storedEvent = await GetStoredEventAsync();

        await handler.StoreSuccessAsyncAsync(storedEvent.EventId, CancellationToken.None);
        var faieldEvent = await GetStoredEventAsync();
        faieldEvent.HandlingStatus.Should().Be(HandlingStatus.Propagated);
    }

    private async Task StorePendingEvent(EventInfoStorage<TAggregate, TKey> handler)
    {
        var action = () => handler.StorePendingEventAsync(CreateAggregate(), CancellationToken.None);
        await action.Should().NotThrowAsync();
    }

    private async Task<DomainEventModel<TAggregate, TKey>> GetStoredEventAsync()
    {
        var storedEvents = await DbContext.GetDbSet<TAggregate, TKey>().AsNoTracking().ToListAsync();
        storedEvents.Should().HaveCount(1);
        return storedEvents[0];
    }

    protected virtual EventInfoStorage<TAggregate, TKey> CreateHandler()
        => new(DbContext);

    protected abstract TAggregate CreateAggregate();
}