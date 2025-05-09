﻿using application.EventHandlers;
using application.EventHandlers.Restaurants;
using application.Menus.Commands.Interfaces;
using core.Operations.FallbackPolicies;
using domain.Menus.ValueObjects;
using domain.Menus.ValueObjects.Identifiers;
using domain.Restaurants.Aggregates;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;
using Moq;
using sharedTests.DataFakers;

namespace unitTests.Application.EventHandlers;

[TestFixture]
internal class RestaurantChangedEventHandlerTests : AggregateChangedEventHandlerTests<Restaurant, RestaurantId>
{
    private Mock<IMenuRepository> _menuRepositoryMock;
    public override void SetUp()
    {
        _menuRepositoryMock = new Mock<IMenuRepository>();
        base.SetUp();
    }

    [Test]
    public async Task Processing_Success()
    {
        ConfigureSuccessfulProcessing();
        var @event = CreateValidEvent();
        var handler = CreateHandler();
        var action = () => handler.Handle(@event, CancellationToken.None);

        await action.Should().NotThrowAsync();
        _menuRepositoryMock.Verify(r => r.CreateRestaurantAsync(It.Is<MenuRestaurant>(restaurant => restaurant.Id!.Value == @event.Aggregate.Id!.Value), It.IsAny<CancellationToken>()), Times.Once());
    }


    [Test]
    public async Task Processing_RetryPolicyApplied_Called()
    {
        ConfigureMenuRepositoryFailure();
        MockLogger();

        var @event = CreateValidEvent();
        var handler = CreateHandler();
        var action = () => handler.Handle(@event, CancellationToken.None);

        await action.Should().NotThrowAsync();
        _menuRepositoryMock.Verify(r => r.CreateRestaurantAsync(It.Is<MenuRestaurant>(restaurant => restaurant.Id!.Value == @event.Aggregate.Id!.Value), It.IsAny<CancellationToken>()), Times.Exactly(FallbackRetryPolicies.NUMBER_OF_RETRIES + 1));
        CheckIfLoggedError();
    }



    protected override AggregateChangedEventHandler<Restaurant, RestaurantId> CreateHandler()
        => new RestaurantChangedEventHandler(EventInfoStorage.Object, LoggerMock.Object, EmitterMock.Object, _menuRepositoryMock.Object);

    protected override AggregateChangedEvent<Restaurant, RestaurantId> CreateValidEvent()
        => new(RestaurantDataFaker.ValidRestaurant());

    protected override void ConfigureSuccessfulProcessing()
    {
        ConfigureMenuRepositorySuccessful();
        EventEmitterConfigurationSuccess();
    }

    protected override void ConfigureFailureProcessing()
        => ConfigureMenuRepositoryFailure();

    private void ConfigureMenuRepositorySuccessful()
        => _menuRepositoryMock.Setup(r => r.CreateRestaurantAsync(It.IsAny<MenuRestaurant>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

    private void ConfigureMenuRepositoryFailure()
        => _menuRepositoryMock.Setup(r => r.CreateRestaurantAsync(It.IsAny<MenuRestaurant>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
}