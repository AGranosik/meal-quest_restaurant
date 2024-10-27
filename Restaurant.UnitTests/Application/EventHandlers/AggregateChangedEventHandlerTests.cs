using application.EventHandlers;
using application.EventHandlers.Interfaces;
using core.FallbackPolicies;
using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using unitTests.DataFakers;

namespace unitTests.Application.EventHandlers
{
    [TestFixture]
    public abstract class AggregateChangedEventHandlerTests<TAggregate, TKey>
        where TKey : SimpleValueType<int, TKey>
        where TAggregate : Aggregate<TKey>
    {
        protected Mock<IEventInfoStorage<TAggregate, TKey>> _eventInfoStorage;
        protected Mock<ILogger<AggregateChangedEventHandler<TAggregate, TKey>>> _loggerMock;
        protected int _eventId;

        [SetUp]
        public virtual void SetUp()
        {
            _eventInfoStorage = new Mock<IEventInfoStorage<TAggregate, TKey>>();
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
            EventStoragePendingConfigurationSuccess();

            var handler = CreateHandler();
            var action = () => handler.Handle(notification, CancellationToken.None);

            await action.Should().NotThrowAsync();
            _eventInfoStorage.Verify(e => e.StorePendingEventAsync(It.Is<TKey>(x => x.Value == notification.Aggregate.Id!.Value), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task PednigState_Failed_RetryPolicyApplied()
        {
            var notification = CreateValidEvent();
            _eventInfoStorage.Setup(e => e.StorePendingEventAsync(It.IsAny<TKey>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new MockedException());

            var handler = CreateHandler();
            var action = () => handler.Handle(notification, CancellationToken.None);

            await action.Should().NotThrowAsync();
            _eventInfoStorage.Verify(e => e.StorePendingEventAsync(It.Is<TKey>(x => x.Value == notification.Aggregate.Id!.Value), It.IsAny<CancellationToken>()), Times.Exactly(FallbackRetryPolicies.NUMBER_OF_RETRIES + 1));
        }

        [Test]
        public async Task PednigState_Failed_MessageLogged()
        {
            var notification = CreateValidEvent();
            _eventInfoStorage.Setup(e => e.StorePendingEventAsync(It.IsAny<TKey>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new MockedException());
            MockLogger();

            var handler = CreateHandler();
            var action = () => handler.Handle(notification, CancellationToken.None);

            await action.Should().NotThrowAsync();
            _eventInfoStorage.Verify(e => e.StorePendingEventAsync(It.Is<TKey>(x => x.Value == notification.Aggregate.Id!.Value), It.IsAny<CancellationToken>()), Times.Exactly(FallbackRetryPolicies.NUMBER_OF_RETRIES + 1));
            CheckIfLoggedError();
        }

        [Test]
        public async Task Process_Success_StoredSuccessfully()
        {
            var notification = CreateValidEvent();
            EventStoragePendingConfigurationSuccess();
            ConfigureSuccessfulProcessing();
            _eventInfoStorage.Setup(e => e.StoreSuccessAsyncAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = CreateHandler();
            var action = () => handler.Handle(notification, CancellationToken.None);
            await action.Should().NotThrowAsync();

            _eventInfoStorage.Verify(e => e.StoreSuccessAsyncAsync(It.Is<int>(id => id == _eventId), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task Process_CannotBeStoredSuccess_Logged()
        {
            var notification = CreateValidEvent();
            EventStoragePendingConfigurationSuccess();
            ConfigureSuccessfulProcessing();
            _eventInfoStorage.Setup(e => e.StoreSuccessAsyncAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new MockedException());

            var handler = CreateHandler();
            var action = () => handler.Handle(notification, CancellationToken.None);
            await action.Should().NotThrowAsync();

            _eventInfoStorage.Verify(e => e.StoreSuccessAsyncAsync(It.Is<int>(id => id == _eventId), It.IsAny<CancellationToken>()), Times.Exactly(FallbackRetryPolicies.NUMBER_OF_RETRIES + 1));
            CheckIfLoggedError();
        }

        [Test]
        public async Task Process_Failure_EventFailureStored()
        {
            var notification = CreateValidEvent();
            EventStoragePendingConfigurationSuccess();
            ConfigureFailureProcessing();
            _eventInfoStorage.Setup(e => e.StoreFailureAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = CreateHandler();
            var action = () => handler.Handle(notification, CancellationToken.None);
            await action.Should().NotThrowAsync();

            _eventInfoStorage.Verify(e => e.StoreFailureAsync(It.Is<int>(id => id == _eventId), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task Process_FailureCannotBeStored_Logged()
        {
            var notification = CreateValidEvent();
            EventStoragePendingConfigurationSuccess();
            ConfigureFailureProcessing();
            _eventInfoStorage.Setup(e => e.StoreFailureAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var handler = CreateHandler();
            var action = () => handler.Handle(notification, CancellationToken.None);
            await action.Should().NotThrowAsync();

            //TODO: extensions method for retry policy veryfication
            _eventInfoStorage.Verify(e => e.StoreFailureAsync(It.Is<int>(id => id == _eventId), It.IsAny<CancellationToken>()), Times.Exactly(FallbackRetryPolicies.NUMBER_OF_RETRIES + 1));
            CheckIfLoggedError(2);
        }

        protected abstract AggregateChangedEventHandler<TAggregate, TKey> CreateHandler();
        protected abstract AggregateChangedEvent<TAggregate, TKey> CreateValidEvent();
        protected abstract void ConfigureSuccessfulProcessing();
        protected abstract void ConfigureFailureProcessing();

        protected virtual void MockLogger()
        {
            _loggerMock.Setup(l => l.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));
        }

        protected void CheckIfLoggedError(int numberOfTimes = 1)
        {
            _loggerMock.Verify(l => l.Log(It.Is<LogLevel>(lvl => lvl == LogLevel.Error), It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Exactly(numberOfTimes));
        }

        protected void EventStoragePendingConfigurationSuccess()
        {
            _eventInfoStorage.Setup(e => e.StorePendingEventAsync(It.IsAny<TKey>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_eventId);
        }
    }
}
