using domain.Common.BaseTypes;
using MediatR;

namespace application.Common.Extensions
{
    public static class MediatorExtensions
    {
        public static async Task PublishEventsAsync<TEntity, TKey>(this IMediator mediator, TEntity entity, CancellationToken cancellationToken)
            where TEntity : Entity<TKey>
            where TKey : ValueObject<TKey>
        {
            var events = entity.GetEvents();
            foreach (var @event in events)
                await mediator.Publish(@event, cancellationToken);
        }
    }
}
