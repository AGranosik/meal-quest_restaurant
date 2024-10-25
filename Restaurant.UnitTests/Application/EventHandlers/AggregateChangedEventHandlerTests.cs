using application.EventHandlers;
using application.EventHandlers.Interfaces;
using core.FallbackPolicies;
using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace unitTests.Application.EventHandlers
{
    [TestFixture]
    public abstract class AggregateChangedEventHandlerTests<TAggregate, TKey>
        where TKey : SimpleValueType<int, TKey>
        where TAggregate : Aggregate<TKey>
    {
        protected Mock<IEventInfoStorage<TAggregate, TKey>> _eventInforStorage;
        protected Mock<ILogger<AggregateChangedEventHandler<TAggregate, TKey>>> _loggerMock;

        [SetUp]
        public virtual void SetUp()
        {
            _eventInforStorage = new Mock<IEventInfoStorage<TAggregate, TKey>>();
            _loggerMock = new Mock<ILogger<AggregateChangedEventHandler<TAggregate, TKey>>>();
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
            _eventInforStorage.Verify(e => e.StorePendingEventAsync(It.Is<TKey>(x => x.Value == notification.Aggregate.Id!.Value), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task PednigState_Failed_RetryPolicyApplied()
        {
            var notification = CreateValidEvent();
            _eventInforStorage.Setup(e => e.StorePendingEventAsync(It.IsAny<TKey>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException());

            var handler = CreateHandler();
            var action = () => handler.Handle(notification, CancellationToken.None);

            await action.Should().NotThrowAsync();
            _eventInforStorage.Verify(e => e.StorePendingEventAsync(It.Is<TKey>(x => x.Value == notification.Aggregate.Id!.Value), It.IsAny<CancellationToken>()), Times.Exactly(FallbackRetryPoicies.NUMBER_OF_RETRIES + 1));
        }

        [Test]
        public async Task PednigState_Failed_MessageLogged()
        {
            var notification = CreateValidEvent();
            _eventInforStorage.Setup(e => e.StorePendingEventAsync(It.IsAny<TKey>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException());
            _loggerMock.Setup(l => l.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));

            var handler = CreateHandler();
            var action = () => handler.Handle(notification, CancellationToken.None);

            await action.Should().NotThrowAsync();
            _eventInforStorage.Verify(e => e.StorePendingEventAsync(It.Is<TKey>(x => x.Value == notification.Aggregate.Id!.Value), It.IsAny<CancellationToken>()), Times.Exactly(FallbackRetryPoicies.NUMBER_OF_RETRIES + 1));
            _loggerMock.Verify(l => l.Log(It.Is<LogLevel>(lvl => lvl == LogLevel.Error), It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Once());
        }

        protected abstract AggregateChangedEventHandler<TAggregate, TKey> CreateHandler();
        protected abstract AggregateChangedEvent<TAggregate, TKey> CreateValidEvent();
    }
}
