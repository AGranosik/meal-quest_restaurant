using application.EventHandlers;
using application.EventHandlers.Interfaces;
using domain.Common.DomainImplementationTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
using FluentAssertions;
using FluentResults;
using Microsoft.Extensions.Logging;
using Moq;
using sharedTests.DataFakers;
using sharedTests.MocksExtensions;

namespace unitTests.Application.EventHandlers;

[TestFixture]
internal abstract class AggregateChangedEventHandlerTests<TAggregate, TKey>
    where TKey : SimpleValueType<int, TKey>
    where TAggregate : Aggregate<TKey>
{
    protected Mock<IEventInfoStorage<TAggregate, TKey>> EventInfoStorage;
    protected Mock<ILogger<AggregateChangedEventHandler<TAggregate, TKey>>> LoggerMock;
    protected Mock<IEventEmitter<TAggregate>> EmitterMock;
    protected int EventId;

    [SetUp]
    public virtual void SetUp()
    {
        EventInfoStorage = new Mock<IEventInfoStorage<TAggregate, TKey>>();
        LoggerMock = new Mock<ILogger<AggregateChangedEventHandler<TAggregate, TKey>>>();
        EmitterMock = new Mock<IEventEmitter<TAggregate>>();
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
        EventEmitterConfigurationSuccess();

        var handler = CreateHandler();
        var action = () => handler.Handle(notification, CancellationToken.None);

        await action.Should().NotThrowAsync();
        EventInfoStorage.Verify(e => e.StorePendingEventAsync(It.Is<TAggregate>(x => x.Id!.Value == notification.Aggregate.Id!.Value), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task PednigState_Failed_RetryPolicyApplied()
    {
        var notification = CreateValidEvent();
        EventInfoStorage.Setup(e => e.StorePendingEventAsync(It.IsAny<TAggregate>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new MockedException());
        EventEmitterConfigurationSuccess();

        var handler = CreateHandler();
        var action = () => handler.Handle(notification, CancellationToken.None);

        await action.Should().NotThrowAsync();
        EventInfoStorage.Verify(e => e.StorePendingEventAsync(It.Is<TAggregate>(x => x.Id!.Value == notification.Aggregate.Id!.Value), It.IsAny<CancellationToken>()), RetryPolicyExtensions.NumberOfAppRetries());
    }

    [Test]
    public async Task PednigState_Failed_MessageLogged()
    {
        var notification = CreateValidEvent();
        EventInfoStorage.Setup(e => e.StorePendingEventAsync(It.IsAny<TAggregate>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new MockedException());
        MockLogger();
        EventEmitterConfigurationSuccess();

        var handler = CreateHandler();
        var action = () => handler.Handle(notification, CancellationToken.None);

        await action.Should().NotThrowAsync();
        EventInfoStorage.Verify(e => e.StorePendingEventAsync(It.Is<TAggregate>(x => x.Id!.Value == notification.Aggregate.Id!.Value), It.IsAny<CancellationToken>()), RetryPolicyExtensions.NumberOfAppRetries());
        CheckIfLoggedError();
    }

    [Test]
    public async Task Process_Success_StoredSuccessfully()
    {
        var notification = CreateValidEvent();
        EventStoragePendingConfigurationSuccess();
        ConfigureSuccessfulProcessing();
        EventEmitterConfigurationSuccess();

        EventInfoStorage.Setup(e => e.StoreSuccessAsyncAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = CreateHandler();
        var action = () => handler.Handle(notification, CancellationToken.None);
        await action.Should().NotThrowAsync();

        EventInfoStorage.Verify(e => e.StoreSuccessAsyncAsync(It.Is<int>(id => id == EventId), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task Process_CannotBeStoredSuccess_Logged()
    {
        var notification = CreateValidEvent();
        EventStoragePendingConfigurationSuccess();
        ConfigureSuccessfulProcessing();
        EventEmitterConfigurationSuccess();
        EventInfoStorage.Setup(e => e.StoreSuccessAsyncAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new MockedException());

        var handler = CreateHandler();
        var action = () => handler.Handle(notification, CancellationToken.None);
        await action.Should().NotThrowAsync();

        EventInfoStorage.Verify(e => e.StoreSuccessAsyncAsync(It.Is<int>(id => id == EventId), It.IsAny<CancellationToken>()), RetryPolicyExtensions.NumberOfAppRetries());
        CheckIfLoggedError();
    }

    [Test]
    public async Task Process_Failure_EventFailureStored()
    {
        var notification = CreateValidEvent();
        EventStoragePendingConfigurationSuccess();
        ConfigureFailureProcessing();
        EventEmitterConfigurationSuccess();
        EventInfoStorage.Setup(e => e.StoreFailureAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = CreateHandler();
        var action = () => handler.Handle(notification, CancellationToken.None);
        await action.Should().NotThrowAsync();

        EventInfoStorage.Verify(e => e.StoreFailureAsync(It.Is<int>(id => id == EventId), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task Process_FailureCannotBeStored_Logged()
    {
        var notification = CreateValidEvent();
        EventStoragePendingConfigurationSuccess();
        ConfigureFailureProcessing();
        EventEmitterConfigurationSuccess();
        EventInfoStorage.Setup(e => e.StoreFailureAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var handler = CreateHandler();
        var action = () => handler.Handle(notification, CancellationToken.None);
        await action.Should().NotThrowAsync();

        EventInfoStorage.Verify(e => e.StoreFailureAsync(It.Is<int>(id => id == EventId), It.IsAny<CancellationToken>()), RetryPolicyExtensions.NumberOfAppRetries());
        CheckIfLoggedError(2);
    }

    [Test]
    public async Task Process_EmitEmissionFailrue_EventFailureStored()
    {
        var notification = CreateValidEvent();
        EventStoragePendingConfigurationSuccess();
        ConfigureSuccessfulProcessing();
        EventEmitterConfigurationFailure();

        EventInfoStorage.Setup(e => e.StoreFailureAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = CreateHandler();
        var action = () => handler.Handle(notification, CancellationToken.None);
        await action.Should().NotThrowAsync();
        EmitterMock.Verify(e => e.EmitEvents(It.IsAny<TAggregate>(), It.IsAny<CancellationToken>()));

        EventInfoStorage.Verify(e => e.StoreFailureAsync(It.Is<int>(id => id == EventId), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task Process_EmitEmissionFailrue_EventFailureLogged()
    {
        var notification = CreateValidEvent();
        EventStoragePendingConfigurationSuccess();
        ConfigureSuccessfulProcessing();
        EventEmitterConfigurationFailure();

        EventInfoStorage.Setup(e => e.StoreFailureAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var handler = CreateHandler();
        var action = () => handler.Handle(notification, CancellationToken.None);
        await action.Should().NotThrowAsync();
        EmitterMock.Verify(e => e.EmitEvents(It.IsAny<TAggregate>(), It.IsAny<CancellationToken>()));

        EventInfoStorage.Verify(e => e.StoreFailureAsync(It.Is<int>(id => id == EventId), It.IsAny<CancellationToken>()), RetryPolicyExtensions.NumberOfAppRetries());
    }

    protected abstract AggregateChangedEventHandler<TAggregate, TKey> CreateHandler();
    protected abstract AggregateChangedEvent<TAggregate, TKey> CreateValidEvent();
    protected abstract void ConfigureSuccessfulProcessing();
    protected abstract void ConfigureFailureProcessing();

    protected virtual void MockLogger()
    {
        LoggerMock.Setup(l => l.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()));
    }

    protected void CheckIfLoggedError(int numberOfTimes = 1)
    {
        LoggerMock.Verify(l => l.Log(It.Is<LogLevel>(lvl => lvl == LogLevel.Error), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Exactly(numberOfTimes));
    }

    protected void EventStoragePendingConfigurationSuccess()
    {
        EventInfoStorage.Setup(e => e.StorePendingEventAsync(It.IsAny<TAggregate>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(EventId);
    }

    protected void EventEmitterConfigurationSuccess()
    {
        EmitterMock.Setup(e => e.EmitEvents(It.IsAny<TAggregate>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
    }

    protected void EventEmitterConfigurationFailure()
    {
        EmitterMock.Setup(e => e.EmitEvents(It.IsAny<TAggregate>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Mock failed."));
    }
}