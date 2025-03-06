using FluentResults;

namespace application.EventHandlers.Interfaces;

public interface IEventEmitter<T>
    where T : class
{
    Task<Result> EmitEvents(T @event, CancellationToken cancellationToken);
}