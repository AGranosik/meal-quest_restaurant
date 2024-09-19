using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace application.Common.Extensions
{
    public static class MediatorExtensions
    {
        public static async Task PublishEventsAsync<TEntity, TKey>(this IMediator mediator, TEntity entity, ILogger logger, CancellationToken cancellationToken)
            where TEntity : Entity<TKey>
            where TKey : ValueObject<TKey>
        {
            DomainEvent? @event = null;
            try 
            {
                var events = entity.GetEvents();
                for(int i = 0; i < events.Count; i++)
                {
                    @event = events[i];
                    await mediator.Publish(@event, cancellationToken);
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error publishing messages for {Aggregate} with Id: {StreamId}", nameof(entity), @event!.StreamId);
            }
        }
    }
}
