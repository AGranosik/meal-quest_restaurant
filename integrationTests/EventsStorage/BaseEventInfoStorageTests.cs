using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
using FluentAssertions;
using infrastructure.EventStorage;
using infrastructure.EventStorage.DatabaseModels.Configurations;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Respawn.Graph;

namespace integrationTests.EventsStorage

    //TODO: unit tests
{
    [TestFixture]
    internal abstract class BaseEventInfoStorageTests<TAggregate, TKey> : BaseContainerIntegrationTests<EventDbContext>
        where TAggregate : Aggregate<TKey>
        where TKey : SimpleValueType<int, TKey>
    {
        protected override async Task OneTimeSetUp()
        {
            await base.OneTimeSetUp();

            _connection = _dbContext.Database.GetDbConnection();
            await _connection.OpenAsync();

            _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                TablesToInclude = [new Table(Constants.SCHEMA, typeof(TAggregate).Name + "s")]
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
            var action = () => handler.StorePendingEventAsync(CreateAggregate(), CancellationToken.None);
            await action.Should().NotThrowAsync();

            var storedEvent = await _dbContext.GetDbSet<TAggregate, TKey>().ToListAsync();
            storedEvent.Should().NotBeEmpty();
            storedEvent.Should().HaveCount(1);
        }


        protected virtual EventInfoStorage<TAggregate, TKey> CreateHandler()
            => new(_dbContext);

        public abstract TAggregate CreateAggregate();
    }
}
