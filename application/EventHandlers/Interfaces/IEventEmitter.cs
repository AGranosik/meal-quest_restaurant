using FluentResults;

namespace application.EventHandlers.Interfaces
{
    // TODO: make internal classes which should be
    public interface IEventEmitter<T>
        where T : class
    {
        Task<Result> EmitEvents(T @event, CancellationToken cancellationToken);
    }
}
