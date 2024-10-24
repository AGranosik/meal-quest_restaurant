using application.EventHandlers;
using application.EventHandlers.Interfaces;
using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
using FluentAssertions;
using Moq;

namespace unitTests.Application.EventHandlers
{
    [TestFixture]
    public abstract class AggregateChangedEventHandlerTests<TAggregate, TKey>
        where TKey : SimpleValueType<int, TKey>
        where TAggregate : Aggregate<TKey>
    {
        protected Mock<IEventInfoStorage<TAggregate, TKey>> _eventInforStorage;

        [SetUp]
        public virtual void SetUp()
        {
            _eventInforStorage = new Mock<IEventInfoStorage<TAggregate, TKey>>();
        }

        [Test]
        public async Task Handle_EventCannotBeNull_ThrowsException()
        {
            var handler = CreateHandler();
            var action = () => handler.Handle(null!, CancellationToken.None);
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task PendingState_Invoked()
        {
            var notification = CreateValidEvent();
            _eventInforStorage.Setup(e => e.StorePendingEventAsync(It.IsAny<TKey>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var handler = CreateHandler();
            var action = () => handler.Handle(notification, CancellationToken.None);

            await action.Should().NotThrowAsync();
            _eventInforStorage.Verify(e => e.StorePendingEventAsync(It.Is<TKey>(x => x.Value == notification.Aggregate.Id!.Value), It.IsAny<CancellationToken>()));
        }

        protected abstract AggregateChangedEventHandler<TAggregate, TKey> CreateHandler();
        protected abstract AggregateChangedEvent<TAggregate, TKey> CreateValidEvent();
    }
}
