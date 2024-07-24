﻿using domain.Common.ValueTypes.Strings;
using domain.Restaurants.Aggregates;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentResults;
using infrastructure.Database.RestaurantContext.Repositories;
using MediatR;

namespace application.Restaurants.Commands
{

    public class CreateRestaurantCommandHandler(IRestaurantRepository repo) : IRequestHandler<CreateRestaurantCommand, Result<RestaurantId>>
    {
        private readonly IRestaurantRepository _repo = repo ?? throw new ArgumentNullException(nameof(IRestaurantRepository));

        public async Task<Result<RestaurantId>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var validationResult = CommandValidation(request);
            if (validationResult.IsFailed)
                return validationResult;

            var domainResult = CreateDomainModel(request);
            if (domainResult.IsFailed) return domainResult.ToResult();

            await _repo.CreateAsync(domainResult.Value, cancellationToken);
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

            return Result.Ok();
        }

        private static Result<Restaurant> CreateDomainModel(CreateRestaurantCommand request)
        {
            var requestOwner = request.Owner;
            var addressResult = Address.Create(
                new Street(requestOwner!.Address!.Street!),
                new City(requestOwner!.Address!.City!),
                new Coordinates(requestOwner.Address.XCoordinate, requestOwner.Address.YCoordinate)
            );

            if (addressResult.IsFailed) return addressResult.ToResult();

            var owner = Owner.Create(
                new Name(request!.Owner!.Name!),
                new Name(request!.Owner!.Surname!),
                addressResult.Value
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

            return Restaurant.Create(owner.Value, openingHours.Value);
        }
    }


    public record CreateRestaurantCommand(CreateOwnerCommand? Owner, OpeningHoursCommand? OpeningHours) : IRequest<Result<RestaurantId>>;

    public record CreateOwnerCommand(string? Name, string? Surname, CreateAddressCommand? Address);

    public record CreateAddressCommand (string? Street, string? City, double XCoordinate, double YCoordinate);

    public record OpeningHoursCommand(List<WorkingDayCommand> WorkingDays);

    public record  WorkingDayCommand(DayOfWeek Day, DateTime From, DateTime To);
}