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

            return Result.Fail("s");
        }

        private Result CommandValidation(CreateRestaurantCommand command)
        {
            if (command is null)
                return Result.Fail("Command cannot be null.");

            return Result.Ok();
        }
    }


    public record CreateRestaurantCommand(CreateOwnerCommand? Owner, OpeningHoursCommand OpeningHours) : IRequest<Result<RestaurantId>>;

    public record CreateOwnerCommand(string? Name, string? Surname, CreateAddressCommand? Address);

    public record CreateAddressCommand (string? Street, string? City, double XCoordinate, double YCoordinate);

    public record OpeningHoursCommand(List<WorkingDayCommand> WorkingDays);

    public record  WorkingDayCommand(DayOfWeek Day, DateTime From, DateTime To);
}
