﻿using application.Common.Extensions;
using application.Restaurants.Commands.Interfaces;
using domain.Common.ValueTypes.Strings;
using domain.Restaurants.Aggregates;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace application.Restaurants.Commands;

//TODO: reduce mappings
internal sealed class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, Result<RestaurantId>>
{
    private readonly IRestaurantRepository _repository;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateRestaurantCommandHandler> _logger;

    public CreateRestaurantCommandHandler(IRestaurantRepository repository, IMediator mediator, ILogger<CreateRestaurantCommandHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<RestaurantId>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var validationResult = CommandValidation(request);
        if (validationResult.IsFailed)
            return validationResult;

        var domainResult = CreateDomainModel(request);
        if (domainResult.IsFailed) return domainResult.ToResult();

        await _repository.CreateAsync(domainResult.Value, cancellationToken);

        await _mediator.PublishEventsAsync<Restaurant, RestaurantId>(domainResult.Value, _logger, cancellationToken);
        return Result.Ok(domainResult.Value.Id!);
    }

    private static Result CommandValidation(CreateRestaurantCommand command)
    {
        if (command is null)
            return Result.Fail("Command cannot be null.");

        if (command.Owner is null)
            return Result.Fail("Owner cannot be null.");

        if (command.OpeningHours is null)
            return Result.Fail("Opening hours cannot be null.");
        var owner = command.Owner;

        if (string.IsNullOrEmpty(owner.Name))
            return Result.Fail("Owners name cannot be empty.");

        if (string.IsNullOrEmpty(owner.Surname))
            return Result.Fail("Owners surname cannot be empty.");

        var ownerAddress = owner.Address;

        if (ownerAddress is null)
            return Result.Fail("Owner address cannot be null.");

        if (string.IsNullOrEmpty(ownerAddress.Street))
            return Result.Fail("Owner street cannot be null.");

        if (string.IsNullOrEmpty(ownerAddress.City))
            return Result.Fail("Owner city cannot be null.");

        var openingHours = command.OpeningHours;

        if (openingHours is null)
            return Result.Fail("Opening hours cannot be null.");

        if (openingHours.WorkingDays.Count == 0)
            return Result.Fail("Working days cannot be empty.");

        if (command.Address is null)
            return Result.Fail("Restaurant address cannot be null.");

        return Result.Ok();
    }

    private static Result<Restaurant> CreateDomainModel(CreateRestaurantCommand request)
    {
        if (string.IsNullOrEmpty(request.Name))
            return Result.Fail("Name cannot be null.");

        var requestOwner = request.Owner;
        var ownerAddressResult = Address.Create(
            new Street(requestOwner!.Address!.Street!),
            new City(requestOwner.Address.City!),
            new Coordinates(requestOwner.Address.XCoordinate, requestOwner.Address.YCoordinate)
        );

        if (ownerAddressResult.IsFailed) return ownerAddressResult.ToResult();

        var owner = Owner.Create(
            new Name(request!.Owner!.Name!),
            new Name(request!.Owner!.Surname!),
            ownerAddressResult.Value
        );

        if(owner.IsFailed) return owner.ToResult();

        var requestOpeningHours = request.OpeningHours;

        var workingDaysResults = requestOpeningHours!.WorkingDays.Select(wd =>
            WorkingDay.Create(
                wd.Day,
                TimeOnly.FromDateTime(wd.From),
                TimeOnly.FromDateTime(wd.To)
            )).ToList();

        var failedResult = workingDaysResults.Where(r => r.IsFailed).ToList();

        if (failedResult.Count > 0) return Result.Fail(failedResult.SelectMany(r => r.Errors).ToList());

        var openingHours = OpeningHours.Create(
            workingDaysResults.Select(r => r.Value).ToList()
        );

        if (openingHours.IsFailed) return openingHours.ToResult();

        var restaurantAddressResult = Address.Create(new Street(request.Address.Street!), new City(request.Address.City!), new Coordinates(request.Address.XCoordinate, request.Address.YCoordinate));

        if(restaurantAddressResult.IsFailed) return restaurantAddressResult.ToResult();

        return Restaurant.Create(new Name(request.Name), owner.Value, openingHours.Value, restaurantAddressResult.Value);
    }
}


public record CreateRestaurantCommand : IRequest<Result<RestaurantId>>
{
    public CreateRestaurantCommand(string? Name, CreateOwnerCommand? Owner, OpeningHoursCommand? OpeningHours, CreateAddressCommand? Address)
    {
        this.Name = Name;
        this.Owner = Owner;
        this.OpeningHours = OpeningHours;
        this.Address = Address;
    }

    public string? Name { get; init; }
    public CreateOwnerCommand? Owner { get; init; }
    public OpeningHoursCommand? OpeningHours { get; init; }
    public CreateAddressCommand? Address { get; init; }
}

public record CreateOwnerCommand
{
    public CreateOwnerCommand(string? Name, string? Surname, CreateAddressCommand? Address)
    {
        this.Name = Name;
        this.Surname = Surname;
        this.Address = Address;
    }

    public string? Name { get; init; }
    public string? Surname { get; init; }
    public CreateAddressCommand? Address { get; init; }
}

public record CreateAddressCommand
{
    public CreateAddressCommand(string? Street, string? City, double XCoordinate, double YCoordinate)
    {
        this.Street = Street;
        this.City = City;
        this.XCoordinate = XCoordinate;
        this.YCoordinate = YCoordinate;
    }

    public string? Street { get; init; }
    public string? City { get; init; }
    public double XCoordinate { get; init; }
    public double YCoordinate { get; init; }
}

public record OpeningHoursCommand
{
    public OpeningHoursCommand(List<WorkingDayCommand> WorkingDays)
    {
        this.WorkingDays = WorkingDays;
    }

    public List<WorkingDayCommand> WorkingDays { get; init; }
}

public record  WorkingDayCommand
{
    public WorkingDayCommand(DayOfWeek Day, DateTime From, DateTime To)
    {
        this.Day = Day;
        this.From = From;
        this.To = To;
    }

    public DayOfWeek Day { get; init; }
    public DateTime From { get; init; }
    public DateTime To { get; init; }
}