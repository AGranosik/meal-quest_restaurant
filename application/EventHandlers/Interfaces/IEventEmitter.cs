using domain.Restaurants.Aggregates;
using FluentResults;

namespace application.EventHandlers.Interfaces
{
    public interface IEventEmitter<T>
        where T : class
    {
        Task<Result> EmitEvents(Restaurant @event, CancellationToken cancellationToken);
    }
}
