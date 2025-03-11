using application.EventHandlers;
using domain.Common.DomainImplementationTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace application.Common.Extensions;

internal static class MediatorExtensions
{
    public static async Task PublishEventsAsync<TAggregate, TKey>(this IMediator mediator, TAggregate entity, ILogger logger, CancellationToken cancellationToken)
        where TAggregate : Aggregate<TKey>
        where TKey : SimpleValueType<int, TKey>
    {
        try 
        {
            await mediator.Publish(new AggregateChangedEvent<TAggregate, TKey>(entity), cancellationToken);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Error publishing messages for {Aggregate} with Id: {Id}", nameof(entity), entity.Id!.Value);
        }
    }
}